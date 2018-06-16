using Benchmarks.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;
using static Benchmarks.Constants;
using static Benchmarks.Sql.ResourceConstants;
using static System.Console;
using static System.String;

namespace Benchmarks
{
    internal static class DatabasePreparer
    {
        private const byte Iterations = 10;

        public static async Task PrepareDatabase()
        {
            await CreateSchema();
            await CreateData();
        }

        private static async Task CreateData()
        {
            WriteLine($"Adding entity B & C data");
            using (var db = new AppDbContext())
            {
                for (byte i = 0; i < Iterations; i++)
                {
                    var @string = i.ToString();
                    db.EntityB.Add(new EntityB { MyProperty = @string });
                    db.EntityC.Add(new EntityC { MyProperty = @string });
                    db.MemoryOptimizedEntityB.Add(new MemoryOptimizedEntityB { MyProperty = @string });
                    db.MemoryOptimizedEntityC.Add(new MemoryOptimizedEntityC { MyProperty = @string });
                }
                await db.SaveChangesAsync();
                WriteLine($"Adding entity A data");
                for (byte i = 1; i <= Iterations; i++)
                    for (byte j = 1; j <= Iterations; j++)
                    {
                        db.EntityA.Add(new EntityA { EntityBId = i, EntityCId = j });
                        db.MemoryOptimizedEntityA.Add(new MemoryOptimizedEntityA { EntityBId = i, EntityCId = j });
                    }
                await db.SaveChangesAsync();
            }
        }

        private static async Task CreateSchema()
        {
            using (var db = new AppDbContext())
            {
                var database = db.Database;
                WriteLine("Dropping db");
                await database.EnsureDeletedAsync();
                WriteLine("Creating db");
                await database.MigrateAsync();
                WriteLine("Set default db for login");
                await database.ExecuteSqlCommandAsync(Concat("ALTER LOGIN [", DatabaseUserName, "] WITH DEFAULT_DATABASE = [", DatabaseDbName, "]"));
                WriteLine("Creating stored procedure");
                await CreateStoredProcedures(database);
            }
        }

        private static async Task CreateStoredProcedures(DatabaseFacade database)
        {
            await CreateNonNativelyCompiledProcedures(database);
            await CreateNativelyCompiledProcedures(database);
        }

        private static async Task CreateNativelyCompiledProcedures(DatabaseFacade database)
        {
            await database.ExecuteSqlCommandAsync(GetNativeCompiled_AsInt_SqlText());
            await database.ExecuteSqlCommandAsync(GetNativeCompiled_Bytes_SqlText());
        }

        private static async Task CreateNonNativelyCompiledProcedures(DatabaseFacade database)
        {
            await database.ExecuteSqlCommandAsync(GetNotNativeCompiled_AsInt_SqlText());
            await database.ExecuteSqlCommandAsync(GetNotNativeCompiled_Bytes_SqlText());
        }
    }
}

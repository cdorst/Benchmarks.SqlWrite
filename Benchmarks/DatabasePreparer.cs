using Benchmarks.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading.Tasks;
using static Benchmarks.Sql.ResourceConstants;

namespace Benchmarks
{
    internal static class DatabasePreparer
    {
        public static async Task PrepareDatabase()
        {
            await CreateSchema();
            await CreateData();
        }

        private static async Task CreateData()
        {
            using (var db = new AppDbContext())
            {
                Console.WriteLine("Adding EntityB & EntityC data");
                byte iterations = 10;
                for (byte i = 0; i < iterations; i++)
                {
                    var @string = i.ToString();
                    db.EntityB.Add(new EntityB { MyProperty = @string });
                    db.EntityC.Add(new EntityC { MyProperty = @string });
                }
                await db.SaveChangesAsync();
                Console.WriteLine("Adding EntityA data");
                for (byte i = 1; i <= iterations; i++)
                    for (byte j = 1; j <= iterations; j++)
                        db.EntityA.Add(new EntityA { EntityBId = i, EntityCId = j });
                await db.SaveChangesAsync();
            }
        }

        private static async Task CreateSchema()
        {
            using (var db = new AppDbContext())
            {
                var database = db.Database;
                Console.WriteLine("Dropping db");
                await database.EnsureDeletedAsync();
                Console.WriteLine("Creating db");
                await database.MigrateAsync();
                Console.WriteLine("Set default db for login");
                await database.ExecuteSqlCommandAsync(string.Concat("ALTER LOGIN [", Constants.DatabaseUserName, "] WITH DEFAULT_DATABASE = [", Constants.DatabaseDbName, "]"));
                Console.WriteLine("Creating stored procedure");
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

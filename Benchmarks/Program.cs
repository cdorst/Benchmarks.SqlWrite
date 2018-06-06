using Benchmarks.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static BenchmarkDotNet.Running.BenchmarkRunner;

namespace Benchmarks
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await PrepareDatabase();
            Run<Tests>();
            Run<TestsAsBytes>();
        }

        private static async Task PrepareDatabase()
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
                await database.ExecuteSqlCommandAsync(@"CREATE PROCEDURE [dbo].[SaveEntityA]
    @EntityBId smallint,
    @EntityCId bigint
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Id [int];

    MERGE INTO [dbo].[EntityA] AS [target]
    USING (VALUES (@EntityBId, @EntityCId))
        AS [source] (EntityBId, EntityCId)
        ON [target].[EntityBId] = [source].[EntityBId]
        AND [target].[EntityCId] = [source].[EntityCId]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (EntityBId, EntityCId)
        VALUES (EntityBId, EntityCId)
    WHEN MATCHED THEN
        UPDATE SET @Id = [target].[Id];

    IF @Id IS NULL SET @Id = CAST(SCOPE_IDENTITY() as [int]);
    SELECT @Id;
END");
                await database.ExecuteSqlCommandAsync(@"CREATE PROCEDURE [dbo].[SaveE_Bytes]
    @EntityBId smallint,
    @EntityCId bigint
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Id [int];

    MERGE INTO [dbo].[EntityA] AS [target]
    USING (VALUES (@EntityBId, @EntityCId))
        AS [source] (EntityBId, EntityCId)
        ON [target].[EntityBId] = [source].[EntityBId]
        AND [target].[EntityCId] = [source].[EntityCId]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (EntityBId, EntityCId)
        VALUES (EntityBId, EntityCId)
    WHEN MATCHED THEN
        UPDATE SET @Id = [target].[Id];

    IF @Id IS NULL SET @Id = CAST(SCOPE_IDENTITY() as [int]);
    SELECT CONVERT(BINARY(4), @Id);
END");
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
    }
}

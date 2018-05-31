using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Benchmarks.Model;
using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Benchmarks
{
    [SimpleJob(10)]
    [MemoryDiagnoser]
    [RankColumn]
    [RPlotExporter]
    public class Tests
    {
        [Benchmark]
        public async Task<int> DapperDotNet_NewCommandDefinition()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<int>(new CommandDefinition(Constants.Command, Constants.Entity.ToSqlSaveParameters(), commandType: CommandType.StoredProcedure));
            }
        }

        [Benchmark]
        public async Task<int> DapperDotNet_TextCommand()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<int>(GetTextCommand(Constants.Entity), commandType: CommandType.Text);
            }
        }

        [Benchmark]
        public async Task<int> DapperDotNet_Parameters()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<int>(Constants.Command, Constants.Entity.ToSqlSaveParameters(), commandType: CommandType.StoredProcedure);
            }
        }

        [Benchmark]
        public async Task<int> AdoSqlCommand_WithParameters()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(Constants.Command, connection) { CommandType = CommandType.StoredProcedure })
            {
                var parameters = command.Parameters;
                parameters.AddWithValue(nameof(EntityA.EntityBId), Constants.Entity.EntityBId);
                parameters.AddWithValue(nameof(EntityA.EntityCId), Constants.Entity.EntityCId);
                await connection.OpenAsync();
                return (int)(await command.ExecuteScalarAsync());
            }
        }

        [Benchmark]
        public async Task<int> AdoSqlCommand_Text()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(GetTextCommand(Constants.Entity), connection) { CommandType = CommandType.Text })
            {
                await connection.OpenAsync();
                return (int)(await command.ExecuteScalarAsync());
            }
        }

        /* TODO: Implement TDS protocol

        [Benchmark]
        public int TdsCommand()
        {
            ReadOnlySpan<byte> parameterBytes = Constants.Entity.ToSqlParameterReadOnlySpan();
            using (var query = new CurriedTdsQuery(in parameterBytes))
            {
                Span<byte> buffer = stackalloc byte[Constants.Int32SizeByte];
                query.ExecuteScalar(out buffer);
                return Common.Extensions.Memory.SpanIntegerExtensions.GetInt32(buffer);
            }
        }

        */

        private static string GetTextCommand(EntityA entity)
            => string.Concat("EXEC ", Constants.Command, " ", entity.EntityBId.ToString(), ",", entity.EntityCId.ToString());
    }
}

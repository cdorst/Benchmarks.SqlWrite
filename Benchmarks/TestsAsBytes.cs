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
    public class TestsAsBytes
    {
        [Benchmark]
        public async Task<byte[]> DapperDotNet_NewCommandDefinition_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(new CommandDefinition(Constants.BytesCommand, Constants.Entity.ToSqlSaveParameters(), commandType: CommandType.StoredProcedure));
            }
        }

        [Benchmark]
        public async Task<byte[]> DapperDotNet_TextCommand_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(GetTextCommand(in Constants.Entity), commandType: CommandType.Text);
            }
        }

        [Benchmark]
        public async Task<byte[]> DapperDotNet_Parameters_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(Constants.BytesCommand, Constants.Entity.ToSqlSaveParameters(), commandType: CommandType.StoredProcedure);
            }
        }

        [Benchmark]
        public async Task<byte[]> AdoSqlCommand_WithParameters_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(Constants.BytesCommand, connection) { CommandType = CommandType.StoredProcedure })
            {
                var parameters = command.Parameters;
                parameters.AddWithValue(nameof(EntityA.EntityBId), Constants.Entity.EntityBId);
                parameters.AddWithValue(nameof(EntityA.EntityCId), Constants.Entity.EntityCId);
                await connection.OpenAsync();
                return BitConverter.GetBytes((int)(await command.ExecuteScalarAsync()));
            }
        }

        [Benchmark]
        public async Task<byte[]> AdoSqlCommand_Text_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(GetTextCommand(in Constants.Entity), connection) { CommandType = CommandType.Text })
            {
                await connection.OpenAsync();
                return BitConverter.GetBytes((int)(await command.ExecuteScalarAsync()));
            }
        }

        /* TODO: Implement TDS protocol

        [Benchmark]
        public static ReadOnlySpan<byte> TdsCommand(in Entity entity)
        {
            ReadOnlySpan<byte> parameterBytes = entity.ToSqlParameterReadOnlySpan();
            using (var query = new CurriedTdsQuery(in parameterBytes))
            {
                Span<byte> buffer = stackalloc byte[Constants.Int32SizeByte];
                query.ExecuteScalar(out buffer);
                return buffer; // cast? buffer.ToReadOnlySpan() / buffer.ToArray()
            }
        }

        */

        private static string GetTextCommand(in EntityA entity)
            => string.Concat(Constants.ExecBytesCommand, entity.EntityBId.ToString(), ",", entity.EntityCId.ToString());
    }
}

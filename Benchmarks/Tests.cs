using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Benchmarks.Model;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Benchmarks
{
    [SimpleJob(20)]
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
        public async Task<byte[]> DapperDotNet_NewCommandDefinition_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(new CommandDefinition(Constants.BytesCommand, Constants.Entity.ToSqlSaveParameters(), commandType: CommandType.StoredProcedure));
            }
        }

        [Benchmark]
        public async Task<int> DapperDotNet_TextCommand()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<int>(GetTextCommand(in Constants.Entity));
            }
        }

        [Benchmark]
        public async Task<byte[]> DapperDotNet_TextCommand_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(GetAsBytesTextCommand(in Constants.Entity));
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
        public async Task<byte[]> DapperDotNet_Parameters_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(Constants.BytesCommand, Constants.Entity.ToSqlSaveParameters(), commandType: CommandType.StoredProcedure);
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
        public async Task<byte[]> AdoSqlCommand_WithParameters_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(Constants.BytesCommand, connection) { CommandType = CommandType.StoredProcedure })
            {
                var parameters = command.Parameters;
                parameters.AddWithValue(nameof(EntityA.EntityBId), Constants.Entity.EntityBId);
                parameters.AddWithValue(nameof(EntityA.EntityCId), Constants.Entity.EntityCId);
                await connection.OpenAsync();
                return (await command.ExecuteScalarAsync()) as byte[];
            }
        }

        [Benchmark]
        public async Task<int> AdoSqlCommand_Text()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(GetTextCommand(in Constants.Entity), connection))
            {
                await connection.OpenAsync();
                return (int)(await command.ExecuteScalarAsync());
            }
        }

        [Benchmark]
        public async Task<byte[]> AdoSqlCommand_Text_AsBytes()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(GetAsBytesTextCommand(in Constants.Entity), connection))
            {
                await connection.OpenAsync();
                return (await command.ExecuteScalarAsync()) as byte[];
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

        [Benchmark]
        public static ReadOnlySpan<byte> TdsCommand_AsBytes()
        {
            ReadOnlySpan<byte> parameterBytes = Constants.Entity.ToSqlParameterReadOnlySpan();
            using (var query = new CurriedTdsQuery(in parameterBytes))
            {
                Span<byte> buffer = stackalloc byte[Constants.Int32SizeByte];
                query.ExecuteScalar(out buffer);
                return buffer; // cast? buffer.ToReadOnlySpan() / buffer.ToArray()
            }
        }

        */

        private static string GetAsBytesTextCommand(in EntityA entity)
            => string.Concat(Constants.ExecBytesCommand, entity.EntityBId.ToString(), ",", entity.EntityCId.ToString());

        private static string GetTextCommand(in EntityA entity)
            => string.Concat(Constants.ExecCommand, entity.EntityBId.ToString(), ",", entity.EntityCId.ToString());
    }
}

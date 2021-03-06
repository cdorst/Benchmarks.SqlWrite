﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Benchmarks.Model;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static Benchmarks.Constants;
using static System.String;

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
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<int>(new CommandDefinition(Command, Entity.ToSqlSaveParameters(), commandType: StoredProcedure));
            }
        }

        [Benchmark]
        public async Task<byte[]> DapperDotNet_NewCommandDefinition_AsBytes()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(new CommandDefinition(BytesCommand, Entity.ToSqlSaveParameters(), commandType: StoredProcedure));
            }
        }

        [Benchmark]
        public async Task<int> DapperDotNet_NewCommandDefinition_Text()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<int>(new CommandDefinition(IntTextCommand(in Entity), commandType: Text));
            }
        }

        [Benchmark]
        public async Task<byte[]> DapperDotNet_NewCommandDefinition_Text_AsBytes()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(new CommandDefinition(AsBytesTextCommand(in Entity), commandType: Text));
            }
        }

        [Benchmark]
        public async Task<int> DapperDotNet_NewCommandDefinition_Text_NativeCompiled()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<int>(new CommandDefinition(NativeIntTextCommand(in MemoryOptimizedEntity), commandType: Text));
            }
        }

        [Benchmark]
        public async Task<byte[]> DapperDotNet_NewCommandDefinition_Text_AsBytes_NativeCompiled()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(new CommandDefinition(NativeAsBytesTextCommand(in MemoryOptimizedEntity), commandType: Text));
            }
        }

        [Benchmark]
        public async Task<int> DapperDotNet_TextCommand()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<int>(IntTextCommand(in Entity));
            }
        }

        [Benchmark]
        public async Task<byte[]> DapperDotNet_TextCommand_AsBytes()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(AsBytesTextCommand(in Entity));
            }
        }

        [Benchmark]
        public async Task<int> DapperDotNet_Parameters()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<int>(Command, Entity.ToSqlSaveParameters(), commandType: StoredProcedure);
            }
        }

        [Benchmark]
        public async Task<byte[]> DapperDotNet_Parameters_AsBytes()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<byte[]>(BytesCommand, Entity.ToSqlSaveParameters(), commandType: StoredProcedure);
            }
        }

        [Benchmark]
        public async Task<int> AdoSqlCommand_WithParameters()
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(Command, connection) { CommandType = StoredProcedure })
            {
                var parameters = command.Parameters;
                parameters.AddWithValue(nameof(EntityA.EntityBId), Entity.EntityBId);
                parameters.AddWithValue(nameof(EntityA.EntityCId), Entity.EntityCId);
                await connection.OpenAsync();
                return (int)(await command.ExecuteScalarAsync());
            }
        }

        [Benchmark]
        public async Task<byte[]> AdoSqlCommand_WithParameters_AsBytes()
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(BytesCommand, connection) { CommandType = StoredProcedure })
            {
                var parameters = command.Parameters;
                parameters.AddWithValue(nameof(EntityA.EntityBId), Entity.EntityBId);
                parameters.AddWithValue(nameof(EntityA.EntityCId), Entity.EntityCId);
                await connection.OpenAsync();
                return (await command.ExecuteScalarAsync()) as byte[];
            }
        }

        [Benchmark]
        public async Task<int> AdoSqlCommand_Text()
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(IntTextCommand(in Entity), connection))
            {
                await connection.OpenAsync();
                return (int)(await command.ExecuteScalarAsync());
            }
        }

        [Benchmark]
        public async Task<byte[]> AdoSqlCommand_Text_AsBytes()
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(AsBytesTextCommand(in Entity), connection))
            {
                await connection.OpenAsync();
                return (await command.ExecuteScalarAsync()) as byte[];
            }
        }

        [Benchmark]
        public async Task<int> AdoSqlCommand_Text_NativeCompiled()
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(NativeIntTextCommand(in MemoryOptimizedEntity), connection))
            {
                await connection.OpenAsync();
                return (int)(await command.ExecuteScalarAsync());
            }
        }

        [Benchmark]
        public async Task<byte[]> AdoSqlCommand_Text_AsBytes_NativeCompiled()
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(NativeAsBytesTextCommand(in MemoryOptimizedEntity), connection))
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

        private static string AsBytesTextCommand(in EntityA entity)
            => Concat(ExecBytesCommand, entity.EntityBId.ToString(), ",", entity.EntityCId.ToString());

        private static string IntTextCommand(in EntityA entity)
            => Concat(ExecCommand, entity.EntityBId.ToString(), ",", entity.EntityCId.ToString());

        private static string NativeAsBytesTextCommand(in MemoryOptimizedEntityA entity)
            => Concat(NativeExecBytesCommand, entity.EntityBId.ToString(), ",", entity.EntityCId.ToString());

        private static string NativeIntTextCommand(in MemoryOptimizedEntityA entity)
            => Concat(NativeExecCommand, entity.EntityBId.ToString(), ",", entity.EntityCId.ToString());
    }
}

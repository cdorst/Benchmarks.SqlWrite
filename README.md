# SQL (stored-procedure) save-operation benchmarks
Run `.\run.ps1` or `./run.sh` at the repository root to repeat the experiment

## Question

How do the `Dapper` and `System.Data.SqlClient` libraries perform with a simple, stored-procedure, write-operation with HTTP PUT behavior: (return `Id` of existing entity matching on alternate-key, else save new row and return computed `Id` integer value)?

## Variables

Two implementations of TDS are tested:

- `Dapper`
- `System.Data.SqlClient`

Performance impact is observed across these dimensions:

- Use of `CommandType.Text`
- Use of `CommandType.StoredProcedure` with parameters
- Use of `CommandDefinition` structure (`Dapper` only)

## Hypothesis

Given that `Dapper` is a micro-ORM layer extending the `System.Data.SqlClient`, `SqlClient` is expected to outperform `Dapper` when executing scalar-valued-return queries. `CommandType.Text` is expected to outperform the parameterized `CommandType.StoredProcedure` implementations given that parameter objects are not used.

## Results

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-HUKUQV : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT

LaunchCount=10  

```
|                            Method |     Mean |     Error |    StdDev |   Median | Rank | Allocated |
|---------------------------------- |---------:|----------:|----------:|---------:|-----:|----------:|
| DapperDotNet_NewCommandDefinition | 4.255 ms | 0.0307 ms | 0.2555 ms | 4.255 ms |    1 |   4.98 KB |
|          DapperDotNet_TextCommand | 5.743 ms | 0.2644 ms | 2.4157 ms | 4.388 ms |    3 |   3.54 KB |
|           DapperDotNet_Parameters | 5.506 ms | 0.2633 ms | 2.4375 ms | 4.211 ms |    2 |   4.98 KB |
|      AdoSqlCommand_WithParameters | 5.761 ms | 0.2600 ms | 2.3990 ms | 4.329 ms |    3 |   4.04 KB |
|                AdoSqlCommand_Text | 5.436 ms | 0.2433 ms | 2.2011 ms | 4.356 ms |    2 |   3.34 KB |

### Iterations

|                            Method | Sample Size |
|---------------------------------- | -----------:|
| DapperDotNet_NewCommandDefinition |         756 |
|          DapperDotNet_TextCommand |         910 |
|           DapperDotNet_Parameters |         934 |
|      AdoSqlCommand_WithParameters |         928 |
|                AdoSqlCommand_Text |         892 |

## Conclusion

Using `CommandType.TextCommand` yielded a ~17% memory-allocation savings over stored-procedure implementations.

Using `Dapper.CommandDefinition` yielded a ~26% CPU-time savings over similar `System.Data.SqlClient` implementation; a ~22% CPU-time savings compared to using the non-`CommandDefinition` `ExecuteScalarAsync<int>` overload.

## Future Work

Implement a `TdsQuery` abstraction using `System.Net.Sockets` and `ReadOnlySpan<byte>` that allocates a minimal amount of memory.

Explore code-generating "Curried" `TDS`-client implementations that share pre-calculated ReadOnlyMemory<byte> arrays for each TDS-transaction message part (re-using all non-value query bytes across many transactions - e.g. for a SQL-write-only atomic microservice that allocates memory exactly as-needed)


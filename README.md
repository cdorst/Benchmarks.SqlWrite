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
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-YGYCBR : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT

LaunchCount=20  

```
|                                                        Method |       Mean |       Error |       StdDev |     Median | Rank |  Gen 0 | Allocated |
|-------------------------------------------------------------- |-----------:|------------:|-------------:|-----------:|-----:|-------:|----------:|
|                             DapperDotNet_NewCommandDefinition | 7,404.3 us |  88.0941 us | 1,190.705 us | 7,368.9 us |    4 |      - |   5.04 KB |
|                     DapperDotNet_NewCommandDefinition_AsBytes | 8,212.6 us |  70.2271 us |   935.677 us | 8,161.1 us |    9 |      - |   5.04 KB |
|                        DapperDotNet_NewCommandDefinition_Text | 8,295.0 us |  68.1457 us |   916.409 us | 8,252.8 us |    9 |      - |   3.47 KB |
|                DapperDotNet_NewCommandDefinition_Text_AsBytes | 8,113.4 us |  72.9239 us |   979.161 us | 8,109.3 us |    8 |      - |   3.47 KB |
|         DapperDotNet_NewCommandDefinition_Text_NativeCompiled |   537.7 us |   0.7513 us |     3.725 us |   537.3 us |    2 | 0.9766 |   3.47 KB |
| DapperDotNet_NewCommandDefinition_Text_AsBytes_NativeCompiled |   541.9 us |   0.8310 us |     4.212 us |   541.3 us |    3 | 0.9766 |   3.47 KB |
|                                      DapperDotNet_TextCommand | 7,395.4 us |  84.6554 us | 1,146.827 us | 7,430.5 us |    4 |      - |   3.47 KB |
|                              DapperDotNet_TextCommand_AsBytes | 8,256.3 us |  71.2147 us |   961.584 us | 8,232.4 us |    9 |      - |   3.47 KB |
|                                       DapperDotNet_Parameters | 8,022.8 us |  58.4209 us |   788.035 us | 7,995.6 us |    7 |      - |   5.04 KB |
|                               DapperDotNet_Parameters_AsBytes | 7,777.9 us |  79.6400 us | 1,074.803 us | 7,793.6 us |    6 |      - |   5.04 KB |
|                                  AdoSqlCommand_WithParameters | 7,526.4 us |  73.3206 us |   983.478 us | 7,532.6 us |    5 |      - |   4.03 KB |
|                          AdoSqlCommand_WithParameters_AsBytes | 7,989.3 us |  62.7117 us |   843.764 us | 7,950.9 us |    7 |      - |   4.03 KB |
|                                            AdoSqlCommand_Text | 8,231.1 us | 107.2667 us | 1,453.874 us | 8,109.7 us |    9 |      - |   3.27 KB |
|                                    AdoSqlCommand_Text_AsBytes | 8,200.4 us |  81.6946 us | 1,106.438 us | 8,239.0 us |    9 |      - |   3.27 KB |
|                             AdoSqlCommand_Text_NativeCompiled |   534.7 us |   0.7943 us |     4.033 us |   534.7 us |    1 | 0.9766 |   3.27 KB |
|                     AdoSqlCommand_Text_AsBytes_NativeCompiled |   535.5 us |   1.0416 us |     5.345 us |   534.7 us |    1 | 0.9766 |   3.27 KB |

### Iterations

|                                                        Method | Sample Size |
|-------------------------------------------------------------- | -----------:|
|                             DapperDotNet_NewCommandDefinition |        1984 |
|                     DapperDotNet_NewCommandDefinition_AsBytes |        1928 |
|                        DapperDotNet_NewCommandDefinition_Text |        1964 |
|                DapperDotNet_NewCommandDefinition_Text_AsBytes |        1958 |
|         DapperDotNet_NewCommandDefinition_Text_NativeCompiled |         272 |
| DapperDotNet_NewCommandDefinition_Text_AsBytes_NativeCompiled |         284 |
|                                      DapperDotNet_TextCommand |        1993 |
|                              DapperDotNet_TextCommand_AsBytes |        1980 |
|                                       DapperDotNet_Parameters |        1976 |
|                               DapperDotNet_Parameters_AsBytes |        1978 |
|                                  AdoSqlCommand_WithParameters |        1954 |
|                          AdoSqlCommand_WithParameters_AsBytes |        1966 |
|                                            AdoSqlCommand_Text |        1995 |
|                                    AdoSqlCommand_Text_AsBytes |        1992 |
|                             AdoSqlCommand_Text_NativeCompiled |         285 |
|                     AdoSqlCommand_Text_AsBytes_NativeCompiled |         290 |

## Conclusion

Using `CommandType.TextCommand` yielded a ~17% memory-allocation savings over parameterized stored-procedure implementations.

Using memory-optimized entities with natively-compiled stored procedures yielded a ~15x time savings over non-memory-optimized entities & stored procedures.

Of scenarios tested, the solution with minimal space & time overhead is `AdoSqlCommand_Text_NativeCompiled` - a `TextCommand` that executes a natively-compiled stored procedure (operating on memory-optimzed tables) using the `System.Data.SqlClient` "ADO.NET" `SqlConnection` & `SqlCommand` types. 

## Future Work

Implement a `TdsQuery` abstraction using `System.Net.Sockets` and `ReadOnlySpan<byte>` that allocates a minimal amount of memory.

Explore code-generating "Curried" `TDS`-client implementations that share pre-calculated `ReadOnlyMemory<byte>` arrays for each TDS-transaction message part (re-using all non-value query bytes across many transactions - e.g. for a SQL-write-only atomic microservice that allocates memory exactly as-needed)


``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  Job-WWENVU : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT

LaunchCount=20  

```
|                                    Method |     Mean |     Error |    StdDev |   Median | Rank | Allocated |
|------------------------------------------ |---------:|----------:|----------:|---------:|-----:|----------:|
|         DapperDotNet_NewCommandDefinition | 6.250 ms | 0.0659 ms | 0.8763 ms | 6.142 ms |    1 |   4.98 KB |
| DapperDotNet_NewCommandDefinition_AsBytes | 7.077 ms | 0.0473 ms | 0.6333 ms | 7.032 ms |    3 |   4.98 KB |
|                  DapperDotNet_TextCommand | 7.229 ms | 0.0410 ms | 0.5470 ms | 7.201 ms |    5 |   3.47 KB |
|          DapperDotNet_TextCommand_AsBytes | 7.177 ms | 0.0456 ms | 0.6113 ms | 7.154 ms |    4 |   3.47 KB |
|                   DapperDotNet_Parameters | 7.072 ms | 0.0497 ms | 0.6592 ms | 7.027 ms |    3 |   4.98 KB |
|           DapperDotNet_Parameters_AsBytes | 6.994 ms | 0.0421 ms | 0.5614 ms | 6.964 ms |    2 |   4.98 KB |
|              AdoSqlCommand_WithParameters | 6.986 ms | 0.0428 ms | 0.5702 ms | 6.951 ms |    2 |   4.03 KB |
|      AdoSqlCommand_WithParameters_AsBytes | 6.973 ms | 0.0394 ms | 0.5286 ms | 6.950 ms |    2 |   4.03 KB |
|                        AdoSqlCommand_Text | 7.436 ms | 0.0506 ms | 0.6792 ms | 7.420 ms |    6 |   3.27 KB |
|                AdoSqlCommand_Text_AsBytes | 7.224 ms | 0.0455 ms | 0.6077 ms | 7.188 ms |    5 |   3.27 KB |

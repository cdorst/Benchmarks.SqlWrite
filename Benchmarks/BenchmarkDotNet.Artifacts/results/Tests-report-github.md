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

# CoreSharp

[![Nuget](https://img.shields.io/nuget/v/CoreSharp.ResponseCaching)](https://www.nuget.org/packages/CoreSharp.ResponseCaching/)
[![Nuget](https://img.shields.io/nuget/dt/CoreSharp.ResponseCaching)](https://www.nuget.org/packages/CoreSharp.ResponseCaching/)

## Benchmarks
- Open Package Manager Console.
- Run `dotnet run --project .\src\Benchmarks -c Release`.

| Method                            | Mean          | Error      | StdDev     | Median        | Gen0   | Allocated |
|-----------------------------------|---------------|------------|------------|---------------|--------|-----------|
| Reflection_GetAttributes          | 1,383.3675 ns | 16.0660 ns | 15.0281 ns | 1,382.9945 ns | 0.0954 | 800 B     |
| CachedReflection_GetAttributes    | 1.2843 ns     | 0.0239 ns  | 0.0223 ns  | 1.2813 ns     | -      | -         |
| Reflection_GetProperties          | 47.2577 ns    | 0.7811 ns  | 0.7306 ns  | 47.3400 ns    | 0.0057 | 48 B      |
| CachedReflection_GetProperties    | 1.2801 ns     | 0.0553 ns  | 0.0757 ns  | 1.2472 ns     | -      | -         |
| Reflection_GetFields              | 64.1093 ns    | 1.2572 ns  | 1.3451 ns  | 64.1704 ns    | 0.0153 | 128 B     |
| CachedReflection_GetFields        | 1.7641 ns     | 0.0667 ns  | 0.1151 ns  | 1.7675 ns     | -      | -         |
| Direct_GetPropertyValue           | 0.0000 ns     | 0.0000 ns  | 0.0000 ns  | 0.0000 ns     | -      | -         |
| Reflection_GetPropertyValue       | 44.1028 ns    | 0.5026 ns  | 0.4456 ns  | 43.9129 ns    | -      | -         |
| CachedReflection_GetPropertyValue | 23.6131 ns    | 0.1533 ns  | 0.1359 ns  | 23.5632 ns    | -      | -         |
| Direct_SetPropertyValue           | 1.9668 ns     | 0.0096 ns  | 0.0089 ns  | 1.9606 ns     | -      | -         |
| Reflection_SetPropertyValue       | 57.6828 ns    | 0.7724 ns  | 0.6450 ns  | 57.5116 ns    | -      | -         |
| CachedReflection_SetPropertyValue | 24.8256 ns    | 0.2052 ns  | 0.1713 ns  | 24.8534 ns    | -      | -         |
| Direct_GetFieldValue              | 0.0084 ns     | 0.0096 ns  | 0.0090 ns  | 0.0028 ns     | -      | -         |
| Reflection_GetFieldValue          | 52.8616 ns    | 0.0615 ns  | 0.0480 ns  | 52.8613 ns    | -      | -         |
| CachedReflection_GetFieldValue    | 50.3885 ns    | 0.6529 ns  | 0.6107 ns  | 49.9727 ns    | -      | -         |
| Direct_SetFieldValue              | 2.0074 ns     | 0.0358 ns  | 0.0335 ns  | 1.9954 ns     | -      | -         |
| Reflection_SetFieldValue          | 56.1454 ns    | 1.0409 ns  | 0.9737 ns  | 56.1375 ns    | -      | -         |
| CachedReflection_SetFieldValue    | 59.6635 ns    | 0.5960 ns  | 0.5575 ns  | 59.7582 ns    | -      | -         |
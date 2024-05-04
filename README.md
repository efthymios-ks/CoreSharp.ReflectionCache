# CoreSharp.ReflectionCache

[![Nuget](https://img.shields.io/nuget/v/CoreSharp.ReflectionCache)](https://www.nuget.org/packages/CoreSharp.ReflectionCache/)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=efthymios-ks_CoreSharp.ReflectionCache&metric=coverage)](https://sonarcloud.io/summary/new_code?id=efthymios-ks_CoreSharp.ReflectionCache)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=efthymios-ks_CoreSharp.ReflectionCache&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=efthymios-ks_CoreSharp.ReflectionCache)
![GitHub License](https://img.shields.io/github/license/efthymios-ks/CoreSharp.ReflectionCache)

> Caching utilities for reflection.

## Features
- Implementations for `UnitOfWork` and `Repository` pattern.
- Implementations for `Store` pattern.
- Track and store `DbContext` changes.

## Installation
Install the package with [Nuget](https://www.nuget.org/packages/CoreSharp.ReflectionCache/).  
```
dotnet add package CoreSharp.ReflectionCache
```

## Use case
```CSharp
var student = GetStudent();
var cachedType = ReflectionCache.GetCachedType<Student>();

var grade = cachedType
    .Properties[nameof(Student.Grade)]
    .GetValue<Student, double>(student);

grade *= 1.5;
cachedType
    .Properties[nameof(Student.Grade)]
    .SetValue<Student, double>(student, grade);

var studentDisplayName = cachedType
    .Attributes
    .OfType<DisplayAttribute>()?.Name;
```

## Benchmarks
- Open Package Manager Console.
- Run `dotnet run --project .\src\Benchmarks -c Release`.

| Method                            | Mean        | Error      | StdDev     | Median      | Completed Work Items | Lock Contentions | Gen0   | Allocated native memory | Native memory leak | Allocated |
|---------------------------------- |------------:|-----------:|-----------:|------------:|---------------------:|-----------------:|-------:|------------------------:|-------------------:|----------:|
| GetConstructors_Reflection        |  23.3377 ns |  0.1950 ns |  0.1824 ns |  23.4375 ns |                    - |                - | 0.0038 |                       - |                  - |      32 B |
| GetConstructors_Cached            |   1.1634 ns |  0.0076 ns |  0.0071 ns |   1.1596 ns |                    - |                - |      - |                       - |                  - |         - |
| GetAttributes_Reflection          | 741.7643 ns | 14.0973 ns | 15.0840 ns | 736.0519 ns |                    - |                - | 0.0610 |                       - |                  - |     512 B |
| GetAttributes_Cached              |   1.2740 ns |  0.0270 ns |  0.0252 ns |   1.2728 ns |                    - |                - |      - |                       - |                  - |         - |
| GetMethods_Reflection             | 130.5076 ns |  0.7894 ns |  0.6163 ns | 130.3080 ns |                    - |                - | 0.0229 |                       - |                  - |     192 B |
| GetMethods_Cached                 |   1.1877 ns |  0.0067 ns |  0.0063 ns |   1.1843 ns |                    - |                - |      - |                       - |                  - |         - |
| GetProperties_Reflection          |  19.0281 ns |  0.3365 ns |  0.3147 ns |  19.0604 ns |                    - |                - | 0.0038 |                       - |                  - |      32 B |
| GetProperties_Cached              |   1.4858 ns |  0.0268 ns |  0.0251 ns |   1.4759 ns |                    - |                - |      - |                       - |                  - |         - |
| GetFields_Reflection              |  19.5024 ns |  0.1480 ns |  0.1236 ns |  19.5148 ns |                    - |                - | 0.0038 |                       - |                  - |      32 B |
| GetFields_Cached                  |   1.2408 ns |  0.0063 ns |  0.0056 ns |   1.2386 ns |                    - |                - |      - |                       - |                  - |         - |
| GetPropertyValue_Directly         |   0.0076 ns |  0.0093 ns |  0.0087 ns |   0.0054 ns |                    - |                - |      - |                       - |                  - |         - |
| GetPropertyValue_Reflection       |  32.3559 ns |  0.5482 ns |  0.4859 ns |  32.1239 ns |                    - |                - |      - |                       - |                  - |         - |
| GetPropertyValue_Cached           |  18.9917 ns |  0.1447 ns |  0.1353 ns |  19.0575 ns |                    - |                - |      - |                       - |                  - |         - |
| SetPropertyValue_Directly         |   2.2014 ns |  0.0122 ns |  0.0114 ns |   2.2092 ns |                    - |                - |      - |                       - |                  - |         - |
| SetPropertyValue_Reflection       |  58.8107 ns |  0.1257 ns |  0.1050 ns |  58.8707 ns |                    - |                - |      - |                       - |                  - |         - |
| SetPropertyValue_Cached           |  20.2941 ns |  0.2563 ns |  0.2272 ns |  20.3702 ns |                    - |                - |      - |                       - |                  - |         - |
| GetFieldValue_Directly            |   0.0025 ns |  0.0015 ns |  0.0012 ns |   0.0022 ns |                    - |                - |      - |                       - |                  - |         - |
| GetFieldValue_Reflection          |  46.8045 ns |  0.1932 ns |  0.1613 ns |  46.7894 ns |                    - |                - |      - |                       - |                  - |         - |
| GetFieldValue_Cached              |  52.8490 ns |  0.1833 ns |  0.1714 ns |  52.8802 ns |                    - |                - |      - |                       - |                  - |         - |
| SetFieldValue_Directly            |   2.1973 ns |  0.0055 ns |  0.0046 ns |   2.1983 ns |                    - |                - |      - |                       - |                  - |         - |
| SetFieldValue_Reflection          |  52.7084 ns |  0.3944 ns |  0.3293 ns |  52.7028 ns |                    - |                - |      - |                       - |                  - |         - |
| SetFieldValue_Cached              |  60.2460 ns |  1.1150 ns |  2.7140 ns |  59.4208 ns |                    - |                - |      - |                       - |                  - |         - |
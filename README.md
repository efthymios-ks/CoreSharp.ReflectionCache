# CoreSharp.ReflectionCache

[![Nuget](https://img.shields.io/nuget/v/CoreSharp.ReflectionCache)](https://www.nuget.org/packages/CoreSharp.ReflectionCache/)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=efthymios-ks_CoreSharp.ReflectionCache&metric=coverage)](https://sonarcloud.io/summary/new_code?id=efthymios-ks_CoreSharp.ReflectionCache)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=efthymios-ks_CoreSharp.ReflectionCache&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=efthymios-ks_CoreSharp.ReflectionCache)
![GitHub License](https://img.shields.io/github/license/efthymios-ks/CoreSharp.ReflectionCache)

> Caching utilities for reflection.

## Features
Cachable members
- Types
- Constructors
- Attributes
- Properties
- Fields
- Methods

## Installation
Install the package with [Nuget](https://www.nuget.org/packages/CoreSharp.ReflectionCache/).  
```
dotnet add package CoreSharp.ReflectionCache
```

## Examples
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
```dotnet run --project .\src\Benchmarks -c Release```

| Method     | Categories         | Mean       | Ratio           | Allocated |
|------------|--------------------|------------|-----------------|-----------|
| Reflection | Get constructors   | 23.054 ns  | baseline        | 32 B      |
| Cached     | Get constructors   | 1.288 ns   | 17.93x faster   | -         |
|            |                    |            |                 |           |
| Reflection | Get attributes     | 730.227 ns | baseline        | 512 B     |
| Cached     | Get attributes     | 1.495 ns   | 489.524x faster | -         |
|            |                    |            |                 |           |
| Reflection | Get methods        | 128.543 ns | baseline        | 192 B     |
| Cached     | Get methods        | 1.283 ns   | 100.295x faster | -         |
|            |                    |            |                 |           |
| Reflection | Get properties     | 20.764 ns  | baseline        | 32 B      |
| Cached     | Get properties     | 1.507 ns   | 13.69x faster   | -         |
|            |                    |            |                 |           |
| Reflection | Get fields         | 20.814 ns  | baseline        | 32 B      |
| Cached     | Get fields         | 1.203 ns   | 17.28x faster   | -         |
|            |                    |            |                 |           |
| Reflection | Get property value | 35.200 ns  | baseline        | -         |
| Cached     | Get property value | 19.920 ns  | 1.77x faster    | -         |
|            |                    |            |                 |           |
| Reflection | Set property value | 64.437 ns  | baseline        | -         |
| Cached     | Set property value | 21.387 ns  | 3.01x faster    | -         |
|            |                    |            |                 |           |
| Reflection | Get field value    | 49.821 ns  | baseline        | -         |
| Cached     | Get field value    | 52.631 ns  | 1.06x slower    | -         |
|            |                    |            |                 |           |
| Reflection | Set field value    | 55.967 ns  | baseline        | -         |
| Cached     | Set field value    | 57.180 ns  | 1.02x slower    | -         |
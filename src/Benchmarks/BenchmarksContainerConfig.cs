using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Loggers;

namespace Benchmarks;

public sealed class BenchmarksContainerConfig : ManualConfig
{
    // Constructors 
    public BenchmarksContainerConfig()
    {
        AddDiagnoser(Diagnosers);
        AddLogger(Loggers);
    }

    // Properties
    private static IDiagnoser[] Diagnosers
        => new IDiagnoser[]
        {
            MemoryDiagnoser.Default,
            new EtwProfiler(),
            new ConcurrencyVisualizerProfiler(),
            new NativeMemoryProfiler(),
            ThreadingDiagnoser.Default
        };

    private static ILogger[] Loggers
        => new ILogger[]
        {
            new ConsoleLogger()
        };
}
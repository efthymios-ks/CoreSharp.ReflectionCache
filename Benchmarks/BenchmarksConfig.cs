using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using SummaryStyle = BenchmarkDotNet.Reports.SummaryStyle;

namespace Benchmarks;

public sealed class BenchmarksConfig : ManualConfig
{
    // Constructors 
    public BenchmarksConfig()
    {
        _ = WithSummaryStyle(SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend));

        _ = AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByCategory);
        _ = AddColumn(CategoriesColumn.Default);

        _ = AddDiagnoser(MemoryDiagnoser.Default);
        _ = HideColumns(Column.Error, Column.StdDev, Column.RatioSD, Column.Gen0, Column.AllocRatio);
    }
}
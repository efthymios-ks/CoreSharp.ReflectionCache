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
        WithSummaryStyle(SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend));

        AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByCategory);
        AddColumn(CategoriesColumn.Default);

        AddDiagnoser(MemoryDiagnoser.Default);
        HideColumns(Column.Error, Column.StdDev, Column.RatioSD, Column.Gen0, Column.AllocRatio);
    }
}
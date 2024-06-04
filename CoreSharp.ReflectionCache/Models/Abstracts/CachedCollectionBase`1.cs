using System.Collections;
using System.Diagnostics;

namespace CoreSharp.ReflectionCache.Models.Abstracts;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class CachedCollectionBase<TElement>(IEnumerable<TElement>? source)
    : IReadOnlyCollection<TElement>
{

    // Properties 
    private string DebuggerDisplay
        => $"Count={Count}";

    protected TElement[] Source { get; } = source?.ToArray() ?? [];

    public int Count
        => Source.Length;

    // Methods 
    public IEnumerator<TElement> GetEnumerator()
        => ((IEnumerable<TElement>)Source).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreSharp.ReflectionCache.Models.Abstracts;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class CachedCollectionBase<TElement> : IReadOnlyCollection<TElement>
{
    // Constructors 
    protected CachedCollectionBase(IEnumerable<TElement> source)
        => Source = source?.ToArray() ?? Array.Empty<TElement>();

    // Properties 
    private string DebuggerDisplay
        => $"Count={Count}";

    protected TElement[] Source { get; }

    public int Count
        => Source.Length;

    // Methods 
    public IEnumerator<TElement> GetEnumerator()
        => ((IEnumerable<TElement>)Source).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}


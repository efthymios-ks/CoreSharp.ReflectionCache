using System.Collections;
using System.Diagnostics;

namespace CoreSharp.ReflectionCache.Models.Abstracts;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class CachedDictionaryBase<TElement>(IReadOnlyDictionary<string, TElement>? source)
    : IReadOnlyDictionary<string, TElement>
{

    // Properties 
    private string DebuggerDisplay
        => $"Count={Count}";

    protected IReadOnlyDictionary<string, TElement> Source { get; } = source ?? new Dictionary<string, TElement>();

    public TElement this[string key]
        => Source[key];

    public IEnumerable<string> Keys
        => Source.Keys;

    public IEnumerable<TElement> Values
        => Source.Values;

    public int Count
        => Source.Count;

    // Methods 
    public bool ContainsKey(string key)
        => Source.ContainsKey(key);

    public bool TryGetValue(string key, out TElement value)
        => Source.TryGetValue(key, out value!);

    public IEnumerator<KeyValuePair<string, TElement>> GetEnumerator()
        => Source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
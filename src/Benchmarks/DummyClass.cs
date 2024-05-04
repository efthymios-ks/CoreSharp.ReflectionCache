using System.ComponentModel.DataAnnotations;

namespace Benchmarks;

public partial class BenchmarksContainer
{
    [Display(Name = "DummyClass")]
    internal sealed class DummyClass
    {
        // Fields 
        public string Field = "Field";

        // Constructors
        public DummyClass()
        {
        }

        // Properties
        public string Property { get; set; } = "Property";

        // Methods 
        public string Process(int value)
            => value.ToString();
    }
}
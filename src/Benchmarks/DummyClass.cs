using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Benchmarks;

public partial class BenchmarksContainer
{
    // Add random fields, properties and attributes for testing.
    [DebuggerDisplay("DummyClass")]
    [Display(Name = "DummyClass")]
    internal sealed class DummyClass
    {
        // Fields 
        public const string ConstField1 = "ConstField1";
        public readonly string ReadOnlyField1 = "ReadOnlyField1";

        [Display(Name = "Field1_Display")]
        public string FieldWith1Attribute = "Field1";

        [Display(Name = "Field2_Display")]
        [Description("Field2_Description")]
        public string FieldWith2Attributes = "Field2";

        // Properties
        public string ReadOnlyProperty { get; } = "ReadOnlyProperty1";

        [Display(Name = "Property1_Display")]
        public string PropertyWith1Attribute { get; set; } = "Property1";

        [Display(Name = "Property2_Display")]
        [Description("Property2_Description")]
        public string PropertyWith2Attributes { get; set; } = "Property2";
    }
}
using System;

namespace Tests.Internal;

[AttributeUsage(AttributeTargets.All)]
internal sealed class DummyNonExistentAttribute : Attribute
{
}
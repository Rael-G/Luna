using System.Runtime.Serialization;

namespace Luna;

[DataContract]
[AttributeUsage(AttributeTargets.Class)]
public sealed class SerializeAttribute : Attribute
{

}


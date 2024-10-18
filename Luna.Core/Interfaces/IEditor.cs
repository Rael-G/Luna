using System;

namespace Luna;

public interface IEditor
{
    T ReadRootFile<T>() where T : Node;
}

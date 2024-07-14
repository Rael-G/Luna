using Luna.Core;

namespace Luna.Engine.OpenGl;

internal class RenderObjectFactory : IRenderObjectFactory
{
    public IRenderObject CreateRenderObject<TData>(TData data)
    {
        if (data is RectangleData rectangleData)
            return new RectangleObject(rectangleData);
        
        throw new LunaException("CreateRenderObject from type " +
            $"{nameof(TData)} is not  possible or it was not yet implemented.");
    }
}

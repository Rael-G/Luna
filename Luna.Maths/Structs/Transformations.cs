using System.Numerics;

namespace Luna.Maths;

public readonly struct Transformations
{
    public static Matrix TranslationMatrix(Vector3 point)
        => Matrix4x4.CreateTranslation(point);

    public static Matrix ScaleMatrix(Vector3 scale)
        => Matrix4x4.CreateScale(scale);

    public static Matrix RotationMatrix(float angle, Vector3 axis)
        => Matrix4x4.CreateFromAxisAngle(axis.Normalize(), angle);
    
    public static Matrix LookAtMatrix(Vector3 eye, Vector3 center, Vector3 up)
        => Matrix4x4.CreateLookAt(eye, center, up);
    
    public static Matrix OrthographicProjection(float left, float right, float bottom, float top, float near, float far)
        => Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, near, far);

    public static Matrix PerspectiveProjection(float fov, float aspect, float near, float far)
        => Matrix4x4.CreatePerspectiveFieldOfView(fov.ToRadians(), aspect, near, far);
    
    public static Matrix FlipMatrix(Vector3 axis)
    {
        var x = axis.X == 0.0f? 1.0f : -axis.X;
        var y = axis.Y == 0.0f? 1.0f : -axis.Y;
        var z = axis.Z == 0.0f? 1.0f : -axis.Z;
        return new (
            4, 4, 
        [
            x ,     0.0f,   0.0f, 0.0f,
            0.0f,   y ,     0.0f, 0.0f,
            0.0f,   0.0f,   z,    0.0f,
            0.0f,   0.0f,   0.0f, 1.0f
        ]);
    }

    public static Matrix ShearMatrix(Vector3 shearFactor, Vector3 axis)
    {
        axis = axis.Normalize();
        return new(
            4, 4,
            [
                1,                      shearFactor.Y * axis.X, shearFactor.Z * axis.X, 0,
                shearFactor.X * axis.Y, 1,                      shearFactor.Z * axis.Y, 0,
                shearFactor.X * axis.Z, shearFactor.Y * axis.Z, 1,                      0,
                0,                      0,                      0,                      1
            ]);        
    }

}

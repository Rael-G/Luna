using System.Numerics;

namespace Luna.Maths;

public readonly struct Transformations
{
    public static Matrix TranslationMatrix(Vector3 point)
        => new (
            4, 4,
        [
            1, 0, 0, point.X,
            0, 1, 0, point.Y,
            0, 0, 1, point.Z,
            0, 0, 0, 1
        ]);

    public static Matrix RotationMatrix(float angle, Vector3 axis)
    {
        axis = axis.Normalize();

        var cosTheta = (float)Math.Cos(angle);
        var oneMinusCosTheta = 1 - cosTheta;
        var sinTheta = (float)Math.Sin(angle);

        return new (
            4, 4,
        [

                cosTheta + axis.X * axis.X * oneMinusCosTheta,
                axis.X * axis.Y * oneMinusCosTheta - axis.Z * sinTheta,
                axis.X * axis.Z * oneMinusCosTheta + axis.Y * sinTheta,
                0
            ,
            
                axis.Y * axis.X * oneMinusCosTheta + axis.Z * sinTheta,
                cosTheta + axis.Y * axis.Y * oneMinusCosTheta,
                axis.Y * axis.X * oneMinusCosTheta - axis.Z * sinTheta,
                0
            ,
            
                axis.Z * axis.X * oneMinusCosTheta - axis.Y * sinTheta,
                axis.Z * axis.Y * oneMinusCosTheta + axis.X * sinTheta,
                cosTheta + axis.Z * axis.Z * oneMinusCosTheta,
                0
            ,
            
                0.0f, 
                0.0f, 
                0.0f, 
                1.0f
        ]);
    }

    public static Matrix ScaleMatrix(Vector3 scale)
    {
        var scaleMatrix = new Matrix(4, 1, [scale.X, scale.Y, scale.Z, 1.0f]);
        return scaleMatrix.Diagonal();
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

    public static Matrix LookAtMatrix(Vector3 eye, Vector3 center, Vector3 up)
    {
        var viewDirection = (center - eye).Normalize();
        var rightVector = viewDirection.Cross(up).Normalize();
        var trueUp = rightVector.Cross(viewDirection);

        viewDirection = -viewDirection;

        return new (
            4, 4,
        [
            rightVector.X,      rightVector.Y,      rightVector.Z,      -rightVector.Dot(eye),
            trueUp.X,           trueUp.Y,           trueUp.Z,           -trueUp.Dot(eye),
            viewDirection.X,    viewDirection.Y,    viewDirection.Z,    viewDirection.Dot(eye),
            0,                  0,                  0,                  1
        ]);
    }

    public static Matrix OrthographicProjection(float left, float right, float bottom, float top, float near, float far)
        => new (
            4, 4,
        [
            2 / (right - left), 0,                  0,                  -(right + left) / (right - left),
            0,                  2 / (top - bottom), 0,                  -(top + bottom) / (top - bottom),
            0,                  0,                  -2 / (far - near),  -(far + near) / (far -near) ,
            0,                  0,                  0,                  1
        ]);

    public static Matrix PerspectiveProjection(float fovy, float aspect, float near, float far)
    {
        var f = 1.0f / (float)Math.Tan(fovy.ToRadians() / 2);
        return new (
            4, 4,
        [
            f / aspect, 0, 0,                           0,
            0,          f, 0,                           0,
            0,          0, (far + near) / (near - far), 2 * far * near / (near - far),
            0,          0, -1,                          0
        ]);
    }

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

}

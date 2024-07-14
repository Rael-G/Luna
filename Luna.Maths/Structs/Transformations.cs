namespace Luna.Maths;

public readonly struct Transformations
{
    public static Matrix TranslationMatrix(Vector3D point)
        => new (new double[,]
        {
            { 1, 0, 0, point.X },
            { 0, 1, 0, point.Y },
            { 0, 0, 1, point.Z },
            { 0, 0, 0, 1 }
        });

    public static Matrix RotationMatrix(double angle, Vector3D axis)
    {
        axis = axis.Normalize();

        var cosTheta = Math.Cos(angle);
        var oneMinusCosTheta = 1 - cosTheta;
        var sinTheta = Math.Sin(angle);

        return new (new double[,]
        {
            {
                cosTheta + axis.X * axis.X * oneMinusCosTheta,
                axis.X * axis.Y * oneMinusCosTheta - axis.Z * sinTheta,
                axis.X * axis.Z * oneMinusCosTheta + axis.Y * sinTheta,
                0
            },
            {
                axis.Y * axis.X * oneMinusCosTheta + axis.Z * sinTheta,
                cosTheta + axis.Y * axis.Y * oneMinusCosTheta,
                axis.Y * axis.X * oneMinusCosTheta - axis.Z * sinTheta,
                0
            },
            {
                axis.Z * axis.X * oneMinusCosTheta - axis.Y * sinTheta,
                axis.Z * axis.Y * oneMinusCosTheta + axis.X * sinTheta,
                cosTheta + axis.Z * axis.Z * oneMinusCosTheta,
                0
            },
            {
                0.0, 0.0, 0.0, 1.0
            }

        });
    }

    public static Matrix ScaleMatrix(Vector3D scale)
    {
        var scaleMatrix = new Matrix(new double[,]{{scale.X}, {scale.Y}, {scale.Z}, {1.0}});
        return scaleMatrix.Diagonal();
    }

    public static Matrix ShearMatrix(Vector3D shearFactor, Vector3D axis)
    {
        axis = axis.Normalize();
        return new(new double[,]
            {
                { 1, shearFactor.Y * axis.X, shearFactor.Z * axis.X, 0 },
                { shearFactor.X * axis.Y, 1, shearFactor.Z * axis.Y, 0 },
                { shearFactor.X * axis.Z, shearFactor.Y * axis.Z, 1, 0 },
                { 0, 0, 0, 1 }
            });
    }

    public static Matrix LookAtMatrix(Vector3D eye, Vector3D center, Vector3D up)
    {
        var viewDirection = (center - eye).Normalize();
        var rightVector = viewDirection.Cross(up).Normalize();
        var trueUp = rightVector.Cross(viewDirection);

        viewDirection = -viewDirection;

        return new Matrix(new double[,]
        {
            { rightVector.X, rightVector.Y, rightVector.Z, -rightVector.Dot(eye) },
            { trueUp.X, trueUp.Y, trueUp.Z, -trueUp.Dot(eye) },
            { viewDirection.X, viewDirection.Y, viewDirection.Z, viewDirection.Dot(eye) },
            { 0, 0, 0, 1}
        });
    }

    public static Matrix OrthographicProjection(double left = -1.0, double right = 1.0, double bottom = -1.0, double top = 1.0, double near = 1.0, double far = 10.0)
        => new (new double[,]
        {
            { 2 / (right - left), 0, 0, -(right + left) / (right - left) },
            { 0, 2 / (top - bottom), 0, -(top + bottom) / (top - bottom) },
            { 0, 0, -2 / (far - near), -(far + near) / (far -near) },
            { 0, 0, 0, 1 }
        });

    public static Matrix PerspectiveProjection(double fovy = 45.0, double aspect = 16.0 / 9.0, double near = 1.0, double far = 10.0)
    {
        var f = 1.0 / Math.Tan(fovy.ToRadians() / 2);
        return new Matrix(new double[,]
        {
            { f / aspect, 0, 0, 0 },
            { 0, f, 0, 0 },
            { 0, 0, (far + near) / (near - far), 2 * far * near / (near - far) },
            { 0, 0, -1, 0 }
        });
    }

}

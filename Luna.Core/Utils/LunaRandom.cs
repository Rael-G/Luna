namespace Luna;

public static class LunaRandom
{
    /// <summary>
    /// Returns a non-negative random integer.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer that is greater than or equal to 0 and less than Int32.Max.
    /// </returns>
    public static int Int()
        => new Random(Seed).Next();

    /// <summary>
    /// Returns a random integer that is within a specified range.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number returned.</param>
    /// <param name="max">The exclusive upper bound of the random number returned. max must be greater than or equal to min.</param>
    /// <returns>
    /// A 32-bit signed integer greater than or equal to min and less than max; that is, the range of return values includes min but not max. If min equals max, min is returned.
    /// </returns>
    public static int Int(int min, int max)
        => new Random(Seed).Next(min, max);

    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
    /// </summary>
    /// <returns>
    /// A single-precision floating point number that is greater than or equal to 0.0, and less than 1.0.
    /// </returns>
    public static float Float()
        => new Random(Seed).NextSingle();

    /// <summary>
    /// Returns a random float that is within a specified range.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number returned.</param>
    /// <param name="max">The exclusive upper bound of the random number returned. max must be greater than or equal to min.</param>
    /// <returns>
    /// A float greater than or equal to min and less than max; that is, the range of return values includes min but not max. If min equals max, min is returned.
    /// </returns>
    public static float Float(float min, float max)
        => min + Float() * (max - min);
    
    private static int Seed => (int)(Time.ElapsedTime * 1000);
}

namespace Luna.Core;

public enum DistanceModel
{
    //
    // Summary:
    //     Bypasses all distance attenuation calculation for all Sources.
    None = 0,
    //
    // Summary:
    //     InverseDistance is equivalent to the IASIG I3DL2 model with the exception that
    //     SourceFloat.ReferenceDistance does not imply any clamping.
    InverseDistance = 53249,
    //
    // Summary:
    //     InverseDistanceClamped is the IASIG I3DL2 model, with SourceFloat.ReferenceDistance
    //     indicating both the reference distance and the distance below which gain will
    //     be clamped.
    InverseDistanceClamped = 53250,
    //
    // Summary:
    //     AL_EXT_LINEAR_DISTANCE extension.
    LinearDistance = 53251,
    //
    // Summary:
    //     AL_EXT_LINEAR_DISTANCE extension.
    LinearDistanceClamped = 53252,
    //
    // Summary:
    //     AL_EXT_EXPONENT_DISTANCE extension.
    ExponentDistance = 53253,
    //
    // Summary:
    //     AL_EXT_EXPONENT_DISTANCE extension.
    ExponentDistanceClamped = 53254
}

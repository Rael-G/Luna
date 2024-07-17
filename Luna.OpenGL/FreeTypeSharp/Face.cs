using FreeTypeSharp;

using static FreeTypeSharp.FT;

namespace Luna.OpenGl.FreeTypeSharp;

public unsafe class Face
{
    public Glyph Glyph { get; }

    private readonly FT_FaceRec_* FacePtr;

    public Face(Library library, string fontPath)
    {
        FacePtr = library.CreateFacePtr(fontPath);
        Glyph = new(FacePtr->glyph);
    }

    internal Face(FT_FaceRec_* facePtr)
    {
        FacePtr = facePtr;
        Glyph = new(FacePtr->glyph);
    }

    public void SetPixelSizes(uint width, uint height)
    {
        var error = FT_Set_Pixel_Sizes(FacePtr, width, height);
        if (error != FT_Error.FT_Err_Ok)
            throw new FreeTypeException(error);

    }

    public void LoadChar(char c, LoadFlags flag)
    {
        var error = FT_Load_Char(FacePtr, c, (FT_LOAD)flag);
        if (error != FT_Error.FT_Err_Ok)
                throw new FreeTypeException(error);
    }

    ~Face()
    {
        FT_Done_Face(FacePtr);
    }
}


using System.Runtime.InteropServices;
using FreeTypeSharp;
using static FreeTypeSharp.FT;
// using static FreeTypeSharp.FT_LOAD;
// using static FreeTypeSharp.FT_Render_Mode_;

namespace Luna.OpenGL.FreeTypeSharp;

public unsafe class Library
{
    private readonly FT_LibraryRec_* LibraryPtr;

    public Library()
    {
        FT_LibraryRec_* lib;
        FT_Error error = FT_Init_FreeType(&lib);

        if (error != FT_Error.FT_Err_Ok)
            throw new FreeTypeException(error);

        LibraryPtr = lib;
    }

    public Face CreateFace(string fontPath)
        => new(CreateFacePtr(fontPath));

    internal FT_FaceRec_* CreateFacePtr(string fontPath)
    {
        FT_FaceRec_* facePtr;

        var error = FT_New_Face(LibraryPtr, (byte*)Marshal.StringToHGlobalAnsi(fontPath), 0, &facePtr);
        if (error != FT_Error.FT_Err_Ok)
            throw new FreeTypeException(error);

        return facePtr;
    }

    ~Library()
    {
        FT_Done_FreeType(LibraryPtr);
    }
    
}

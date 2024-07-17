using System.Numerics;
using FreeTypeSharp;

namespace Luna.OpenGl.FreeTypeSharp;

public unsafe class Glyph
{
    //public Glyph Next { get; } 

    public uint GlyphIndex { get => GlyphPtr->glyph_index; }

    public nint LinearHorizontalAdvance { get => GlyphPtr->linearHoriAdvance; }

    public nint LinearVerticalAdvance { get => GlyphPtr->linearVertAdvance; }

    public Vector2 Advance { get => new(GlyphPtr->advance.x, GlyphPtr->advance.y); }

    public FT_Glyph_Format_ Format { get => GlyphPtr->format; }

    public Bitmap Bitmap { get => new(GlyphPtr->bitmap); }

    public int BitmapLeft { get => GlyphPtr->bitmap_left; }

    public int BitmapTop { get => GlyphPtr->bitmap_top; }

    public FT_Outline_ OutLine { get => GlyphPtr->outline; }

    public uint SubglyphsCount { get => GlyphPtr->num_subglyphs; }

    public unsafe FT_SubGlyphRec_* SubGlyphs { get => GlyphPtr->subglyphs; }

    public unsafe void* ControlData { get => GlyphPtr->control_data; }

    public nint ControlLen { get => GlyphPtr->control_len; }

    public nint LsbDelta { get => GlyphPtr->lsb_delta; }

    public nint RsbDelta { get => GlyphPtr->rsb_delta; }

    public unsafe void* Other { get => GlyphPtr->other; }

    public unsafe FT_Slot_InternalRec_* Internal { get => GlyphPtr->_internal; }

    private readonly FT_GlyphSlotRec_* GlyphPtr;

    internal Glyph(FT_GlyphSlotRec_* glyphPtr)
    {
        GlyphPtr = glyphPtr;
        //Next = new Glyph(glyphPtr->next);
    }
}

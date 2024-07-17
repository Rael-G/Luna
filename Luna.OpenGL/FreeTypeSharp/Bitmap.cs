using System.Runtime.InteropServices;
using FreeTypeSharp;

namespace Luna.OpenGl.FreeTypeSharp;

public readonly unsafe struct Bitmap
{
    public readonly uint Rows { get => _bitmap.rows; }

    public uint Width { get => _bitmap.width; }

    public int Pitch { get => _bitmap.pitch; }

    public byte[] Buffer { get => _buffer; }

    public ushort GraysCount { get => _bitmap.num_grays; }

    public FT_Pixel_Mode_ PixelMode { get => _bitmap.pixel_mode; }

    public byte PalleteMode { get => _bitmap.palette_mode; }

    public unsafe void* Palette { get => _bitmap.palette; }

    private readonly FT_Bitmap_ _bitmap;

    private readonly byte[] _buffer;

    internal Bitmap(FT_Bitmap_ bitmap)
    {
        _bitmap = bitmap;
        _buffer = new byte[Width * Rows];
        if (bitmap.buffer is not null)
            Marshal.Copy((IntPtr)bitmap.buffer, _buffer, 0, _buffer.Length);
    }
}

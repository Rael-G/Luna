﻿using System.Numerics;
using Luna.Core;
using Luna.OpenGl;

namespace Luna.OpenGL;

using FontKey = (string Path, Vector2 Size);


public class Utils : IUtils
{
    public Vector2 MeasureTextSize(FontKey font, string text)
    {
        Console.WriteLine(1);
        float width = 0.0f;
        float maxHeight = 0.0f;

        var face = FontManager.GetFont(font);
        FontManager.StartUsing(font);
        
        foreach (var c in text)
        {
            var ch = GlyphManager.GetGlyph(font, c);

            width += ch.Advance >> 6; 
            float height = ch.Size.Y;
            if (height > maxHeight)
            {
                maxHeight = height;
            }
        }
        FontManager.StopUsing(font);

        return new Vector2(width, maxHeight);
    }
}

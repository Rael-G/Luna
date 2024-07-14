﻿using Luna.Maths;

namespace Luna.Core;

public struct RectangleData
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Matrix Transform { get; set; }
    public Color Color { get; set;}
}

using System;

namespace Luna.Core.Events;

public class MousePositionEvent(double x, double y) : InputEvent
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;
}

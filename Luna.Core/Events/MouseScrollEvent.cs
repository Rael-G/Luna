namespace Luna;

public class MouseScrollEvent(double x, double y) : InputEvent
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;
}

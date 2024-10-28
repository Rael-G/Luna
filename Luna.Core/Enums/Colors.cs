namespace Luna;

public readonly struct Colors
{
    public static readonly Color White = new(1f, 1f, 1f);
    public static readonly Color Black = new();
    public static readonly Color Gray = White.Mix(Black);

    public static readonly Color Red = new(1f, 0f, 0f);
    public static readonly Color Green = new(0f, 1f, 0f);
    public static readonly Color Blue = new(0f, 0f, 1f);

    public static readonly Color Cyan = Green.Mix(Blue);
    public static readonly Color Magenta = Red.Mix(Blue);
    public static readonly Color Yellow = Red.Mix(Green);
    
    public static readonly Color Orange = Red.Mix(Yellow);
    public static readonly Color Lime = Green.Mix(Yellow);
    public static readonly Color SpringGreen = Green.Mix(Cyan);
    public static readonly Color Violet = Blue.Mix(Magenta);
    public static readonly Color Azure = Blue.Mix(Cyan);
}
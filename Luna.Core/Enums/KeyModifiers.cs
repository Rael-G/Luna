﻿namespace Luna.Core;

[Flags]
public enum KeyModifiers
{
    //
    // Summary:
    //     if one or more Shift keys were held down.
    Shift = 1,
    //
    // Summary:
    //     If one or more Control keys were held down.
    Control = 2,
    //
    // Summary:
    //     If one or more Alt keys were held down.
    Alt = 4,
    //
    // Summary:
    //     If one or more Super keys were held down.
    Super = 8
}
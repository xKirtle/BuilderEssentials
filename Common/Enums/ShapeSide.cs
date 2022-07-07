using System;

namespace BuilderEssentials.Common.Enums;

[Flags]
public enum ShapeSide
{
    Left = 0x1,
    Top = 0x2,
    Right = 0x4,
    Bottom = 0x8,
    All = Left | Top | Right | Bottom
}
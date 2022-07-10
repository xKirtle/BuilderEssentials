using System;

namespace BuilderEssentials.Common.Enums;

[Flags]
public enum ShapeSide
{
    Top = 0x1,
    Right = 0x2,
    Bottom = 0x4,
    Left = 0x8,
    All = Top | Right | Bottom | Left
}
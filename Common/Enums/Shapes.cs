using System;

namespace BuilderEssentials.Common.Enums;

[Flags]
public enum Shapes
{
    Rectangle = 0x2,
    TopHalfEllipse = 0x4,
    RightHalfEllipse = 0x8,
    BottomHalfEllipse = 0x16,
    LeftHalfEllipse = 0x32,
    Ellipse = TopHalfEllipse | RightHalfEllipse | BottomHalfEllipse | LeftHalfEllipse
}
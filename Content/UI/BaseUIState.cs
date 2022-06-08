using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public abstract class BaseUIState : UIState, IDisposable
{
    //Is this really needed?
    public abstract void Dispose();
}
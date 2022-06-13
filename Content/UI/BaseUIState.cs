using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public abstract class BaseUIState : UIState, IDisposable
{
    public virtual int[] BoundItemType { get; protected set; } = { -1 }; 
    public abstract void Dispose();
}
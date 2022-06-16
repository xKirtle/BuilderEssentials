using System;
using System.Collections.Generic;
using BuilderEssentials.Common.Systems;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class PixelShapesUIState : ManagedUIState<BaseShapePanel>
{
    public override List<Type> PanelTypes() => new() {
        typeof(RectangleShape)
    };
}

public abstract class BaseShapePanel : UIElement
{
    public bool IsVisible => Parent != null;
    public virtual int[] ItemBoundToDisplay { get; protected set; } = { -1 };
}
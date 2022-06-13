using System;
using System.Collections.Generic;
using BuilderEssentials.Common;
using BuilderEssentials.Content.Items;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class PaintBrushState : BaseUIState
{
    public static PaintBrushState Instance;
    public MultiWandPanel menuPanel;

    public override int[] BoundItemType => new int[]
        {ModContent.ItemType<PaintBrush>(), ModContent.ItemType<SpectrePaintBrush>()};

    public PaintBrushState() {
        Instance = this;
        menuPanel = new MultiWandPanel();
        Append(menuPanel);
    }
    
    public override void Dispose() {
        Instance = null;
    }
}

public class PaintBrushPanel : UIElement
{
    
}
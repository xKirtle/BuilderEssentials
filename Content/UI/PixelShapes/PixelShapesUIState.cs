using System;
using System.Collections.Generic;
using BuilderEssentials.Common.Systems;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class PixelShapesUIState : ManagedUIState<BaseShapePanel>
{
    public override List<Type> PanelTypes() => new() {
        
    };
}

public class BaseShapePanel : UIElement
{
    
}
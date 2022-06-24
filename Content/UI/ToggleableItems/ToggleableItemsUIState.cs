using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Assets;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class ToggleableItemsUISystem : UISystem<ToggleableItemsUIState> { }

public class ToggleableItemsUIState : ManagedUIState<BaseToggleablePanel>
{
    public override List<Type> PanelTypes() => new() {
        typeof(AutoHammerPanel),
        typeof(MultiWandPanel),
        typeof(PaintBrushPanel)
    };
    
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        for (int i = 0; i < PanelTypes().Count; i++) {
            var panel = GetUIPanel(i);
            if (!panel.ItemBoundToDisplay.Contains(Main.LocalPlayer.HeldItem.type) && panel.IsVisible) {
                panel.Remove();
                panel.Deactivate();
            }
        }
    }
}

public abstract class BaseToggleablePanel : UIElement
{
    public bool IsVisible => Parent != null;
    public virtual int[] ItemBoundToDisplay { get; protected set; } = { -1 };
    
    private bool canDisplay = false;
    public override void OnActivate() {
        canDisplay = true;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        if (canDisplay) {
            PreventElementOffScreen(this, BEPlayer.PointedScreenCoords);
            canDisplay = false;
        }
        base.Draw(spriteBatch);
    }
    
    public static void PreventElementOffScreen(UIElement element, Vector2 center, Vector2 size = default) {
        size = size == default ? new Vector2(element.Width.Pixels, element.Height.Pixels) : size;
        Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
        
        float offsetX = Utils.Clamp(center.X - size.X / 2, 0, screenSize.X - size.X);
        float offsetY = Utils.Clamp(center.Y - size.Y / 2, 0, screenSize.Y - size.Y);
        
        element.Left.Set(offsetX, 0f);
        element.Top.Set(offsetY, 0f);
        element.Recalculate();
    }
    
    public static void PreventTextOffScreen(UIElement parent, UIText uiText, Vector2 center, Vector2 centerOffset) {
        Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
        Vector2 textSize = new Vector2(uiText.GetOuterDimensions().Width, uiText.GetOuterDimensions().Height);

        float offsetX = Utils.Clamp(center.X + centerOffset.X, 0, screenSize.X - textSize.X - centerOffset.X);
        offsetX -= parent.Left.Pixels;
        float offsetY = Utils.Clamp(center.Y + centerOffset.Y, 4f, screenSize.Y - textSize.Y + centerOffset.Y / 2);
        offsetY -= parent.Top.Pixels;
        
        uiText.Left.Set(offsetX, 0);
        uiText.Top.Set(offsetY, 0);
    }
}
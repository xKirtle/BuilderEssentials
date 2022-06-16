﻿using System;
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

public class ToggleableItemsUIState : UIState, IDisposable
{
    public static ToggleableItemsUIState Instance;
    private static List<BaseToggleablePanel> uIStateElements = new();

    private static List<Type> panelTypes = new() {
        typeof(AutoHammerPanel),
        typeof(MultiWandPanel),
        typeof(PaintBrushPanel)
    };
    
    public override void OnInitialize() {
        Instance = this;

        for (var i = 0; i < panelTypes.Count; i++)
            uIStateElements.Add((BaseToggleablePanel)Activator.CreateInstance(panelTypes[i])); 
    }

    public void Dispose() {
        for (int i = 0; i < uIStateElements.Count; i++) {
            uIStateElements[i].Deactivate();
            uIStateElements[i] = null;
        }

        panelTypes = null;
    }

    public static BaseToggleablePanel GetUIPanel(UIStateType uiStateType)
        => uIStateElements[(int) uiStateType];
    
    public static T GetUIPanel<T>() where T : BaseToggleablePanel 
        => (T)uIStateElements[panelTypes.IndexOf(typeof(T))];
    
    public static void ToggleUIPanelVisibility(UIStateType uiStateType) 
        => ActuallyTogglePanelVisiblity(uIStateElements[(int) uiStateType]);
    

    public static void ToggleUIPanelVisibility<T>() where T : BaseToggleablePanel 
        => ActuallyTogglePanelVisiblity(GetUIPanel<T>());

    private static void ActuallyTogglePanelVisiblity(BaseToggleablePanel panel) {
        if (panel.Parent == null) {
            Instance.Append(panel);
            panel.Activate();
        }
        else {
            panel.Remove();
            panel.Deactivate();
        }
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        foreach (BaseToggleablePanel panel in uIStateElements) {
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

    //Kirtle: Hacky workaround to fix mouse coords not matching screen coords because of UIScale?
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
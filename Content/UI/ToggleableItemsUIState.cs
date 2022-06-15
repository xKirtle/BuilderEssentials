using System;
using System.Collections;
using System.Collections.Generic;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class ToggleableItemsUISystem : UISystem<ToggleableItemsUIState> { }

//Everything added to this UIState will display only by itself
public class ToggleableItemsUIState : UIState, IDisposable
{
    public static ToggleableItemsUIState Instance;
    private static List<BaseToggleableUIElement> panelList;
    private static readonly List<Type> panelTypes = new() {
        typeof(AutoHammerPanel),
        typeof(MultiWandPanel),
        typeof(PaintBrushPanel)
    };

    public override void OnInitialize() {
        Instance = this;
        
        panelList = new();
        foreach (Type type in panelTypes) {
            var element = Activator.CreateInstance(type) as BaseToggleableUIElement;
            panelList.Add(element);
            element.Initialize();
        }
    }

    public void Dispose() {
        for (var i = 0; i < panelList.Count; i++) {
            panelList[i].Dispose();
            panelList[i] = null;
        }
    }

    public static BaseToggleableUIElement GetUIPanel(UIStateType type) {
        if (type == UIStateType.None || type == UIStateType.Count)
            return null;
        
        return panelList[(int) type - 1];
    }

    public static T GetUIPanel<T>() where T : BaseToggleableUIElement {
        if (Nullable.GetUnderlyingType(typeof(T)) != null)
            return null;

        return (T)panelList[panelTypes.IndexOf(typeof(T))];
    }

    public static void ChangeOrTogglePanel(UIStateType type) {
        var panel = GetUIPanel(type);

        if (panel.Parent != null) {
            panel.Deactivate();
            panel.Remove();
        }
        else {
            panel.Activate();
            Instance.Append(panel);
        }
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
    }

    protected override void DrawChildren(SpriteBatch spriteBatch) {
        base.DrawChildren(spriteBatch);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch) {
        base.DrawSelf(spriteBatch);
    }
}

public abstract class BaseToggleableUIElement : UIElement, IDisposable
{
    public virtual int[] BoundItemType { get; protected set; } = { ItemID.None };

    public void Dispose() { }

    public static void PreventElementOffScreen(UIElement element, Vector2 center, Vector2 size = default) {
        size = size == default ? new Vector2(element.Width.Pixels, element.Height.Pixels) : size;
        Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.UIScale;
        
        float offsetX = Utils.Clamp(center.X - size.X / 2, 0, screenSize.X - size.X);
        float offsetY = Utils.Clamp(center.Y - size.Y / 2, 0, screenSize.Y - size.Y);
        
        element.Left.Set(offsetX, 0f);
        element.Top.Set(offsetY, 0f);
    }

    //This method is wacky
    public static void PreventTextOffScreen(UIElement parent, UIText uiText, Vector2 center, Vector2 centerOffset) {
        Vector2 screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
        Vector2 textSize = FontAssets.MouseText.Value.MeasureString(uiText.Text);

        float offsetX = Utils.Clamp(center.X + centerOffset.X, 0, screenSize.X - textSize.X);
        offsetX -= parent.Left.Pixels - centerOffset.X;
        float offsetY = Utils.Clamp(center.Y + centerOffset.Y, 4f, screenSize.Y - textSize.Y + centerOffset.Y / 2);
        offsetY -= parent.Top.Pixels;
        
        uiText.Left.Set(offsetX, 0);
        uiText.Top.Set(offsetY, 0);
    }
}
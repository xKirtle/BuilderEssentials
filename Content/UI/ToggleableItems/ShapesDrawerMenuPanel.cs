using System;
using BuilderEssentials.Assets;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace BuilderEssentials.Content.UI;

public class ShapesDrawerMenuPanel : BaseToggleablePanel
{
    public override bool IsHoldingBindingItem() 
        => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ShapesDrawer>();

    private const float ParentWidth = 50f, ParentHeight = 200f;
    private UIImage builderSquirrel;
    private UIImage builderSquirrelHover;
    private bool elementHovered;
    private string text = "";
    private bool isMenuOpen = false;
    
    public override void OnInitialize() {
        Width.Set(ParentWidth, 0);
        Height.Set(ParentHeight, 0);
        Left.Set(Main.GameMode == GameModeID.Creative ? 75f : 32f, 0);
        Top.Set(267f, 0);
        SetPadding(0);

        builderSquirrel = new(AssetsLoader.GetAssets($"{AssetsID.ShapesMenu}/SquirrelToggle"));
        builderSquirrel.Width.Set(34f, 0);
        builderSquirrel.Height.Set(32f, 0);
        
        builderSquirrelHover = new(AssetsLoader.GetAssets($"{AssetsID.ShapesMenu}/SquirrelToggleHover"));
        builderSquirrelHover.Width.Set(34f, 0);
        builderSquirrelHover.Height.Set(32f, 0);
        
        builderSquirrel.OnMouseOver += (__, _) => {
            Append(builderSquirrelHover);
            elementHovered = true;
        };
        
        builderSquirrelHover.OnMouseOut += (__, _) => {
            builderSquirrelHover.Remove();
            elementHovered = false;
        };
        
        builderSquirrelHover.OnClick += (__, _) => {
            isMenuOpen = !isMenuOpen;  
        };
        
        builderSquirrelHover.OnMouseDown += (__, _) => {
            builderSquirrel.Top.Set(builderSquirrel.Top.Pixels + 1f, 0);  
            builderSquirrelHover.Top.Set(builderSquirrelHover.Top.Pixels + 1f, 0);  
        };

        builderSquirrelHover.OnMouseUp += (__, _) => {
            builderSquirrel.Top.Set(builderSquirrel.Top.Pixels - 1f, 0);  
            builderSquirrelHover.Top.Set(builderSquirrelHover.Top.Pixels - 1f, 0);  
        };
        
        Append(builderSquirrel);
    }
    
    public override void UpdateRegardlessOfVisibility() {
        if ((IsHoldingBindingItem() && !IsVisible) || (!IsHoldingBindingItem() && IsVisible))
            ToggleableItemsUIState.TogglePanelVisibility<ShapesDrawerMenuPanel>();
    }

    public override void OnActivate() { }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        Left.Set(Main.GameMode == GameModeID.Creative ? 75f : 32f, 0);
        
        if (IsMouseHovering && elementHovered)
            Main.LocalPlayer.mouseInterface = true;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        if (!Main.playerInventory) return;
        base.Draw(spriteBatch);

        if (elementHovered) {
            Main.LocalPlayer.cursorItemIconEnabled = false;
            text = (isMenuOpen ? "Close" : "Open") + " shape selection menu";
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text,
                BEPlayer.PointedScreenCoords + new Vector2(22f, 22f), Color.White, 0f, Vector2.Zero, Vector2.One);
        }
    }
}
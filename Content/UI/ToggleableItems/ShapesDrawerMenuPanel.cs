using System;
using BuilderEssentials.Assets;
using BuilderEssentials.Common.Enums;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace BuilderEssentials.Content.UI;

//This might be one of the worst pieces of code I ever wrote
public class ShapesDrawerMenuPanel : BaseToggleablePanel
{
    public override bool IsHoldingBindingItem() 
        => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ShapesDrawer>();

    private const float ParentWidth = 230f, ParentHeight = 120f;
    private UIImage builderSquirrel;
    private UIImage builderSquirrelHover;
    private bool elementHovered;
    private string text = "";
    private bool isMenuOpen = false;
    private ShapesMenuOption[] menuOptions;
    public Shapes SelectedShape { get; private set; }
    public ShapeSide SelectedShapeSide { get; private set; }
    public bool IsFilled { get; private set; }

    public override void OnInitialize() {
        Width.Set(ParentWidth, 0);
        Height.Set(ParentHeight, 0);
        Left.Set(Main.GameMode == GameModeID.Creative ? 75f : 32f, 0);
        Top.Set(260f, 0);
        SetPadding(0);

        builderSquirrel = new(AssetsLoader.GetAssets($"{AssetsID.ShapesMenu}/SquirrelToggle"));
        builderSquirrel.Width.Set(34f, 0);
        builderSquirrel.Height.Set(32f, 0);
        builderSquirrel.Top.Set(7f, 0);
        
        builderSquirrelHover = new(AssetsLoader.GetAssets($"{AssetsID.ShapesMenu}/SquirrelToggleHover"));
        builderSquirrelHover.Width.Set(34f, 0);
        builderSquirrelHover.Height.Set(32f, 0);
        builderSquirrelHover.Top.Set(7f, 0);
        
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

            if (isMenuOpen) {
                for (int i = 0; i < 3; i++)
                    Append(menuOptions[i]);
            }
            else {
                for (int i = 0; i < menuOptions.Length; i++)
                    menuOptions[i].Remove();
            }
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

        menuOptions = new ShapesMenuOption[7];
        for (int i = 0; i < menuOptions.Length; i++)
            menuOptions[i] = new ShapesMenuOption(i);

        for (int i = 0; i < 3; i++)
            menuOptions[i].Left.Set(50f + 43f * i + 4f, 0);

        for (int i = 3; i < menuOptions.Length; i++) {
            menuOptions[i].Top.Set(43f, 0);
            menuOptions[i].Left.Set(50f + 43f * (i-3) + 4f, 0);
        }

        void ClearAllSelections(int startIndex = 0) {
            for (int i = startIndex; i < menuOptions.Length; i++)
                if (menuOptions[i].IsSelected && i != 2)
                    menuOptions[i].ToggleSelection();
        }

        void ToggleHalfShapesVisibility(bool removeOnly = false) {
            for (int i = 3; i < menuOptions.Length; i++)
                if (menuOptions[i].Parent != null || removeOnly)
                    menuOptions[i].Remove();
                else
                    Append(menuOptions[i]);
        }
        
        //Ellipse
        menuOptions[0].OnClick += (__, _) => {
            ClearAllSelections();
            menuOptions[0].ToggleSelection();
            SelectedShape = Shapes.Ellipse;
            SelectedShapeSide = ShapeSide.All;
            ToggleHalfShapesVisibility();
        };
        
        //Rectangle
        menuOptions[1].OnClick += (__, _) => {
            ClearAllSelections();
            menuOptions[1].ToggleSelection();
            SelectedShape = Shapes.Rectangle;
            SelectedShapeSide = ShapeSide.All;
            ToggleHalfShapesVisibility(removeOnly: true);
        };
        
        //Fill
        menuOptions[2].OnClick += (__, _) => {
            for (int i = 0; i < menuOptions.Length; i++)
                if (i != 2) menuOptions[i].ToggleFill();
            
            menuOptions[2].ToggleSelection();
            IsFilled = menuOptions[2].IsSelected;
        };
        
        //Half Ellipses
        for (int i = 3; i < menuOptions.Length; i++) {
            int index = i;
            menuOptions[i].OnClick += (__, _) => {
                if (menuOptions[index].IsSelected) {
                    menuOptions[index].ToggleSelection();
                    SelectedShapeSide = ShapeSide.All;
                    return;
                }
                
                ClearAllSelections(startIndex: 3);
                menuOptions[index].ToggleSelection();
                
                switch (index) {
                    case 3:
                        SelectedShapeSide = ShapeSide.Top;
                        break;
                    case 4:
                        SelectedShapeSide = ShapeSide.Right;
                        break;
                    case 5:
                        SelectedShapeSide = ShapeSide.Bottom;
                        break;
                    case 6:
                        SelectedShapeSide = ShapeSide.Left;
                        break;
                }
            };
        }

        for (int i = 0; i < menuOptions.Length; i++)
            menuOptions[i].OnInitialize();
        
        //Leaving rectangle as the default selection
        menuOptions[1].ToggleSelection();
        SelectedShape = Shapes.Rectangle;
        SelectedShapeSide = ShapeSide.All;
    }
    
    public override void UpdateRegardlessOfVisibility() {
        if ((IsHoldingBindingItem() && !IsVisible) || (!IsHoldingBindingItem() && IsVisible))
            ToggleableItemsUIState.TogglePanelVisibility<ShapesDrawerMenuPanel>();
    }

    public override void OnActivate() { }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        Left.Set(Main.GameMode == GameModeID.Creative ? 75f : 32f, 0);
        
        if (IsMouseHovering || elementHovered)
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

public class ShapesMenuOption : UIImage
{
    public int OptionIndex { get; }
    private UIImage actualOption;
    public bool IsSelected { get; private set; }
    public bool IsFilled { get; private set; }

    public ShapesMenuOption(int optionIndex) : base(AssetsLoader.GetAssets($"{AssetsID.ShapesMenu}/Frame")) {
        OptionIndex = optionIndex;
        RemoveFloatingPointsFromDrawPosition = true;
    }

    public override void OnInitialize() {
        Width.Set(52f, 0);
        Height.Set(52f, 0);
        ImageScale = .75f;
        SetPadding(0);
        
        actualOption = new UIImage(AssetsLoader.GetAssets($"{AssetsID.ShapesMenu}/Shape{OptionIndex}")) {
            Width = Height = new StyleDimension(52f, 0),
            IgnoresMouseInteraction = true,
            RemoveFloatingPointsFromDrawPosition =  true,
            ImageScale = .75f
        };
        
        Append(actualOption);
        
        OnMouseOver += (__, _) => {
            SetImage(AssetsLoader.GetAssets($"{AssetsID.ShapesMenu}/FrameHover"));
            SoundEngine.PlaySound(SoundID.MenuTick);
        };
        
        OnMouseOut += (__, _) => {
            SetImage(AssetsLoader.GetAssets($"{AssetsID.ShapesMenu}/Frame"));
            SoundEngine.PlaySound(SoundID.MenuTick);
        };
    }
    
    public void ToggleSelection() {
        IsSelected = !IsSelected;
        UpdateTexture();
    }

    public void ToggleFill() {
        IsFilled = !IsFilled;
        UpdateTexture();
    }

    public void UpdateTexture() =>
        actualOption.SetImage(AssetsLoader.GetAssets(
            $"{AssetsID.ShapesMenu}/{(IsFilled ? "Filled" : "")}{(IsSelected ? "Selected" : "")}Shape{OptionIndex}"));

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        if (IsMouseHovering)
            Main.LocalPlayer.mouseInterface = true;
    }
}
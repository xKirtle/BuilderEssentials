using System;
using BuilderEssentials.Assets;
using BuilderEssentials.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace BuilderEssentials.Content.UI;

public class MultiWandPanel : UIElement
{
    private const float ParentWidth = 180f, ParentHeight = 160f;
    private const int ElementsCount = 6;
    private UIImageButton[] elements;
    private bool elementHovered;
    public int selectedIndex;
    private UIText hoverText;

    public override void OnInitialize() {
        Width.Set(ParentWidth, 0);
        Height.Set(ParentHeight, 0);
        Left.Set(Main.screenWidth / 2 - ParentWidth, 0);
        Top.Set(Main.screenHeight / 2 - ParentHeight, 0);
        SetPadding(0);
        
        elements = new UIImageButton[ElementsCount];
        for (int i = 0; i < ElementsCount; i++)
            elements[i] = new UIImageButton(AssetsLoader.GetAssets(AssetsID.MultiWand)[i]);
        
        //Define our shape
        float offsetX = ParentWidth / 3f;
        float offsetY = (float)(Math.Sqrt(11) / 4) * ParentHeight;
        
        Vector2 elementOffset = new Vector2(22, 22);
        Vector2[] buttonPositions = new[] {
            new Vector2(offsetX, offsetY) - elementOffset,
            new Vector2(ParentWidth - offsetX, offsetY) - elementOffset,
            new Vector2(ParentWidth - 24, ParentHeight / 2) - elementOffset,
            new Vector2(ParentWidth - offsetX, ParentHeight - offsetY) - elementOffset,
            new Vector2(offsetX, ParentHeight - offsetY) - elementOffset,
            new Vector2(24, ParentHeight / 2) - elementOffset
        };
        
        for (int i = 0; i < ElementsCount; i++) {
            int index = i;
            elements[i].Left.Set(buttonPositions[i].X, 0f);
            elements[i].Top.Set(buttonPositions[i].Y, 0f);
            elements[i].OnClick += (__, _) => ElementOnClick(index);
            elements[i].OnMouseOver += (__, _) => ElementOnMouseOver(index);
            elements[i].OnMouseOut += (__, _) => ElementOnMouseOut();
        }

        elements[selectedIndex].SetVisibility(1f, 1f);
        
        //Append to the main panel
        for (int i = 0; i < ElementsCount; i++)
            Append(elements[i]);
    }
    
    private void ElementOnClick(int index) {
        for (int i = 0; i < ElementsCount; i++)
            elements[i].SetVisibility(.75f, .4f);

        elements[index].SetVisibility(1f, 1f);
        selectedIndex = index;
    }
    
    private readonly string[] text = {
        "Places living wood, consumes [i/s:9]", "Places bones, consumes [i/s:154]", 
        "Places leaves, consumes [i/s:9]", "Places hives, consumes [i/s:1124]", 
        "Places living rich mahogany, consumes [i/s:620]", 
        "Places rich mahogany leaves, consumes [i/s:620]"
    };
    
    private void ElementOnMouseOver(int index) {
        elementHovered = true;
        hoverText = new UIText(text[index], 0.4f, true);
        hoverText.Left.Set(BEPlayer.CachedPointedCoord.X + 22 - Left.Pixels, 0);
        hoverText.Top.Set(BEPlayer.CachedPointedCoord.Y - 22 - Top.Pixels, 0);
        Append(hoverText);
    }

    private void ElementOnMouseOut() {
        elementHovered = false;
        hoverText?.Remove();
    }

    public override void OnActivate() {
        UISystem.PreventOffScreen(this, BEPlayer.CachedPointedCoord);
    }
    
    public override void Update(GameTime gameTime) {
        if (IsMouseHovering && elementHovered) {
            Main.LocalPlayer.mouseInterface = true;
            hoverText?.Left.Set(BEPlayer.CachedPointedCoord.X + 22 - Left.Pixels, 0);
            hoverText?.Top.Set(BEPlayer.CachedPointedCoord.Y - 22 - Top.Pixels, 0);
        }
    }
}
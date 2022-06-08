using System;
using BuilderEssentials.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI;

public class AutoHammerPanel : UIElement
{
    private const float ParentWidth = 170f, ParentHeight = 150f;
    private const int elementsCount = 6;
    private UIImageButton[] elements;
    private bool elementHovered;
    public int selectedIndex = -1;
    public SlopeType slopeType;
    public bool isHalfBlock;

    public AutoHammerPanel() {
        Width.Set(ParentWidth, 0);
        Height.Set(ParentHeight, 0);
        Left.Set(Main.screenWidth / 2 - ParentWidth, 0);
        Top.Set(Main.screenHeight / 2 - ParentHeight, 0);
        SetPadding(0);
        
        //Initialize image buttons
        string texturePath = "BuilderEssentials/Assets/UI/AutoHammer/AH";
        elements = new UIImageButton[elementsCount];
        for (int i = 0; i < elementsCount; i++)
            elements[i] = new UIImageButton(ModContent.Request<Texture2D>(texturePath + i, AssetRequestMode.ImmediateLoad));
        
        //Define our Wheel (circle)
        const int radius = 60;
        const double angle = Math.PI / 3;
        const int ElementsSize = 44;

        for (int i = 0; i < elementsCount; i++) {
            int index = i;
            Vector2 offset = new Vector2(ParentWidth - ElementsSize, ParentHeight - ElementsSize) / 2;
            double x = offset.X + (radius * Math.Cos(angle * (i + 3)));
            double y = offset.Y - (radius * Math.Sin(angle * (i + 3)));
            elements[i].Left.Set((float) x, 0);
            elements[i].Top.Set((float) y, 0);
            elements[i].SetVisibility(.75f, .4f);
            elements[i].OnClick += (__, _) => ElementOnClick(index);
            elements[i].OnMouseOver += (__, _) => elementHovered = true;
            elements[i].OnMouseOut += (__, _) => elementHovered = false;
        }

        //Correct display of previously toggled settings
        if (selectedIndex != -1)
            elements[selectedIndex].SetVisibility(1f, 1f);
        
        //Append to the main panel
        for (int i = 0; i < elementsCount; i++)
            Append(elements[i]);
    }
    
    private void ElementOnClick(int index) {
        for (int i = 0; i < elementsCount; i++)
            elements[i].SetVisibility(.75f, .4f);

        if (selectedIndex != index) {
            elements[index].SetVisibility(1f, 1f);
            selectedIndex = index;
        }
        else selectedIndex = -1;
        
        //Assign slopeType and IsHalfBlock based on selectedIndex (and its respective UI image)
        SlopeType[] types = new[] {
            SlopeType.SlopeDownLeft, SlopeType.SlopeDownRight, SlopeType.SlopeUpLeft, 
            SlopeType.SlopeUpRight, SlopeType.Solid
        };

        isHalfBlock = selectedIndex == 5;
        if (selectedIndex != -1 && !isHalfBlock)
            slopeType = types[selectedIndex];
    }

    public override void OnActivate() {
        UISystem.PreventOffScreen(this, ModContent.GetInstance<UISystem>().cachedMouseCoords);
    }

    public override void Update(GameTime gameTime) {
        if (IsMouseHovering && elementHovered)
            Main.LocalPlayer.mouseInterface = true;
    }
}
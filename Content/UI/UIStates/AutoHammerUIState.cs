using System;
using BuilderEssentials.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.UI.UIStates;

public class AutoHammerUIState : BaseUIState
{
    private const float ParentWidth = 170f, ParentHeight = 150f;
    private const int elementsCount = 6;
    private UIElement _wheel;
    private UIImageButton[] elements;
    private bool elementHovered;
    private int selectedIndex = -1;
    
    public override void OnInitialize()
    {
        base.OnInitialize();

        _wheel = new UIElement()
        {
            Width = new StyleDimension(ParentWidth, 0f),
            Height = new StyleDimension(ParentHeight, 0f),
            Left = new StyleDimension(Main.screenWidth / 2 - ParentWidth, 0f),
            Top = new StyleDimension(Main.screenHeight / 2 - ParentHeight, 0f),
            PaddingBottom = PaddingLeft = PaddingRight = PaddingTop = 0
        };
        
        //Initialize image buttons
        string texturePath = "BuilderEssentials/Assets/UI/AutoHammer/AH";
        elements = new UIImageButton[elementsCount];
        for (int i = 0; i < elementsCount; i++)
            elements[i] = new UIImageButton(ModContent.Request<Texture2D>(texturePath + i));
        
        //Define our Wheel (circle)
        const int radius = 60;
        const double angle = Math.PI / 3;
        const int ElementsSize = 44;

        for (int i = 0; i < elementsCount; i++)
        {
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
            _wheel.Append(elements[i]);

        PreventParentOffScreen();
        Append(_wheel);
    }
    
    private void ElementOnClick(int index)
    {
        for (int i = 0; i < elementsCount; i++)
            elements[i].SetVisibility(.75f, .4f);

        if (selectedIndex != index)
        {
            elements[index].SetVisibility(1f, 1f);
            selectedIndex = index;
        }
        else selectedIndex = -1;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        // if (_wheel.IsMouseHovering)
        //     Main.LocalPlayer.mouseInterface = false;
        //
        // if (elementHovered)
        //     Main.LocalPlayer.mouseInterface = true;
    }

    public void PreventParentOffScreen()
    {
        Vector2 cachedCoords = ModContent.GetInstance<UISystem>().cachedMouseCoords;
        
        float offsetX = cachedCoords.X - ParentWidth / 2 > 0 ? cachedCoords.X - ParentWidth / 2 : 0;
        offsetX = cachedCoords.X + ParentWidth / 2 > Main.screenWidth ? Main.screenWidth - cachedCoords.X : offsetX;
        float offsetY = cachedCoords.Y - ParentHeight / 2 > 0 ? cachedCoords.Y - ParentHeight / 2 : 0;
        offsetY = cachedCoords.Y + ParentHeight / 2 > Main.screenHeight ? cachedCoords.Y - ParentHeight : offsetY;

        _wheel.Left.Set(offsetX, 0);
        _wheel.Top.Set(offsetY, 0);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public override void OnActivate()
    {
        PreventParentOffScreen();
        //Retrieve most recent data to fill the UI
    }

    public override void OnDeactivate()
    {
        //Reset variables (or null them out to unallocate memory) to keep the UI ready to be activated again
    }
    
    public void Dispose()
    {
        //Dispose of static references such as textures
    }
}
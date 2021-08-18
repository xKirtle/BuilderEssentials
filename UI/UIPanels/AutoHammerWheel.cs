using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using BuilderEssentials.UI.Elements;
using BuilderEssentials.Utilities;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;

namespace BuilderEssentials.UI.UIPanels
{
    internal class AutoHammerWheel : CustomUIPanel
    {
        private const float width = 170f, height = 150f;
        private const int elementsCount = 6;
        private UIImageButton[] elements;
        private bool elementHovered;
        public int selectedIndex = -1;
        public SlopeType slopeType;
        public bool IsHalfBlock;

        public AutoHammerWheel()
        {
            Width.Set(width, 0);
            Height.Set(height, 0);
            Left.Set(Main.screenWidth / 2 - width, 0);
            Top.Set(Main.screenHeight / 2 - height, 0);
            SetPadding(0);
            BorderColor = Color.Transparent;
            BackgroundColor = Color.Transparent;

            //Initialize image buttons
            string texturePath = "BuilderEssentials/Textures/UIElements/AutoHammer/AH";
            elements = new UIImageButton[elementsCount];
            for (int i = 0; i < elementsCount; i++)
                elements[i] = new UIImageButton(HelperMethods.RequestTexture(texturePath + i));

            //Define our Wheel (circle)
            const int radius = 60;
            const double angle = Math.PI / 3;
            const int elementsSize = 44;

            for (int i = 0; i < elementsCount; i++)
            {
                int index = i;
                Vector2 offset = new Vector2(width - elementsSize, height - elementsSize) / 2;
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
            
            //Assign slopeType and IsHalfBlock based on selectedIndex (and its respective UI image)
            IsHalfBlock = false;
            slopeType = SlopeType.Solid;
            switch (selectedIndex)
            {
                case 0:
                    slopeType = SlopeType.SlopeDownLeft;
                    break;
                case 1:
                    slopeType = SlopeType.SlopeDownRight;
                    break;
                case 2:
                    slopeType = SlopeType.SlopeUpLeft;
                    break;
                case 3:
                    slopeType = SlopeType.SlopeUpRight;
                    break;
                case 4:
                    IsHalfBlock = true;
                    break;
                default:
                    break;
            }
        }
        
        public void Update()
        {
            if (!Visible) return;
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = false;
            
            if (elementHovered)
                Main.LocalPlayer.mouseInterface = true;
        }

        public override void Show()
        {
            base.Show();

            //Making sure the UI will stay within our screen
            Vector2 cachedMouse = UIModSystem.cachedMouseCoords;
            float offsetX = cachedMouse.X - width / 2 > 0 ? cachedMouse.X - width / 2 : 0;
            offsetX = cachedMouse.X + width / 2 > Main.screenWidth ? Main.screenWidth - width : offsetX;
            float offsetY = cachedMouse.Y - height / 2 > 0 ? cachedMouse.Y - height / 2 : 0;
            offsetY = cachedMouse.Y + height / 2 > Main.screenHeight ? Main.screenHeight - height : offsetY;

            Left.Set(offsetX, 0);
            Top.Set(offsetY, 0);
        }
    }
}
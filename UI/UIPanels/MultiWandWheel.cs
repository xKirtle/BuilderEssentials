﻿using System;
using System.Linq;
using BuilderEssentials.UI.Elements;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.UI.UIPanels
{
    public class MultiWandWheel : CustomUIPanel
    {
        private const float width = 170f, height = 150;
        private const int elementsCount = 6;
        private UIImageButton[] elements;
        private bool elementHovered;
        public int selectedIndex;
        private UIText hoverText;

        public MultiWandWheel(float scale = 1f, float opacity = 1f) : base(scale, opacity)
        {
            Width.Set(width, 0);
            Height.Set(height, 0);
            Left.Set(Main.screenWidth / 2 - width, 0);
            Top.Set(Main.screenHeight / 2 - height, 0);
            SetPadding(0);
            BorderColor = Color.Transparent;
            BackgroundColor = Color.Transparent;

            //Initialize image buttons
            string texturePath = "BuilderEssentials/Textures/UIElements/MultiWandWheel/WandWheel";
            elements = new UIImageButton[elementsCount];
            for (int i = 0; i < elementsCount; i++)
                elements[i] = new UIImageButton(ModContent.GetTexture(texturePath + i));

            //Define our Wheel (circle)
            const int radius = 60;
            const double angle = Math.PI / 3;

            for (int i = 0; i < elementsCount; i++)
            {
                int index = i;
                //The magic number 22f is half the width/height of the elements[i] Texture size (44x44 pixels)
                Vector2 offset = new Vector2(width / 2 - 22f, height / 2 - 22f);
                double x = offset.X + (radius * Math.Cos(angle * (i + 3)));
                double y = offset.Y - (radius * Math.Sin(angle * (i + 3)));
                elements[i].Left.Set((float) x, 0);
                elements[i].Top.Set((float) y, 0);
                elements[i].SetVisibility(.75f, .4f);
                elements[i].OnClick += (__, _) => ElementOnClick(index);
                elements[i].OnMouseOver += (__, _) => ElementOnMouseOver(index);
                elements[i].OnMouseOut += (__, _) => ElementOnMouseOut();
            }

            //Correct display of previously toggled settings
            elements[selectedIndex].SetVisibility(1f, 1f);

            //Append to the main panel
            for (int i = 0; i < elementsCount; i++)
                Append(elements[i]);
        }

        private void ElementOnClick(int index)
        {
            for (int i = 0; i < elementsCount; i++)
                elements[i].SetVisibility(.75f, .4f);

            elements[index].SetVisibility(1f, 1f);
            selectedIndex = index;
        }

        private void ElementOnMouseOver(int index)
        {
            string text = "";
            switch (index)
            {
                case 0:
                    text = "Places living wood (wood)";
                    break;
                case 1:
                    text = "Places bones (bone)";
                    break;
                case 2:
                    text = "Places leaves (wood)";
                    break;
                case 3:
                    text = "Places Hives (hive)";
                    break;
                case 4:
                    text = "Places living rich mahogany (rich mahogany)";
                    break;
                case 5:
                    text = "Places rich mahogany leaves (rich mahogany)";
                    break;
            }

            elementHovered = true;
            hoverText = new UIText(text, 1, false);
            hoverText.Left.Set(Main.mouseX + 22 - Left.Pixels, 0);
            hoverText.Top.Set(Main.mouseY + 22 - Top.Pixels, 0);
            Append(hoverText);
        }

        private void ElementOnMouseOut()
        {
            elementHovered = false;
            hoverText?.Remove();
        }

        public void Update()
        {
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = false;
            
            if (!Visible) return;
            if (elementHovered)
            {
                Main.LocalPlayer.mouseInterface = true;
                hoverText?.Left.Set(Main.mouseX + 22 - Left.Pixels, 0);
                hoverText?.Top.Set(Main.mouseY + 22 - Top.Pixels, 0);
            }
        }

        public override void Show()
        {
            base.Show();

            //Making sure the UI will stay within our screen
            float offsetX = Main.mouseX - width / 2 > 0 ? Main.mouseX - width / 2 : 0;
            offsetX = Main.mouseX + width / 2 > Main.screenWidth ? Main.screenWidth - width : offsetX;
            float offsetY = Main.mouseY - height / 2 > 0 ? Main.mouseY - height / 2 : 0;
            offsetY = Main.mouseY + height / 2 > Main.screenHeight ? Main.screenHeight - height : offsetY;
            
            Left.Set(offsetX, 0);
            Top.Set(offsetY, 0);
        }
    }
}
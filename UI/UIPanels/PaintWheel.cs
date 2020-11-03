using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using BuilderEssentials.UI.Elements;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels
{
    public class PaintWheel : CustomUIPanel
    {
        private const float width = 430f, height = 340f;
        private UIImageButton[] colorElements;
        private UIImageButton[] toolElements;
        private bool elementHovered;
        public int selectedIndex;

        public PaintWheel(float scale = 1f, float opacity = 1f)
        {
            Width.Set(width, 0);
            Height.Set(height, 0);
            Left.Set(Main.screenWidth / 2 - width, 0);
            Top.Set(Main.screenHeight / 2 - height, 0);
            BorderColor = Color.Red;
            BackgroundColor = Color.Transparent;

            string colorsTexturePath = "BuilderEssentials/Textures/UIElements/Paint/Paint";
            colorElements = new UIImageButton[30];
            for (int i = 0; i < 30; i++)
                colorElements[i] = new UIImageButton(ModContent.GetTexture(colorsTexturePath + i));

            toolElements = new UIImageButton[3];
            for (int i = 0; i < 3; i++)
                toolElements[i] = new UIImageButton(ModContent.GetTexture(colorsTexturePath + "Tool" + i));

            int radius = 155;
            double angle = Math.PI / 12;
            const float colorElementSize = 40f;

            for (int i = 0; i < 12; i++)
            {
                int index = i;
                Vector2 offset = new Vector2(width - colorElementSize, height - colorElementSize) / 2;
                double x = offset.X - (radius * Math.Cos(angle * (i + .48)) * 0.95);
                double y = offset.Y - (radius * Math.Sin(angle * (i + .48)));
                colorElements[i].Left.Set((float) x - colorElementSize / 4, 0);
                colorElements[i].Top.Set((float) y + 40, 0);
                colorElements[i].SetVisibility(1f, 0.85f);
                colorElements[i].OnMouseDown += (__, _) => ColorSelected(index);
                Append(colorElements[i]);
            }

            radius = 190;

            for (int i = 12; i < 24; i++)
            {
                int index = i;
                Vector2 offset = new Vector2(width - colorElementSize, height - colorElementSize) / 2;
                double x = offset.X + (radius * Math.Cos(angle * (i + .48)) * 1);
                double y = offset.Y - (radius * Math.Sin(-angle * (i + .48)));
                colorElements[i].Left.Set((float) x - colorElementSize / 4, 0);
                colorElements[i].Top.Set((float) y + 30, 0);
                colorElements[i].SetVisibility(1f, 0.85f);
                colorElements[i].OnMouseDown += (__, _) => ColorSelected(index);
                Append(colorElements[i]);
            }

            radius = 95;
            angle = Math.PI / 6;

            for (int i = 24; i < 30; i++)
            {
                int index = i;
                Vector2 offset = new Vector2(width - colorElementSize, height - colorElementSize) / 2;
                double x = offset.X - (radius * Math.Cos(angle * (i + .50)) * 1.10);
                double y = offset.Y + (radius * Math.Sin(-angle * (i + .50)) * 1.25);
                colorElements[i].Left.Set((float) x - colorElementSize / 4, 0);
                colorElements[i].Top.Set((float) y + 50, 0);
                colorElements[i].SetVisibility(1f, 0.85f);
                colorElements[i].OnMouseDown += (__, _) => ColorSelected(index);
                Append(colorElements[i]);
            }

            for (int i = 0; i < 3; i++)
            {
                double x = (width / 3) * i + 20f;
                double y = (width / 2) + 10f;
                toolElements[i].Left.Set((float) x - colorElementSize / 4, 0);
                toolElements[i].Top.Set((float) y, 0);
                toolElements[i].SetVisibility(1f, .8f);
                Append(toolElements[i]);
            }
        }

        private void ColorSelected(int index)
        {
            //TODO: Improve golden border and display crosses on colors not present in inventory/piggy/vault?
            
            string texturePath = "BuilderEssentials/Textures/UIElements/Paint/";
            for (int i = 0; i < colorElements.Length; i++)
            {
                colorElements[i].SetVisibility(1f, 0.85f);
                colorElements[i].SetImage(ModContent.GetTexture(texturePath + "Paint" + i));
            }

            if (selectedIndex != index)
            {
                colorElements[index].SetVisibility(1f, 1f);
                colorElements[index].SetImage(ModContent.GetTexture(texturePath + "Selected/Paint" + index));
                selectedIndex = index;
            }
        }

        public void Update()
        {
        }
    }
}
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using BuilderEssentials.UI.Elements;

namespace BuilderEssentials.UI.UIPanels
{
    public class PaintWheel : CustomUIPanel
    {
        private const float width = 430f, height = 340f;
        private UIImageButton[] colorElements;
        private UIImageButton[] toolElements;
        private CustomUIImage[] noPaintOverlay;
        private CustomUIImage colorOverlay;
        private CustomUIImage toolOverlay;
        private bool[] colorAvailable;
        private bool elementHovered;
        private int[] paints;
        public int colorIndex = -1;
        public int toolIndex;

        public PaintWheel(float scale = 1f, float opacity = 1f)
        {
            paints = new int[30];
            for (int i = 0; i < 27; i++) paints[i] = (1073 + i); //Basic && Deep colors type
            for (int i = 0; i < 3; i++) paints[i + 27] = (1966 + i); //Extra Color Effects type

            Width.Set(width, 0);
            Height.Set(height, 0);
            Left.Set(Main.screenWidth / 2 - width, 0);
            Top.Set(Main.screenHeight / 2 - height, 0);
            BorderColor = Color.Red;
            BackgroundColor = Color.Transparent;

            string texturePath = "BuilderEssentials/Textures/UIElements/Paint/";

            colorElements = new UIImageButton[30];
            noPaintOverlay = new CustomUIImage[30];
            colorAvailable = new bool[30];
            for (int i = 0; i < 30; i++)
            {
                colorElements[i] = new UIImageButton(ModContent.GetTexture(texturePath + "Paint" + i));
                noPaintOverlay[i] = new CustomUIImage(ModContent.GetTexture(texturePath + "NoPaint"), 1f);
            }

            toolElements = new UIImageButton[3];
            for (int i = 0; i < 3; i++)
                toolElements[i] = new UIImageButton(ModContent.GetTexture(texturePath + "Tool" + i));

            colorOverlay = new CustomUIImage(ModContent.GetTexture(texturePath + "PaintSelected"), 1f);
            toolOverlay = new CustomUIImage(ModContent.GetTexture(texturePath + "ToolSelected"), 1f);

            int radius = 155;
            double angle = Math.PI / 12;
            const float colorElementSize = 40f;

            for (int i = 0; i < 12; i++)
            {
                int index = i;
                Vector2 offset = new Vector2(width - colorElementSize, height - colorElementSize) / 2;
                double x = offset.X - (radius * Math.Cos(angle * (i + .48)) * 0.95);
                double y = offset.Y - (radius * Math.Sin(angle * (i + .48)));
                var left = new StyleDimension((float) x - colorElementSize / 4, 0);
                var top = new StyleDimension((float) y + 40, 0);
                colorElements[i].Left = noPaintOverlay[i].Left = left;
                colorElements[i].Top = noPaintOverlay[i].Top = top;
                colorElements[i].SetVisibility(1f, 0.85f);
                colorElements[i].OnMouseDown += (__, _) => ColorSelected(index);
                colorElements[i].OnMouseOver += (__, _) => elementHovered = true;
                colorElements[i].OnMouseOut += (__, _) => elementHovered = false;
                Append(colorElements[i]);
                Append(noPaintOverlay[i]);
            }

            radius = 190;

            for (int i = 12; i < 24; i++)
            {
                int index = i;
                Vector2 offset = new Vector2(width - colorElementSize, height - colorElementSize) / 2;
                double x = offset.X + (radius * Math.Cos(angle * (i + .48)) * 1);
                double y = offset.Y - (radius * Math.Sin(-angle * (i + .48)));
                var left = new StyleDimension((float) x - colorElementSize / 4, 0);
                var top = new StyleDimension((float) y + 30, 0);
                colorElements[i].Left = noPaintOverlay[i].Left = left;
                colorElements[i].Top = noPaintOverlay[i].Top = top;
                colorElements[i].SetVisibility(1f, 0.85f);
                colorElements[i].OnMouseDown += (__, _) => ColorSelected(index);
                colorElements[i].OnMouseOver += (__, _) => elementHovered = true;
                colorElements[i].OnMouseOut += (__, _) => elementHovered = false;
                Append(colorElements[i]);
                Append(noPaintOverlay[i]);
            }

            radius = 95;
            angle = Math.PI / 6;

            for (int i = 24; i < 30; i++)
            {
                int index = i;
                Vector2 offset = new Vector2(width - colorElementSize, height - colorElementSize) / 2;
                double x = offset.X - (radius * Math.Cos(angle * (i + .50)) * 1.10);
                double y = offset.Y + (radius * Math.Sin(-angle * (i + .50)) * 1.25);
                var left = new StyleDimension((float) x - colorElementSize / 4, 0);
                var top = new StyleDimension((float) y + 50, 0);
                colorElements[i].Left = noPaintOverlay[i].Left = left;
                colorElements[i].Top = noPaintOverlay[i].Top = top;
                colorElements[i].SetVisibility(1f, 0.85f);
                colorElements[i].OnMouseDown += (__, _) => ColorSelected(index);
                colorElements[i].OnMouseOver += (__, _) => elementHovered = true;
                colorElements[i].OnMouseOut += (__, _) => elementHovered = false;
                Append(colorElements[i]);
                Append(noPaintOverlay[i]);
            }

            for (int i = 0; i < 3; i++)
            {
                int index = i;
                double x = (width / 3) * i + 20f;
                double y = (width / 2) + 10f;

                toolElements[i].Left.Set((float) x - colorElementSize / 4, 0);
                toolElements[i].Top.Set((float) y, 0);
                toolElements[i].SetVisibility(1f, .8f);
                toolElements[i].OnMouseDown += (__, _) => ToolSelected(index);
                toolElements[i].OnMouseOver += (__, _) => elementHovered = true;
                toolElements[i].OnMouseOut += (__, _) => elementHovered = false;
                Append(toolElements[i]);
            }

            Append(colorOverlay);
            colorOverlay.Hide();

            toolOverlay.Left = toolElements[toolIndex].Left;
            toolOverlay.Top = toolElements[toolIndex].Top;
            Append(toolOverlay);
        }

        private void ColorSelected(int index)
        {
            if (!colorAvailable[index]) return;

            for (int i = 0; i < colorElements.Length; i++)
                colorElements[i].SetVisibility(1f, 0.85f);
            if (colorOverlay.Visible) colorOverlay.Hide();

            if (colorIndex != index)
            {
                colorElements[index].SetVisibility(1f, 1f);
                colorOverlay.Left = colorElements[index].Left;
                colorOverlay.Top = colorElements[index].Top;
                colorOverlay.Show();
                colorIndex = index;
            }
            else colorIndex = -1;
        }

        private void ToolSelected(int index)
        {
            for (int i = 0; i < toolElements.Length; i++)
                toolElements[i].SetVisibility(1f, .85f);

            toolElements[index].SetVisibility(1f, 1f);
            toolOverlay.Left = toolElements[index].Left;
            toolOverlay.Top = toolElements[index].Top;
            toolIndex = index;
        }

        private void UpdateCrossesOnColors()
        {
            EvaluateAvailableColorsInInventory();

            //TODO: CLEAN CODE BELOW
            for (int i = 0; i < noPaintOverlay.Length; i++)
            {
                if (!colorAvailable[i])
                {
                    Append(noPaintOverlay[i]);
                    if (colorIndex == i)
                    {
                        colorElements[colorIndex].SetVisibility(1f, 0.85f);
                        if (colorOverlay.Visible) colorOverlay.Hide();
                        colorIndex = -1;
                    }
                }
                else noPaintOverlay[i].Hide();
            }
        }

        private void EvaluateAvailableColorsInInventory()
        {
            List<int> paintInInventory = new List<int>();
            foreach (Item item in Main.LocalPlayer.inventory)
            {
                if (paints.Contains(item.type))
                    paintInInventory.Add(item.type);
            }

            paintInInventory.Sort();
            paintInInventory.TrimExcess();
            int[] foundIndexes = new int[paintInInventory.Count];

            //Convering paint item types to actual indexes
            for (int i = 0; i < paintInInventory.Count; i++)
                foundIndexes[i] = PaintItemTypeToIndex(paintInInventory[i]);

            //Reset all colors available
            for (int i = 0; i < colorAvailable.Length; i++)
                colorAvailable[i] = false;

            //Update colors available
            foreach (int index in foundIndexes)
                colorAvailable[index] = true;

            //The outputed indexes are not the paint color byte values. For those just increment one.
            int PaintItemTypeToIndex(int paintType)
            {
                //[TAG 1.4] Implements new paints and changes item types

                if (paintType >= 1073 && paintType <= 1099)
                    return paintType - 1073;
                else if (paintType >= 1966 && paintType <= 1968)
                    return paintType - 1939;
                else
                    return -1; //it will never reach here
            }
        }

        public void UpdateColors()
        {
            if (Visible)
                UpdateCrossesOnColors();
        }

        public void Update()
        {
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = false;

            if (!Visible) return;
            if (elementHovered)
                Main.LocalPlayer.mouseInterface = true;
        }
    }
}
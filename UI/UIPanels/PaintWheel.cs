using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Items;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using BuilderEssentials.UI.Elements;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace BuilderEssentials.UI.UIPanels
{
    internal class PaintWheel : CustomUIPanel
    {
        private const float width = 430f, height = 340f;
        private UIImageButton[] colorElements;
        private UIImageButton[] toolElements;
        private CustomUIImage[] noPaintOverlay;
        private CustomUIImage colorOverlay;
        private CustomUIImage toolOverlay;
        private Asset<Texture2D>[] toolTextures;
        private bool[] colorAvailable;
        private bool elementHovered;
        private int[] paints;
        public int colorIndex = -1;
        public int toolIndex;

        public PaintWheel()
        {
            paints = new int[31];
            for (int i = 0; i < 27; i++) paints[i] = (1073 + i); //Basic && Deep colors type
            for (int i = 0; i < 3; i++) paints[i + 27] = (1966 + i); //Extra Color Effects type
            paints[30] = 4668; //Illuminant Paint

            Width.Set(width, 0);
            Height.Set(height, 0);
            Left.Set(Main.screenWidth / 2 - width, 0);
            Top.Set(Main.screenHeight / 2 - height, 0);
            BorderColor = Color.Transparent;
            BackgroundColor = Color.Transparent;

            string texturePath = "BuilderEssentials/Textures/UIElements/Paint/";

            colorElements = new UIImageButton[31];
            noPaintOverlay = new CustomUIImage[31];
            colorAvailable = new bool[31];
            for (int i = 0; i < paints.Length; i++)
            {
                colorElements[i] = new UIImageButton(HelperMethods.RequestTexture(texturePath + "Paint" + i));
                noPaintOverlay[i] = new CustomUIImage(HelperMethods.RequestTexture(texturePath + "NoPaint"));
            }

            toolTextures = new Asset<Texture2D>[6];
            for (int i = 0; i < toolTextures.Length; i++)
                toolTextures[i] = HelperMethods.RequestTexture(texturePath + "Tool" + i);

            toolElements = new UIImageButton[3];
            for (int i = 0; i < toolElements.Length; i++)
                toolElements[i] = new UIImageButton(toolTextures[i]);

            colorOverlay = new CustomUIImage(HelperMethods.RequestTexture(texturePath + "PaintSelected"));
            colorOverlay.OnMouseOver += (__, _) => elementHovered = true;
            colorOverlay.OnMouseOut += (__, _) => elementHovered = false;
            toolOverlay = new CustomUIImage(HelperMethods.RequestTexture(texturePath + "ToolSelected"));
            toolOverlay.OnMouseOver += (__, _) => elementHovered = true;
            toolOverlay.OnMouseOut += (__, _) => elementHovered = false;

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
                noPaintOverlay[i].OnMouseOver += (__, _) => elementHovered = true;
                noPaintOverlay[i].OnMouseOut += (__, _) => elementHovered = false;


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
                noPaintOverlay[i].OnMouseOver += (__, _) => elementHovered = true;
                noPaintOverlay[i].OnMouseOut += (__, _) => elementHovered = false;

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
                noPaintOverlay[i].OnMouseOver += (__, _) => elementHovered = true;
                noPaintOverlay[i].OnMouseOut += (__, _) => elementHovered = false;

                Append(colorElements[i]);
                Append(noPaintOverlay[i]);
            }
            
            //Illuminant paint
            Vector2 offsetPaint = new Vector2(width - colorElementSize - 20, height - colorElementSize - 20) / 2;
            var leftOffset = new StyleDimension(offsetPaint.X, 0);
            var topOffset = new StyleDimension(offsetPaint.Y, 0);
            colorElements[30].Left = noPaintOverlay[30].Left = leftOffset;
            colorElements[30].Top = noPaintOverlay[30].Top = topOffset;
            
            colorElements[30].SetVisibility(1f, 0.85f);
            colorElements[30].OnMouseDown += (__, _) => ColorSelected(30);
            colorElements[30].OnMouseOver += (__, _) => elementHovered = true;
            colorElements[30].OnMouseOut += (__, _) => elementHovered = false;
            noPaintOverlay[30].OnMouseOver += (__, _) => elementHovered = true;
            noPaintOverlay[30].OnMouseOut += (__, _) => elementHovered = false;
            
            Append(colorElements[30]);
            Append(noPaintOverlay[30]);

            for (int i = 0; i < toolElements.Length; i++)
            {
                int index = i;
                double x = (width / 3) * i + 35f;
                double y = (width / 2) + 15f;

                toolElements[i].Left.Set((float) x - colorElementSize / 4, 0);
                toolElements[i].Top.Set((float) y, 0);

                toolElements[i].SetVisibility(1f, .8f);
                toolElements[i].OnMouseDown += (__, _) => ToolSelected(index);
                toolElements[i].OnMouseOver += (__, _) => elementHovered = true;
                toolElements[i].OnMouseOut += (__, _) => elementHovered = false;
                noPaintOverlay[i].OnMouseOver += (__, _) => elementHovered = true;
                noPaintOverlay[i].OnMouseOut += (__, _) => elementHovered = false;

                Append(toolElements[i]);
            }

            Append(colorOverlay);
            colorOverlay.Hide();

            toolOverlay.Left = toolElements[toolIndex].Left;
            toolOverlay.Top = toolElements[toolIndex].Top;
            toolElements[toolIndex].SetVisibility(1f, 1f);
            Append(toolOverlay);
        }

        private void ColorSelected(int index)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            if (!colorAvailable[index] && !mp.infinitePaintBucketEquipped) return;

            for (int i = 0; i < colorElements.Length; i++)
                colorElements[i].SetVisibility(1f, 0.85f);
            colorOverlay.Hide();

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
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            EvaluateAvailableColorsInInventory();

            for (int i = 0; i < noPaintOverlay.Length; i++)
            {
                if (!colorAvailable[i] && !mp.infinitePaintBucketEquipped)
                {
                    noPaintOverlay[i].Show();
                    if (colorIndex == i)
                    {
                        colorElements[colorIndex].SetVisibility(1f, 0.85f);
                        colorOverlay.Hide();
                        colorIndex = -1;
                    }
                }
                else if (colorAvailable[i] || mp.infinitePaintBucketEquipped)
                {
                    colorAvailable[i] = true;
                    noPaintOverlay[i].Hide();
                }
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
            //paintInInventory?.Distinct();
            int[] foundIndexes = new int[paintInInventory.Count];

            //Convering paint item types to actual indexes
            for (int i = 0; i < paintInInventory.Count; i++)
                foundIndexes[i] = HelperMethods.PaintItemTypeToColorIndex(paintInInventory[i]);

            //Reset all colors available
            for (int i = 0; i < colorAvailable.Length; i++)
                colorAvailable[i] = false;

            //Update colors available
            foreach (int index in foundIndexes)
                colorAvailable[index] = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            bool isSpectre = Main.LocalPlayer.HeldItem.type == ModContent.ItemType<SpectrePaintTool>();
            for (int i = 0; i < toolElements.Length; i++)
                toolElements[i].SetImage(toolTextures[isSpectre ? i + toolElements.Length : i]);
            
            UpdateCrossesOnColors();
            
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
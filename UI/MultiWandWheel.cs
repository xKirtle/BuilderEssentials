using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace BuilderEssentials.UI
{
    class MultiWandWheel : UIPanel
    {
        public static UIPanel MultiWandWheelPanel;
        private static float MultiWandWheelWidth;
        private static float MultiWandWheelHeight;
        public static List<UIImageButton> WandWheelElements;
        public static int selectedIndex = 0;
        public static bool IsWandsUIVisible;
        public static bool WandsWheelUIOpen;
        public static bool Hovering = MultiWandWheelPanel != null && MultiWandWheelPanel.IsMouseHovering && IsWandsUIVisible;

        public static void CreateMultiWandWheelPanel(int mouseX, int mouseY)
        {
            MultiWandWheelWidth = 250f;
            MultiWandWheelHeight = 250f;

            MultiWandWheelPanel = new UIPanel();
            MultiWandWheelPanel.Width.Set(MultiWandWheelWidth, 0);
            MultiWandWheelPanel.Height.Set(MultiWandWheelHeight, 0);
            MultiWandWheelPanel.Left.Set(mouseX - MultiWandWheelWidth / 2, 0);
            MultiWandWheelPanel.Top.Set(mouseY - MultiWandWheelHeight / 2, 0);
            MultiWandWheelPanel.BorderColor = Color.Transparent; //Color.Transparent;
            MultiWandWheelPanel.BackgroundColor = Color.Transparent;

            CreateLayout();
            ItemsWheel.Instance.Append(MultiWandWheelPanel);
        }

        private static void CreateLayout()
        {
            //Initialize the list that contains the WandWheel Elements, that are also intialized below
            WandWheelElements = new List<UIImageButton>(BuilderEssentials.WandWheelElements.Count);
            for (int i = 0; i < BuilderEssentials.WandWheelElements.Count; i++)
                WandWheelElements.Add(new UIImageButton(BuilderEssentials.WandWheelElements[i]));

            //Define them in a circle
            double radius = 60;
            double angle = Math.PI / 3; //6 Buttons
            for (int i = 0; i < WandWheelElements.Count; i++)
            {
                int index = i; //Magic values to keep the rotation aligned the way I want, sorry
                double x = (MultiWandWheelWidth / 2 - 35f) + (radius * Math.Cos(angle * (i + 3)));
                double y = (MultiWandWheelHeight / 2 - 35f) - (radius * Math.Sin(angle * (i + 3)));
                WandWheelElements[i].Left.Set((float)x, 0f);
                WandWheelElements[i].Top.Set((float)y, 0f);
                WandWheelElements[i].SetVisibility(.75f, .4f);
                WandWheelElements[i].OnClick += (__, _) => MainElementClick(index);
            }

            //Correct display of previously toggled settings
            WandWheelElements[selectedIndex].SetVisibility(1f, 1f);

            //Append them to the Main Panel
            for (int i = 0; i < WandWheelElements.Count; i++)
                MultiWandWheelPanel.Append(WandWheelElements[i]);
        }

        private static void MainElementClick(int index)
        {
            for (int i = 0; i < WandWheelElements.Count; i++)
                WandWheelElements[i].SetVisibility(.75f, .4f);

            WandWheelElements[index].SetVisibility(1f, 1f);
            selectedIndex = index;
        }

        private static string text;
        private static UIText hoverText;
        public static void HoverTextUpdate()
        {
            hoverText?.Remove();
            if (MultiWandWheelPanel != null && IsWandsUIVisible)
            {
                List<UIImageButton> hoveredElements = WandWheelElements.FindAll(x => x.IsMouseHovering == true);

                var elementsList = WandWheelElements;
                foreach (var element in hoveredElements)
                {
                    if (element == elementsList[0])
                        text = "Places living wood (wood)";
                    if (element == elementsList[1])
                        text = "Places bones (bone)";
                    if (element == elementsList[2])
                        text = "Places leaves (wood)";
                    if (element == elementsList[3])
                        text = "Places Hives (hive)";
                    if (element == elementsList[4])
                        text = "Places living rich mahogany (rich mahogany)";
                    if (element == elementsList[5])
                        text = "Places rich mahogany leaves (rich mahogany)";

                    hoverText = Tools.CreateUIText(text, Main.mouseX + 33, Main.mouseY + 33);
                    ItemsWheel.Instance.Append(hoverText);
                }
            }
        }

        public static void RemovePanel()
        {
            if (MultiWandWheelPanel != null)
            {
                MultiWandWheelPanel.Remove();
                IsWandsUIVisible = false;
                WandsWheelUIOpen = false;
            }
        }
    }
}

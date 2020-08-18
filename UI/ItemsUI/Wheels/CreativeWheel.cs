using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using static BuilderEssentials.Utilities.Tools;

namespace BuilderEssentials.UI.ItemsUI.Wheels
{
    public class CreativeWheel
    {
        static BuilderPlayer modPlayer;
        public static UIPanel CreativeWheelPanel;
        public static float CreativeWheelReworkWidth;
        public static float CreativeWheelReworkHeight;
        public static List<UIImageButton> CreativeWheelElements;
        private static List<UIImageButton> CreativeWheelHammerElements;
        private static readonly int numberOfElements = 5;
        public static int autoHammerSelectedIndex = 5; //full tile
        public static bool IsCreativeWheelVisible;
        public static bool CreativeWheelUIOpen;
        public static bool Hovering = CreativeWheelPanel != null && CreativeWheelPanel.IsMouseHovering && IsCreativeWheelVisible;

        public static void CreateCreativeWheelReworkPanel(int mouseX, int mouseY)
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            CreativeWheelReworkWidth = 250f;
            CreativeWheelReworkHeight = 250f;

            CreativeWheelPanel = new UIPanel();
            CreativeWheelPanel.Width.Set(CreativeWheelReworkWidth, 0);
            CreativeWheelPanel.Height.Set(CreativeWheelReworkHeight, 0);
            CreativeWheelPanel.Left.Set(mouseX - CreativeWheelReworkWidth / 2, 0); //mouseX - this.width/2
            CreativeWheelPanel.Top.Set(mouseY - CreativeWheelReworkHeight / 2, 0); //mouseY - this.height/2
            CreativeWheelPanel.BorderColor = Color.Transparent; //Color.Transparent;
            CreativeWheelPanel.BackgroundColor = Color.Transparent;

            CreateLayout();
            ItemsWheelState.Instance.Append(CreativeWheelPanel);
        }

        private static void CreateLayout()
        {
            //Initialize the list that contains the CreativeWheel Elements, that are also intialized below
            CreativeWheelElements = new List<UIImageButton>(BuilderEssentials.CreativeWheelElements.Count);
            for (int i = 0; i < BuilderEssentials.CreativeWheelElements.Count; i++)
                CreativeWheelElements.Add(new UIImageButton(BuilderEssentials.CreativeWheelElements[i]));

            //Define them in a circle
            double radius = 60;
            double angle = 2 * Math.PI / CreativeWheelElements.Count; //5 Buttons
            for (int i = 0; i < CreativeWheelElements.Count; i++)
            {
                int index = i; //Magic values to keep the rotation aligned the way I want, sorry
                double x = (CreativeWheelReworkWidth / 2 - 35f) + (radius * Math.Cos(angle * (i + 3) - 0.3));
                double y = (CreativeWheelReworkHeight / 2 - 35f) - (radius * Math.Sin(angle * (i + 3) - 0.3));
                CreativeWheelElements[i].Left.Set((float)x, 0f);
                CreativeWheelElements[i].Top.Set((float)y, 0f);
                CreativeWheelElements[i].SetVisibility(.75f, .4f);
                CreativeWheelElements[i].OnClick += (__, _) => MainElementClick(index);
            }

            //Correct display of previously toggled settings
            foreach (int selectedItem in modPlayer.creativeWheelSelectedIndex)
            {
                //Required because only the first 5 indexes correspond to actual CW Elements
                if (selectedItem < numberOfElements)
                {
                    CreativeWheelElements[selectedItem].SetVisibility(1f, 1f);
                    if (selectedItem == 2) //AutoHammer
                        CreateHammerLayout(autoHammerSelectedIndex);
                }
            }

            //Append them to the Main Panel
            for (int i = 0; i < CreativeWheelElements.Count; i++)
                CreativeWheelPanel.Append(CreativeWheelElements[i]);
        }

        private static void MainElementClick(int index)
        {
            if (!modPlayer.creativeWheelSelectedIndex.Contains(index))
                modPlayer.creativeWheelSelectedIndex.Add(index);
            else
                modPlayer.creativeWheelSelectedIndex.Remove(index);
            ResetVisibility();

            //Unnecessary Switch?
            switch (index)
            {
                case 0: //Item Picker
                    break;
                case 1: //InfinitePlacement
                    break;
                case 2: //AutoHammer
                    if (modPlayer.creativeWheelSelectedIndex.Contains(CreativeWheelItem.AutoHammer.ToInt()))
                        CreateHammerLayout(autoHammerSelectedIndex);
                    else
                        RemoveHammerLayout();
                    break;
                case 3: //PlacementAnywhere
                    break;
                case 4: //InfinitePickupRange
                    break;
            }

            void ResetVisibility()
            {
                for (int i = 0; i < CreativeWheelElements.Count; i++)
                    CreativeWheelElements[i].SetVisibility(.75f, .4f);

                foreach (int activeElement in modPlayer.creativeWheelSelectedIndex)
                {
                    //Only indexes smaller than 5 correspond to actual CW Elements
                    if (activeElement < numberOfElements)
                        CreativeWheelElements[activeElement].SetVisibility(1f, 1f);
                }
            }
        }

        private static string text;
        private static UIText hoverText;
        public static void HoverTextUpdate()
        {
            hoverText?.Remove();
            if (CreativeWheelPanel != null && IsCreativeWheelVisible)
            {
                List<UIImageButton> hoveredElements = CreativeWheelElements.FindAll(x => x.IsMouseHovering == true);

                //Don't need the actual list itself, just a reference of it
                var elementsList = CreativeWheel.CreativeWheelElements;
                foreach (var element in hoveredElements)
                {
                    if (element == elementsList[0])
                        text = "Middle Click to grab a block to your inventory";
                    if (element == elementsList[1])
                        text = "Allows infinite placement of any block/wall";
                    if (element == elementsList[2])
                        text = "Select a slope and Left Mouse Click with an empty hand";
                    if (element == elementsList[3])
                        text = "Allows block placement in the middle of the air";
                    if (element == elementsList[4])
                        text = "Gives infinite pick up range";

                    hoverText = Tools.CreateUIText(text, Main.mouseX + 22, Main.mouseY + 22);
                    ItemsWheelState.Instance.Append(hoverText);
                }
            }
        }

        private static void CreateHammerLayout(int selectedIndex)
        {
            CreativeWheelHammerElements = new List<UIImageButton>(BuilderEssentials.CWAutoHammerElements.Count);
            for (int i = 0; i < BuilderEssentials.CWAutoHammerElements.Count; i++)
                CreativeWheelHammerElements.Add(new UIImageButton(BuilderEssentials.CWAutoHammerElements[i]));

            double radius = 36;
            double angle = Math.PI / (CreativeWheelHammerElements.Count / 2); //6 Elements
            for (int i = 0; i < CreativeWheelHammerElements.Count; i++)
            {
                int index = i;
                //We add 11 to both x and y axis since that's half of the width/height on the small icons around the AutoHammer
                double x = (CreativeWheelElements[2].Left.Pixels + 11) + (radius * Math.Cos(angle * (i + 3)));
                double y = (CreativeWheelElements[2].Top.Pixels + 11) - (radius * Math.Sin(angle * (i + 3)));
                CreativeWheelHammerElements[i].Left.Set((float)x, 0f);
                CreativeWheelHammerElements[i].Top.Set((float)y, 0f);
                CreativeWheelHammerElements[i].SetVisibility(.75f, .4f);
                CreativeWheelHammerElements[i].OnClick += (__, _) => AutoHammerElementClick(index);
            }

            CreativeWheelHammerElements[selectedIndex].SetVisibility(1f, 1f);

            for (int i = 0; i < CreativeWheelHammerElements.Count; i++)
                CreativeWheelPanel.Append(CreativeWheelHammerElements[i]);
        }

        private static void RemoveHammerLayout()
        {
            if (CreativeWheelHammerElements != null)
                for (int i = 0; i < CreativeWheelHammerElements.Count; i++)
                    CreativeWheelHammerElements[i].Remove();
        }
        private static void AutoHammerElementClick(int index)
        {
            autoHammerSelectedIndex = index;
            ResetVisibility();
            CreativeWheelHammerElements[index].SetVisibility(1f, 1f);

            void ResetVisibility()
            {
                for (int i = 0; i < CreativeWheelHammerElements.Count; i++)
                    CreativeWheelHammerElements[i].SetVisibility(.75f, .4f);
            }
        }

        public static void RemovePanel()
        {
            if (CreativeWheelPanel != null)
            {
                CreativeWheelPanel.Remove();
                IsCreativeWheelVisible = false;
                CreativeWheelUIOpen = false;
            }
        }
    }
}
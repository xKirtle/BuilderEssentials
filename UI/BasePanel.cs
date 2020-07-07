using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BuilderEssentials.Utilities;
using System.Collections.Generic;

namespace BuilderEssentials.UI
{
    public class BasePanel : UIState
    {
        public static UIImageButton buildingModeButton;
        public static Texture2D buttonTexture;
        public static UIPanel creativeWheelPanel;
        public static bool isBuildingModeButtonVisible;
        public static bool isCreativeWheelVisible;
        public static bool creativeWheelUIOpen;
        static UIText hoverText;
        public override void OnInitialize()
        {
            buttonTexture = BuilderEssentials.BuildingModeOff;
            buildingModeButton = new UIImageButton(buttonTexture);
            buildingModeButton.VAlign = 0f;
            buildingModeButton.HAlign = 0f;
            buildingModeButton.Top.Set(40f, 0);
            buildingModeButton.Left.Set(510f, 0);
            buildingModeButton.OnClick += ChangeAccessories_OnClick;
            buildingModeButton.SetVisibility(0f, 0f);
            Append(buildingModeButton);
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.playerInventory)
            {
                if (!isBuildingModeButtonVisible)
                {
                    buildingModeButton.SetVisibility(1f, .4f);
                    isBuildingModeButtonVisible = true;
                }

                if (isCreativeWheelVisible)
                {
                    creativeWheelPanel.Remove();
                    creativeWheelUIOpen = false;
                    isCreativeWheelVisible = false;
                }

                if (PaintWheel.IsPaintingUIVisible)
                {
                    PaintWheel.PaintWheelPanel.Remove();
                    PaintWheel.PaintingUIOpen = false;
                    PaintWheel.IsPaintingUIVisible = false;
                }

                if (MultiWandWheel.IsWandsUIVisible)
                {
                    MultiWandWheel.MultiWandWheelPanel.Remove();
                    MultiWandWheel.WandsWheelUIOpen = false;
                    MultiWandWheel.IsWandsUIVisible = false;
                }

                if (AutoHammerWheel.IsAutoHammerUIVisible)
                {
                    AutoHammerWheel.AutoHammerWheelPanel.Remove();
                    AutoHammerWheel.AutoHammerUIOpen = false;
                    AutoHammerWheel.IsAutoHammerUIVisible = false;
                }
            }
            else //!Main.playerInventory
            {
                if (isBuildingModeButtonVisible)
                {
                    buildingModeButton.SetVisibility(0f, 0f);
                    isBuildingModeButtonVisible = false;
                }
            }

            //Blocks mouse from interacting with the world when hovering on UI interfaces
            if ((buildingModeButton.IsMouseHovering && isBuildingModeButtonVisible) ||
            (creativeWheelPanel != null && creativeWheelPanel.IsMouseHovering && isCreativeWheelVisible) ||
            (PaintWheel.Hovering || MultiWandWheel.Hovering || AutoHammerWheel.Hovering))
                Main.LocalPlayer.mouseInterface = true;

            //CreativeWrench Wheel UI
            if (creativeWheelUIOpen && !isCreativeWheelVisible)
            {
                //creativeWheelPanel = CreativeWheel.CreateCreativeWheelPanel(Main.mouseX, Main.mouseY, this);
                creativeWheelPanel = CreativeWheelRework.CreateCreativeWheelReworkPanel(Main.mouseX, Main.mouseY, this);
                isCreativeWheelVisible = true;
            }
            else if (!creativeWheelUIOpen && isCreativeWheelVisible)
            {
                creativeWheelPanel.Remove();
                isCreativeWheelVisible = false;
            }

            //SuperPaintingTool Paint UI
            if (PaintWheel.PaintingUIOpen && !PaintWheel.IsPaintingUIVisible)
            {
                PaintWheel.CreatePaintWheel(Main.mouseX, Main.mouseY, this); //Method to create it
                PaintWheel.IsPaintingUIVisible = true;
            }
            else if (!PaintWheel.PaintingUIOpen && PaintWheel.IsPaintingUIVisible)
            {
                PaintWheel.PaintWheelPanel.Remove();
                PaintWheel.IsPaintingUIVisible = false;
            }

            //Wand Wheel UI
            if (MultiWandWheel.WandsWheelUIOpen && !MultiWandWheel.IsWandsUIVisible)
            {
                MultiWandWheel.CreateMultiWandWheelPanel(Main.mouseX, Main.mouseY, this);
                MultiWandWheel.IsWandsUIVisible = true;
            }
            else if (!MultiWandWheel.WandsWheelUIOpen && MultiWandWheel.IsWandsUIVisible)
            {
                MultiWandWheel.MultiWandWheelPanel.Remove();
                MultiWandWheel.IsWandsUIVisible = false;
            }

            //AutoHammer Wheel UI
            if (AutoHammerWheel.AutoHammerUIOpen && !AutoHammerWheel.IsAutoHammerUIVisible)
            {
                AutoHammerWheel.CreateAutoHammerWheelPanel(Main.mouseX, Main.mouseY, this);
                AutoHammerWheel.IsAutoHammerUIVisible = true;
            }
            else if (!AutoHammerWheel.AutoHammerUIOpen && AutoHammerWheel.IsAutoHammerUIVisible)
            {
                AutoHammerWheel.AutoHammerWheelPanel.Remove();
                AutoHammerWheel.IsAutoHammerUIVisible = false;
            }

            //Tooltips while hovering CWElements
            if (CreativeWheelRework.CreativeWheelReworkPanel != null)
            {
                List<UIImageButton> hoveredElements = CreativeWheelRework.CreativeWheelElements.FindAll(x => x.IsMouseHovering == true);

                if (hoverText != null)
                {
                    hoverText.Remove();
                    hoverText = null;
                }

                //Don't need the actual list itself, just a reference of it
                var elementsList = CreativeWheelRework.CreativeWheelElements;
                string text = "";
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

                    hoverText = new UIText(text, 1, false);
                    hoverText.VAlign = 0f;
                    hoverText.HAlign = 0f;
                    hoverText.Left.Set(Main.mouseX + 22, 0);
                    hoverText.Top.Set(Main.mouseY + 22, 0);
                    Append(hoverText);
                }
            }
        }

        public void ChangeAccessories_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (isBuildingModeButtonVisible)
                Tools.BuildingModeToggle();
        }
    }
}

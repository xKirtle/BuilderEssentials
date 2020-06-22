using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BuilderEssentials.Utilities;
using static BuilderEssentials.BuilderPlayer;
using System.Collections.Generic;

namespace BuilderEssentials.UI
{
    public class BasePanel : UIState
    {
        public static UIImageButton buildingModeButton;
        public static Texture2D buttonTexture;
        public static UIPanel creativeWheelPanel;
        public static UIPanel paintingPanel;
        public static bool isBuildingModeButtonVisible;
        public static bool isCreativeWheelVisible;
        public static bool creativeWheelUIOpen;
        public static bool isPaintingUIVisible;
        public static bool paintingUIOpen;


        //Temporary, this is stupid
        static UIText hoverText0;
        static UIText hoverText1;
        static UIText hoverText2;
        static UIText hoverText3;
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

                if (isPaintingUIVisible)
                {
                    paintingPanel.Remove();
                    paintingUIOpen = false;
                    isPaintingUIVisible = false;
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
            (creativeWheelPanel != null && creativeWheelPanel.IsMouseHovering && isCreativeWheelVisible))
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
            if (paintingUIOpen && !isPaintingUIVisible)
            {
                paintingPanel = PaintWheel.CreatePaintWheel(Main.mouseX, Main.mouseY, this); //Method to create it
                isPaintingUIVisible = true;
            }
            else if (!paintingUIOpen && isPaintingUIVisible)
            {
                paintingPanel.Remove();
                isPaintingUIVisible = false;
            }


            //Maybe make a whole class for the CreativeWheelElements so that I have access to the Update method?
            //TODO: CLEAN THIS CODE AND MAKE IT PROPERLY
            if (CreativeWheelRework.CreativeWheelReworkPanel != null)
            {
                if (CreativeWheelRework.CreativeWheelElements[0].IsMouseHovering)
                {
                    //ItemPicker
                    if (hoverText0 == null)
                    {
                        hoverText0 = new UIText("Middle Click to grab a block to your inventory", 1, false);
                        hoverText0.VAlign = 0f;
                        hoverText0.HAlign = 0f;
                        hoverText0.Left.Set(Main.mouseX + 22, 0);
                        hoverText0.Top.Set(Main.mouseY + 22, 0);
                        Append(hoverText0);
                    }
                }
                else
                {
                    if (hoverText0 != null)
                    {
                        hoverText0.Remove();
                        hoverText0 = null;
                    }
                }

                if (CreativeWheelRework.CreativeWheelElements[1].IsMouseHovering)
                {
                    //Infinite Placement
                    if (hoverText1 == null)
                    {
                        hoverText1 = new UIText("Allows infinite placements of any block/wall", 1, false);
                        hoverText1.VAlign = 0f;
                        hoverText1.HAlign = 0f;
                        hoverText1.Left.Set(Main.mouseX + 22, 0);
                        hoverText1.Top.Set(Main.mouseY + 22, 0);
                        Append(hoverText1);
                    }
                }
                else
                {
                    if (hoverText1 != null)
                    {
                        hoverText1.Remove();
                        hoverText1 = null;
                    }
                }

                if (CreativeWheelRework.CreativeWheelElements[2].IsMouseHovering)
                {
                    //Infinite Placement
                    if (hoverText2 == null)
                    {
                        hoverText2 = new UIText("Select a slope and Left Mouse Click with an empty hand", 1, false);
                        hoverText2.VAlign = 0f;
                        hoverText2.HAlign = 0f;
                        hoverText2.Left.Set(Main.mouseX + 22, 0);
                        hoverText2.Top.Set(Main.mouseY + 22, 0);
                        Append(hoverText2);
                    }
                }
                else
                {
                    if (hoverText2 != null)
                    {
                        hoverText2.Remove();
                        hoverText2 = null;
                    }
                }

                if (CreativeWheelRework.CreativeWheelElements[3].IsMouseHovering)
                {
                    //Infinite Placement
                    if (hoverText3 == null)
                    {
                        hoverText3 = new UIText("Allows block placements in the middle of the air", 1, false);
                        hoverText3.VAlign = 0f;
                        hoverText3.HAlign = 0f;
                        hoverText3.Left.Set(Main.mouseX + 22, 0);
                        hoverText3.Top.Set(Main.mouseY + 22, 0);
                        Append(hoverText3);
                    }
                }
                else
                {
                    if (hoverText3 != null)
                    {
                        hoverText3.Remove();
                        hoverText3 = null;
                    }
                }
            }
        }

        public void ChangeAccessories_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (isBuildingModeButtonVisible)
                BuildingMode.BuildingModeAccessoriesToggle();
        }
    }
}

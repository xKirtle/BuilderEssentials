using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using BuilderEssentials.Utilities;
using System.Collections.Generic;

namespace BuilderEssentials.UI
{
    public class BasePanel : UIState
    {
        public static UIText hoverText;
        static string text;

        public override void Update(GameTime gameTime)
        {
            //Blocks mouse from interacting with the world when hovering on UI interfaces
            if (BuildingModeState.Hovering || CreativeWheel.Hovering || PaintWheel.Hovering || MultiWandWheel.Hovering || AutoHammerWheel.Hovering)
                Main.LocalPlayer.mouseInterface = true;

            //CreativeWrench Wheel UI
            if (Tools.UIPanelLogic(CreativeWheel.CreativeWheelPanel, ref CreativeWheel.CreativeWheelUIOpen, ref CreativeWheel.IsCreativeWheelVisible))
                CreativeWheel.CreateCreativeWheelReworkPanel(Main.mouseX, Main.mouseY, this);

            //SuperPaintingTool Paint UI
            if (Tools.UIPanelLogic(PaintWheel.PaintWheelPanel, ref PaintWheel.PaintingUIOpen, ref PaintWheel.IsPaintingUIVisible))
                PaintWheel.CreatePaintWheel(Main.mouseX, Main.mouseY, this);

            //Wand Wheel UI
            if (Tools.UIPanelLogic(MultiWandWheel.MultiWandWheelPanel, ref MultiWandWheel.WandsWheelUIOpen, ref MultiWandWheel.IsWandsUIVisible))
                MultiWandWheel.CreateMultiWandWheelPanel(Main.mouseX, Main.mouseY, this);

            //AutoHammer Wheel UI
            if (Tools.UIPanelLogic(AutoHammerWheel.AutoHammerWheelPanel, ref AutoHammerWheel.AutoHammerUIOpen, ref AutoHammerWheel.IsAutoHammerUIVisible))
                AutoHammerWheel.CreateAutoHammerWheelPanel(Main.mouseX, Main.mouseY, this);

            //Tooltips while hovering CWElements
            if (CreativeWheel.CreativeWheelPanel != null && CreativeWheel.IsCreativeWheelVisible)
            {
                List<UIImageButton> hoveredElements = CreativeWheel.CreativeWheelElements.FindAll(x => x.IsMouseHovering == true);

                if (hoverText != null)
                {
                    hoverText.Remove();
                    hoverText = null;
                }

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
                    Append(hoverText);
                }
            }

            //Tooltips while hovering Multi Wand Elements
            if (MultiWandWheel.MultiWandWheelPanel != null && MultiWandWheel.IsWandsUIVisible)
            {
                List<UIImageButton> hoveredElements = MultiWandWheel.WandWheelElements.FindAll(x => x.IsMouseHovering == true);

                if (hoverText != null)
                {
                    hoverText.Remove();
                    hoverText = null;
                }

                var elementsList = MultiWandWheel.WandWheelElements;
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

                    hoverText = Tools.CreateUIText(text, Main.mouseX + 22, Main.mouseY + 22);
                    Append(hoverText);
                }
            }
        }
    }
}

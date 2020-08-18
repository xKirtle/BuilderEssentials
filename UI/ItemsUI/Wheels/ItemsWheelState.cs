using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using BuilderEssentials.Utilities;

namespace BuilderEssentials.UI.ItemsUI.Wheels
{
    public class ItemsWheelState : UIState
    {
        public static ItemsWheelState Instance;
        public ItemsWheelState() => Instance = this;

        public override void Update(GameTime gameTime)
        {
            //Blocks mouse from interacting with the world when hovering on UI interfaces
            if (BuildingModeState.Hovering || CreativeWheel.Hovering || PaintWheel.Hovering || MultiWandWheel.Hovering || AutoHammerWheel.Hovering)
                Main.LocalPlayer.mouseInterface = true;

            //CreativeWrench Wheel UI
            if (Tools.UIPanelLogic(CreativeWheel.CreativeWheelPanel, ref CreativeWheel.CreativeWheelUIOpen, ref CreativeWheel.IsCreativeWheelVisible))
                CreativeWheel.CreateCreativeWheelReworkPanel(Main.mouseX, Main.mouseY);

            //SuperPaintingTool Paint UI
            if (Tools.UIPanelLogic(PaintWheel.PaintWheelPanel, ref PaintWheel.PaintingUIOpen, ref PaintWheel.IsPaintingUIVisible))
                PaintWheel.CreatePaintWheel(Main.mouseX, Main.mouseY);

            //Wand Wheel UI
            if (Tools.UIPanelLogic(MultiWandWheel.MultiWandWheelPanel, ref MultiWandWheel.WandsWheelUIOpen, ref MultiWandWheel.IsWandsUIVisible))
                MultiWandWheel.CreateMultiWandWheelPanel(Main.mouseX, Main.mouseY);

            //AutoHammer Wheel UI
            if (Tools.UIPanelLogic(AutoHammerWheel.AutoHammerWheelPanel, ref AutoHammerWheel.AutoHammerUIOpen, ref AutoHammerWheel.IsAutoHammerUIVisible))
                AutoHammerWheel.CreateAutoHammerWheelPanel(Main.mouseX, Main.mouseY);

            //Updating CW hover text
            CreativeWheel.HoverTextUpdate();

            //Updating MultiWand hover text
            MultiWandWheel.HoverTextUpdate();
        }
    }
}

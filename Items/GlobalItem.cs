using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static BuilderEssentials.UI.UIStates.UIStateLogic1;
using static BuilderEssentials.UI.UIStates.UIStateLogic4;

namespace BuilderEssentials.Items
{
    public class GlobalItem : Terraria.ModLoader.GlobalItem
    {
        public override void UpdateInventory(Item item, Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            BEPlayer mp = player.GetModPlayer<BEPlayer>();

            //Just stop updating the whole Interface Layer?
            if (player.HeldItem.type != ItemType<MultiWand>() && multiWandWheel.Visible)
                multiWandWheel.Hide();

            if (player.HeldItem.type != ItemType<AutoHammer>() && autoHammerWheel.Visible)
                autoHammerWheel.Hide();

            if ((player.HeldItem.type != ItemType<SpectrePaintingTool>() &&
                 player.HeldItem.type != ItemType<PaintingTool>()) && paintWheel.Visible)
                paintWheel.Hide();

            if (player.HeldItem.type != ItemType<FillWand>() && fillWandSelection.Visible)
                fillWandSelection.Hide();

            if (player.HeldItem.type != ItemType<ShapesDrawer>())
            {
                arrowPanel.Hide();
                menuPanel.Hide();
            }
            else if (player.HeldItem.type == ItemType<ShapesDrawer>() && !arrowPanel.Visible && !menuPanel.Visible)
                arrowPanel.Show();

            // if (player.HeldItem.type == ItemType<ShapesDrawer>() && mirrorWandSelection.Visible)
            // {
            //     mirrorWandSelection.Hide();
            //     //ShapesDrawer.Show();
            // }
            //
            // if (player.HeldItem.type == ItemType<MirrorWand>()) /*&& shapesDrawerSelection.Visible)*/
            // {
            //     //shapesDrawerSelection.Hide();
            //     mirrorWandSelection.Show();
            // }
        }
    }
}
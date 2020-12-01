using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items
{
    public class GlobalItem : Terraria.ModLoader.GlobalItem
    {
        public override void UpdateInventory(Item item, Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            BEPlayer mp = player.GetModPlayer<BEPlayer>();

            //Just stop updating the whole Interface Layer?
            if (player.HeldItem.type != ItemType<MultiWand>() && UIStateLogic1.multiWandWheel.Visible)
                UIStateLogic1.multiWandWheel.Hide();

            if (player.HeldItem.type != ItemType<AutoHammer>() && UIStateLogic1.autoHammerWheel.Visible)
                UIStateLogic1.autoHammerWheel.Hide();

            if ((player.HeldItem.type != ItemType<SpectrePaintingTool>() &&
                 player.HeldItem.type != ItemType<PaintingTool>())
                && UIStateLogic1.paintWheel.Visible)
                UIStateLogic1.paintWheel.Hide();

            if (player.HeldItem.type != ItemType<FillWand>() && UIStateLogic1.fillWandSelection.Visible)
                UIStateLogic1.fillWandSelection.Hide();
        }
    }
}
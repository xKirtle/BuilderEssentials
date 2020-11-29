using BuilderEssentials.UI.UIStates;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items
{
    public class GlobalItem : Terraria.ModLoader.GlobalItem
    {
        public override void UpdateInventory(Item item, Player player)
        {
            //Just stop updating the whole Interface Layer?
            if (player.whoAmI == Main.myPlayer &&
                player.HeldItem.type != ItemType<MultiWand>() && UIStateLogic1.multiWandWheel.Visible)
                UIStateLogic1.multiWandWheel.Hide();

            if (player.whoAmI == Main.myPlayer &&
                player.HeldItem.type != ItemType<AutoHammer>() && UIStateLogic1.autoHammerWheel.Visible)
                UIStateLogic1.autoHammerWheel.Hide();

            if (player.whoAmI == Main.myPlayer &&
                (player.HeldItem.type != ItemType<SpectrePaintingTool>() && player.HeldItem.type != ItemType<PaintingTool>())
                && UIStateLogic1.paintWheel.Visible)
                UIStateLogic1.paintWheel.Hide();

            if (player.whoAmI == Main.myPlayer &&
                player.HeldItem.type != ItemType<FillWand>() && UIStateLogic1.fillWandSelection.Visible)
                UIStateLogic1.fillWandSelection.Hide();
        }
    }
}
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static BuilderEssentials.Utilities.HelperMethods;

namespace BuilderEssentials.Items.Accessories
{
    public class FastWrench : BaseWrench
    {
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.whoAmI != Main.myPlayer) return;
            UpdateUpgrades(player);
        }
    }
}
using System;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Common.GlobalItems;

public class InfiniteGlobalItem : GlobalItem
{
    public override bool ConsumeItem(Item item, Player player) {
        if (player.whoAmI != Main.myPlayer) return base.ConsumeItem(item, player);
        //Kirtle: Return false for infinite items! Works for tile wands as well

        if (player.GetModPlayer<BEPlayer>().InfinitePaint) {
            if (item.paint > 0)
                return false;
        }
        
        return base.ConsumeItem(item, player);
    }
}
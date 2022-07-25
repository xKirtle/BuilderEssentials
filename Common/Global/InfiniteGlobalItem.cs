using System;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Common.GlobalItems;

public class InfiniteGlobalItem : GlobalItem
{
    public override bool ConsumeItem(Item item, Player player) {
        if (player.whoAmI != Main.myPlayer)
            return base.ConsumeItem(item, player);
        BEPlayer mp = player.GetModPlayer<BEPlayer>();

        if (mp.InfinitePlacement)
            return false;

        if (mp.InfinitePaint && item.paint > 0)
            return false;

        return base.ConsumeItem(item, player);
    }
}
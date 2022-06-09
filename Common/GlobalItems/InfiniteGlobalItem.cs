using System;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Common.GlobalItems;

public class InfiniteGlobalItem : GlobalItem
{
    public override bool ConsumeItem(Item item, Player player) {
        //Kirtle: Return false for infinite items! Works for tile wands as well
        
        return base.ConsumeItem(item, player);
    }
}
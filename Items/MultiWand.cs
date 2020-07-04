using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class MultiWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Contains all wands");
        }

        public override void SetDefaults()
        {
            item.height = 40;
            item.width = 40;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = false;
        }
    }
}

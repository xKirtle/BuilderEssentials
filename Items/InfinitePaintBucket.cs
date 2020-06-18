using BuilderEssentials.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class InfinitePaintBucket : ModItem
    {
        public override void SetDefaults()
        {
            item.height = 20;
            item.width = 18;
            item.useTime = 1;
            item.useAnimation = 10;
            //item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.noMelee = true;
            item.noUseGraphic = true;
        }
    }
}

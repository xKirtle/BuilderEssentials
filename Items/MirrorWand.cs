using BuilderEssentials.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class MirrorWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mirrors everything!");
            Item.staff[item.type] = true;
        }
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

        public override bool AltFunctionUse(Player player) //Right click
        {
            int posX = Player.tileTargetX;
            int posY = Player.tileTargetY;
            Tile tile = Main.tile[posX, posY];

            //Draw Semi Transparent "Tiles"?

            return true;
        }


    }
}

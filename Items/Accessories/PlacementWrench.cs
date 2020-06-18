using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    class PlacementWrench : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows infinite range");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.blockRange += 55;
            player.wallSpeed += 10;
            player.tileSpeed += 50;
            Player.tileRangeX = 65;
            Player.tileRangeY = 55;
        }

        public override void AddRecipes()
        {
            //Not really worried about balancing at this point
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.ArchitectGizmoPack, 1);
            modRecipe.AddIngredient(ItemID.LaserRuler, 1);
            modRecipe.AddIngredient(ItemID.Toolbox, 1);
            modRecipe.AddIngredient(ItemID.Toolbelt, 1);
            modRecipe.AddTile(TileID.AdamantiteForge);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}

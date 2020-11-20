using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BuilderEssentials.UI.UIStates;

namespace BuilderEssentials.Items
{
    public class InfinitePaintBucket : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/InfinitePaintBucket";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Allows infinite paint in the modded Painting Tools");

        public override void SetDefaults()
        {
            item.height = 20;
            item.width = 18;
            item.useTime = 1;
            item.useAnimation = 10;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.noMelee = true;
            item.noUseGraphic = true;
        }
        
        //SmartCursorHelper TryFindingPaintInplayerInventory (returns first color byte of paint found)
        
        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;

            base.UpdateInventory(player);
            ItemsUIState.paintWheel.infPaintBucket = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DeepRedPaint, 999);
            recipe.AddIngredient(ItemID.DeepGreenPaint, 999);
            recipe.AddIngredient(ItemID.DeepBluePaint, 999);
            recipe.AddIngredient(ItemID.NegativePaint, 999);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
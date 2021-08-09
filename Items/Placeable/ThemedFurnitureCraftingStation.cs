using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    internal class ThemedFurnitureCraftingStation : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Placeable/ThemedFurnitureCraftingStation";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to craft Themed Furniture items");

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 25, 0, 0);;
            Item.createTile = TileType<Tiles.ThemedFurnitureCraftingStation>();
            Item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.BoneWelder)
            .AddIngredient(ItemID.GlassKiln)
            .AddIngredient(ItemID.HoneyDispenser)
            .AddIngredient(ItemID.IceMachine)
            .AddIngredient(ItemID.LivingLoom)
            .AddIngredient(ItemID.SkyMill)
            .AddIngredient(ItemID.Solidifier)
            .AddIngredient(ItemID.FleshCloningVaat)
            .AddIngredient(ItemID.SteampunkBoiler)
            .AddIngredient(ItemID.LihzahrdFurnace)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
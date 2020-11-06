using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.DataStructures;
using BuilderEssentials.Utilities;
using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIStates;

namespace BuilderEssentials.Items
{
    public class MultiWand : ModItem
    {
        private Point toolRange;
        private bool canPlaceItems;

        private int[] wandMaterials =
        {
            ItemID.Wood,
            ItemID.Bone,
            ItemID.Wood,
            ItemID.Hive,
            ItemID.RichMahogany,
            ItemID.RichMahogany
        };

        private int[] wandPlacedTiles =
        {
            TileID.LivingWood,
            TileID.BoneBlock,
            TileID.LeafBlock,
            TileID.Hive,
            TileID.LivingMahogany,
            TileID.LivingMahoganyLeaves
        };

        public override string Texture => "BuilderEssentials/Textures/Items/MultiWand";

        public override void SetStaticDefaults() =>
            Tooltip.SetDefault("Contains all building wands!\nRight Click to open selection menu");

        public override void SetDefaults()
        {
            item.height = 44;
            item.width = 44;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.autoReuse = true;
            item.noMelee = false;
            toolRange = new Point(8, 8);
        }

        public override Vector2? HoldoutOffset() => new Vector2(2, -9);

        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;

            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            if (Main.netMode != NetmodeID.Server && mp.ValidCursorPos)
            {
                canPlaceItems = HelperMethods.ToolHasRange(toolRange) &&
                                HelperMethods.IsUIAvailable(playerNotWieldingItem: false);
                player.showItemIcon = canPlaceItems && !ItemsUIState.multiWandWheel.IsMouseHovering;
                player.showItemIcon2 = item.type;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer || !canPlaceItems) return false;

            MultiWandWheel panel = ItemsUIState.multiWandWheel;
            int materialType = wandMaterials[panel.selectedIndex];
            int tileType = wandPlacedTiles[panel.selectedIndex];
            if (HelperMethods.ValidTilePlacement(Player.tileTargetX, Player.tileTargetY) &&
                HelperMethods.CanReduceItemStack(materialType, true))
                HelperMethods.PlaceTile(Player.tileTargetX, Player.tileTargetY, HelperMethods.ItemTypes.Tile, tileType);

            return true;
        }

        private int mouseRightTimer = 0;

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            
            if (Main.mouseRight && player.HeldItem == item &&
                HelperMethods.IsUIAvailable() && ++mouseRightTimer == 2)
                ItemsUIState.multiWandWheel.Toggle();

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LivingWoodWand);
            recipe.AddIngredient(ItemID.BoneWand);
            recipe.AddIngredient(ItemID.LeafWand);
            recipe.AddIngredient(ItemID.HiveWand);
            recipe.AddIngredient(ItemID.LivingMahoganyWand);
            recipe.AddIngredient(ItemID.LivingMahoganyLeafWand);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
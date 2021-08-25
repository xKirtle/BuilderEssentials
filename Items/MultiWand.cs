using System;
using System.Linq;
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
    internal class MultiWand : ModItem
    {
        private Vector2 toolRange;
        private bool canPlaceItems;
        private MultiWandWheel panel;

        internal static int[] wandTypes =
        {
            ItemID.LivingWoodWand, 
            ItemID.BoneWand, 
            ItemID.LeafWand, 
            ItemID.HiveWand, 
            ItemID.LivingMahoganyWand, 
            ItemID.LivingMahoganyLeafWand
        };
        
        internal static int[] wandMaterials =
        {
            ItemID.Wood,
            ItemID.Bone,
            ItemID.Wood,
            ItemID.Hive,
            ItemID.RichMahogany,
            ItemID.RichMahogany
        };

        internal static int[] wandPlacedTiles =
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
            Item.height = 44;
            Item.width = 44;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.noMelee = false;
            toolRange = new Vector2(9, 8);
        }

        public override Vector2? HoldoutOffset() => new Vector2(2, -9);

        private int mouseRightTimer = 0;
        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            panel = UIUIState.Instance.multiWandWheel;
            
            if (Main.mouseRight && player.HeldItem == Item &&
                (HelperMethods.IsUIAvailable() || panel.IsMouseHovering) && ++mouseRightTimer == 2)
                UIUIState.Instance.multiWandWheel.Toggle();

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;
            
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            if (mp.ValidCursorPos)
            {
                canPlaceItems = HelperMethods.ToolHasRange(toolRange) && !Main.LocalPlayer.mouseInterface;
                player.cursorItemIconEnabled = canPlaceItems;
                player.cursorItemIconID = Item.type;
            }
        }

        private bool CanReduceWandAmmo(int amount = 1, bool reduceStack = true, bool itemShouldBeInHand = false)
        {
            panel = UIUIState.Instance.multiWandWheel;
            int materialType = wandMaterials[panel.selectedIndex];
            return HelperMethods.CanReduceItemStack(materialType);
        }
        
        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer || !canPlaceItems) return false;
            panel = UIUIState.Instance.multiWandWheel;
            int tileType = wandPlacedTiles[panel.selectedIndex];

            if (HelperMethods.ValidTilePlacement(Player.tileTargetX, Player.tileTargetY) && CanReduceWandAmmo(reduceStack: false))
            {
                bool forced = HelperMethods.CanBreakToPlaceTile(Player.tileTargetX, Player.tileTargetY);
                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, tileType, forced: forced);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LivingWoodWand)
            .AddIngredient(ItemID.BoneWand)
            .AddIngredient(ItemID.LeafWand)
            .AddIngredient(ItemID.HiveWand)
            .AddIngredient(ItemID.LivingMahoganyWand)
            .AddIngredient(ItemID.LivingMahoganyLeafWand)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}
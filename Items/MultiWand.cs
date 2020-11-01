using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIStates;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BuilderEssentials.Utilities;

namespace BuilderEssentials.Items
{
    public class MultiWand : ModItem
    {
        private int baseRange = 8;
        private int toolRange;
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
        }

        public override Vector2? HoldoutOffset() => new Vector2(2, -9);

        public override void HoldItem(Player player)
        {
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            if (Main.netMode != NetmodeID.Server && mp.ValidCursorPos)
            {
                toolRange = baseRange; // or bigger if inf range?

                if (HelperMethods.ToolHasRange(toolRange) &&
                    (!Main.mouseLeft && !ItemsUIState.multiWandWheel.IsMouseHovering))
                {
                    canPlaceItems = true;
                    player.showItemIcon = true;
                    player.showItemIcon2 = item.type;
                }
                else if (!Main.mouseLeft)
                {
                    canPlaceItems = false;
                    player.showItemIcon = false;
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            MultiWandWheel panel = ItemsUIState.multiWandWheel;

            if (player.altFunctionUse == 0 && canPlaceItems) //LMB
            {
                //TODO: Fix items being placed midair?

                int materialType = wandMaterials[panel.selectedIndex];
                int tileType = wandPlacedTiles[panel.selectedIndex];
                if (HelperMethods.ValidTilePlacement(Player.tileTargetX, Player.tileTargetY) &&
                    HelperMethods.CanReduceItemStack(materialType, true)) //or inf placement
                    HelperMethods.PlaceTile(Player.tileTargetX, Player.tileTargetY, HelperMethods.ItemTypes.Tile,
                        tileType);
            }

            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            base.Update(ref gravity, ref maxFallSpeed);
            //Check if UI is Visible while item is dropped and close it if so.
            if (ItemsUIState.multiWandWheel.Visible)
                ItemsUIState.multiWandWheel.Hide();
        }

        private int mouseRightTimer = 0;
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
            //Check if UI is Visible while item is not the held one and close it if so.
            if (player.HeldItem.IsNotTheSameAs(item) && ItemsUIState.multiWandWheel.Visible)
                ItemsUIState.multiWandWheel.Hide();

            if (Main.mouseRight && player.HeldItem.IsTheSameAs(item) && HelperMethods.IsUIAvailable() &&
                ++mouseRightTimer == 2)
            {
                if (!ItemsUIState.multiWandWheel.Visible)
                    ItemsUIState.multiWandWheel.Show();
                else
                    ItemsUIState.multiWandWheel.Hide();
            }

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;
        }
    }
}
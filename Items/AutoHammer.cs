using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;

namespace BuilderEssentials.Items
{
    class AutoHammer : ModItem
    {
        private Point toolRange;
        private bool canHammerTiles;

        public override string Texture => "BuilderEssentials/Textures/Items/AutoHammer";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Better than a regular hammer!" +
                               "\nRight Click to open selection menu");
        }

        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 44;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.melee = true;
            item.damage = 26;
            item.hammer = 80;
            item.UseSound = SoundID.Item1;
            toolRange = new Point(8, 8);
        }

        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;

            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            if (Main.netMode != NetmodeID.Server && mp.ValidCursorPos)
            {
                canHammerTiles = HelperMethods.ToolHasRange(toolRange) &&
                                 HelperMethods.IsUIAvailable(playerNotWieldingItem: false);
                player.showItemIcon = canHammerTiles && !ItemsUIState.autoHammerWheel.IsMouseHovering;
                player.showItemIcon2 = item.type;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return true;

            AutoHammerWheel panel = ItemsUIState.autoHammerWheel;
            if (canHammerTiles && panel.selectedIndex != -1)
            {
                HelperMethods.ChangeSlope(panel.selectedIndex);
                return false;
            }

            return true;
        }

        private int mouseRightTimer = 0;

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;

            //Having two of the same item is breaking this
            if (Main.mouseRight && player.HeldItem.IsTheSameAs(item) &&
                HelperMethods.IsUIAvailable() && ++mouseRightTimer == 2)
                ItemsUIState.autoHammerWheel.Toggle();

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pwnhammer);
            recipe.AddRecipeGroup("BuilderEssentials:Woods", 200);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
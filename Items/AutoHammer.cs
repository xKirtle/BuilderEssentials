using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

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
            AutoHammerWheel panel = ItemsUIState.autoHammerWheel;

            if (player.altFunctionUse == 0 && canHammerTiles && panel.selectedIndex != -1)
            {
                HelperMethods.ChangeSlope(panel.selectedIndex);
                return false;
            }

            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            base.Update(ref gravity, ref maxFallSpeed);
            //Check if UI is Visible while item is dropped and close it if so.
            if (ItemsUIState.autoHammerWheel.Visible)
                ItemsUIState.autoHammerWheel.Hide();
        }

        private int mouseRightTimer = 0;

        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
            //Check if UI is Visible while item is not the held one and close it if so.
            if (player.HeldItem.IsNotTheSameAs(item) && ItemsUIState.autoHammerWheel.Visible)
                ItemsUIState.autoHammerWheel.Hide();

            if (Main.mouseRight && player.HeldItem.IsTheSameAs(item) && HelperMethods.IsUIAvailable() &&
                ++mouseRightTimer == 2)
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
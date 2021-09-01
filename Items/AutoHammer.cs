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
        private Vector2 toolRange;
        private bool canHammerTiles;
        private AutoHammerWheel panel;
        
        public override string Texture => "BuilderEssentials/Textures/Items/AutoHammer";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Better than a regular hammer!" +
                               "\nRight Click to open selection menu");
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Red;
            Item.damage = 26;
            Item.hammer = 80;
            Item.UseSound = SoundID.Item1;
            toolRange = new Vector2(9, 8);
            Item.tileBoost = 4;
        }

        private int mouseRightTimer = 0;
        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            panel = UIUIState.Instance.autoHammerWheel;
            
            if (Main.mouseRight && player.HeldItem == Item &&
                (HelperMethods.IsUIAvailable(notShowingMouseIcon: false) || panel.IsMouseHovering) && ++mouseRightTimer == 2)
                panel.Toggle();

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;
            
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            if (mp.ValidCursorPos)
            {
                canHammerTiles = HelperMethods.ToolHasRange(toolRange) && !Main.LocalPlayer.mouseInterface;
                player.cursorItemIconEnabled = canHammerTiles;
                player.cursorItemIconID = Item.type;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer || !canHammerTiles) return true;
            panel = UIUIState.Instance.autoHammerWheel;
            
            if (canHammerTiles && panel.selectedIndex != -1)
            {
                HelperMethods.ChangeSlope(Player.tileTargetX, Player.tileTargetY, panel.slopeType, panel.IsHalfBlock);
                return false;
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Pwnhammer)
                .AddRecipeGroup("BuilderEssentials:Wood", 200)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
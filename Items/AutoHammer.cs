using BuilderEssentials.UI.ItemsUI.Wheels;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace BuilderEssentials.Items
{
    class AutoHammer : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/AutoHammer";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Better than a regular hammer!" +
            "\nRight Click to open selection menu");
        }

        public override void SetDefaults()
        {
            item.damage = 26;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 10;
            item.useAnimation = 10;
            item.hammer = 80;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = 10000;
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            toolRange = new Point16(8, 6);
        }

        Point16 toolRange;
        int mouseRightTimer = 0;
        public override void HoldItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.mouseRight && Tools.IsUIAvailable() && (!player.mouseInterface || (AutoHammerWheel.AutoHammerUIOpen && AutoHammerWheel.AutoHammerWheelPanel.IsMouseHovering)) && player.HeldItem.IsTheSameAs(item) && ++mouseRightTimer == 2)
                    AutoHammerWheel.AutoHammerUIOpen = !AutoHammerWheel.AutoHammerUIOpen;

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;
            }
        }

        public override bool CanUseItem(Player player)
        {
            //Disabling vanilla hammer
            if (AutoHammerWheel.selectedIndex != -1)
            {
                if (AutoHammerWheel.IsAutoHammerUIVisible) return false;
                if (Tools.ToolHasRange(toolRange)) Tools.ChangeSlope(AutoHammerWheel.selectedIndex);

                return false;
            }

            return true;
        }
    }
}

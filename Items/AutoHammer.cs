using BuilderEssentials.UI.ItemsUI.Wheels;
using BuilderEssentials.Utilities;
using System;
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
        }

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
            BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();
            //Disabling vanilla hammer
            if (AutoHammerWheel.selectedIndex != -1)
            {
                if (AutoHammerWheel.IsAutoHammerUIVisible) return false;
                Point16 playerCenter = player.Center.ToTileCoordinates16();

                //modded hammer happens here. range (8, 6)
                if (modPlayer.infiniteRange || (Math.Abs(playerCenter.X - modPlayer.pointedTilePos.X) <= 8 && Math.Abs(playerCenter.Y - modPlayer.pointedTilePos.Y) <= 6))
                    Tools.ChangeSlope(AutoHammerWheel.selectedIndex);

                return false;
            }

            return true;
        }
    }
}

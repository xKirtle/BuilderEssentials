using System;
using BuilderEssentials.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class MirrorWand : ModItem
    {
        private bool firstValue = false;
        public static bool OperationComplete = false;
        public static Vector2 start;
        public static Vector2 end;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mirrors everything!");
            Item.staff[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.height = 20;
            item.width = 18;
            item.useTime = 1;
            item.useAnimation = 10;
            //item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.noMelee = true;
            item.noUseGraphic = true;
        }

        public override bool AltFunctionUse(Player player) //Right click selects area that will mirror stuff
        {
            if (OperationComplete)
                OperationComplete = false;

            return false;
        }

        public override void HoldItem(Player player)
        {
            if (!firstValue && (start.X != Player.tileTargetX || start.Y != Player.tileTargetY) && !OperationComplete)
            {
                start.X = Player.tileTargetX;
                start.Y = Player.tileTargetY;
                firstValue = true;
            }

            if (Main.mouseRight)
            {
                end.X = Player.tileTargetX;
                end.Y = Player.tileTargetY;
            }

            if (Main.mouseRightRelease && firstValue && !OperationComplete)
            {
                firstValue = false;
                OperationComplete = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
        public static bool firstValue = false;
        public static bool OperationComplete = false;
        public static Vector2 start;
        public static Vector2 end;
        //--------------------------------------
        public static bool firstvalueLeft = false;
        public static bool OperationCompleteLeft = false;
        public static Vector2 mouseLeftStart;
        public static Vector2 mouseLeftEnd;
        //--------------------------------------
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mirrors everything!");
            //Item.staff[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.height = 20;
            item.width = 18;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.noMelee = false;
        }

        public override bool AltFunctionUse(Player player) //Right click selects area that will mirror stuff
        {
            if (OperationComplete)
                OperationComplete = false;

            return false;
        }

        public override bool UseItem(Player player) //Left click selects tiles that will act as mirror axis
        {
            if (OperationCompleteLeft)
            {
                OperationCompleteLeft = false;
                firstvalueLeft = false;
            }

            return true;
        }

        public override void HoldItem(Player player)
        {
            //----------------Right Click----------------
            if (!firstValue && !OperationComplete)
            {
                start.X = Player.tileTargetX;
                start.Y = Player.tileTargetY;
                firstValue = true;
            }

            if (Main.mouseRight && !player.mouseInterface)
            {
                end.X = Player.tileTargetX;
                end.Y = Player.tileTargetY;
            }

            if (Main.mouseRightRelease && firstValue && !OperationComplete && !player.mouseInterface)
            {
                firstValue = false;
                OperationComplete = true;
            }

            //----------------Left Click----------------

            if (Main.mouseLeft && !firstvalueLeft && !OperationCompleteLeft)
            {
                mouseLeftStart = new Vector2(Player.tileTargetX, Player.tileTargetY);
                firstvalueLeft = true;
            }

            if (Main.mouseLeft && firstvalueLeft && !OperationCompleteLeft)
            {
                mouseLeftEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);
            }

            if (Main.mouseLeftRelease && firstvalueLeft && !OperationCompleteLeft)
            {
                firstvalueLeft = false;
                OperationCompleteLeft = true;
            }
        }
    }
}

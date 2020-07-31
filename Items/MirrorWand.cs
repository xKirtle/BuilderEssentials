using BuilderEssentials.UI;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class MirrorWand : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/MirrorWand";

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
        static bool TopBottom;
        static bool BottomTop;
        static bool LeftRight;
        static bool RightLeft;
        public static bool Horizontal;
        public static bool HorizontalLine;
        public static bool VerticalLine;
        //--------------------------------------
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mirrors everything!" +
            "\nRight Click to make a selection area" +
            "\nLeft Click to make a mirror axis" +
            "\nMight not work for all multi tiles");

            //Item.staff[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.height = 40;
            item.width = 40;
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
                start = new Vector2(Player.tileTargetX, Player.tileTargetY);
                firstValue = true;
            }

            if (Main.mouseRight)
                end = new Vector2(Player.tileTargetX, Player.tileTargetY);

            if (Main.mouseRightRelease && firstValue && !OperationComplete)
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
                //Update coords
                mouseLeftEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);

                //Direction
                TopBottom = mouseLeftStart.Y <= mouseLeftEnd.Y;
                BottomTop = mouseLeftStart.Y >= mouseLeftEnd.Y;
                LeftRight = mouseLeftStart.X <= mouseLeftEnd.X;
                RightLeft = mouseLeftStart.X >= mouseLeftEnd.X;
                Horizontal = Math.Abs(mouseLeftStart.X - mouseLeftEnd.X) > Math.Abs(mouseLeftStart.Y - mouseLeftEnd.Y);

                VerticalLine = (TopBottom || BottomTop) && !Horizontal;
                HorizontalLine = (LeftRight || RightLeft) && Horizontal;


                //Limit coords based on mirror width
                if (VerticalLine)
                {
                    if (mouseLeftEnd.X == mouseLeftStart.X)
                        return;
                    else if (mouseLeftEnd.X - mouseLeftStart.X > 1) //End Right side
                        mouseLeftEnd.X = mouseLeftStart.X + 1;
                    else if (mouseLeftEnd.X - mouseLeftStart.X < 1) //End Left side
                        mouseLeftEnd.X = mouseLeftStart.X - 1;
                }
                if (HorizontalLine)
                {
                    if (mouseLeftEnd.Y == mouseLeftStart.Y)
                        return;
                    else if (mouseLeftEnd.Y - mouseLeftStart.Y > 1) //End Bottom side
                        mouseLeftEnd.Y = mouseLeftStart.Y + 1;
                    else if (mouseLeftEnd.Y - mouseLeftStart.Y < 1) //End Top side
                        mouseLeftEnd.Y = mouseLeftStart.Y - 1;
                }
            }

            if (Main.mouseLeftRelease && firstvalueLeft && !OperationCompleteLeft)
            {
                firstvalueLeft = false;
                OperationCompleteLeft = true;
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (OperationComplete && OperationCompleteLeft && TransparentSelection.validPlacement)
                BuilderEssentials.validMirrorWand = true;
        }
    }
}

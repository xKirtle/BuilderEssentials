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
        //TODO: MAKE MIRROR SELECTION BE SAVED PER WORLD, RATHER THAN ON THE MOD ITSELF
        public override string Texture => "BuilderEssentials/Textures/Items/MirrorWand";

        public static bool firstSelectionValue = false;
        public static bool selectionComplete = false;
        public static Vector2 selectionStart;
        public static Vector2 selectionEnd;
        //--------------------------------------
        public static bool firstMirrorValue = false;
        public static bool mirrorComplete = false;
        public static Vector2 mirrorStart;
        public static Vector2 mirrorEnd;
        //--------------------------------------
        public static bool TopBottom;
        public static bool BottomTop;
        public static bool LeftRight;
        public static bool RightLeft;
        static bool Horizontal;
        public static bool HorizontalLine;
        public static bool VerticalLine;
        public static bool WideMirrorAxis;
        //--------------------------------------
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mirrors everything!" +
            "\nRight Click to make a selection area" +
            "\nLeft Click to make a mirror axis" +
            "\nMight not work for all multi tiles");
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

        public override Vector2? HoldoutOffset() => new Vector2(5, -7);

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("BuilderEssentials:MagicMirrors");
            recipe.AddIngredient(ItemID.SoulofLight, 25);
            recipe.AddIngredient(ItemID.SoulofNight, 25);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool AltFunctionUse(Player player) //Right click selects area that will mirror stuff
        {
            if (selectionComplete)
                selectionComplete = false;

            return false;
        }

        public override bool UseItem(Player player) //Left click selects tiles that will act as mirror axis
        {
            if (mirrorComplete)
            {
                mirrorComplete = false;
                firstMirrorValue = false;
            }

            return false;
        }

        //TODO: GOING LMB 1 WIDE LEFT TO RIGHT / RIGHT TO LEFT WILL MAKE IT DRAW IN THE MIRRORED SIDE BECAUSE OF SOME MATH.ABS
        public override void HoldItem(Player player)
        {
            //----------------Right Click----------------
            if (!firstSelectionValue && !selectionComplete)
            {
                selectionStart = new Vector2(Player.tileTargetX, Player.tileTargetY);
                firstSelectionValue = true;
            }

            if (Main.mouseRight)
                selectionEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);

            if (Main.mouseRightRelease && firstSelectionValue && !selectionComplete)
            {
                firstSelectionValue = false;
                selectionComplete = true;
            }

            //----------------Left Click----------------
            if (Main.mouseLeft && !firstMirrorValue && !mirrorComplete)
            {
                mirrorStart = new Vector2(Player.tileTargetX, Player.tileTargetY);
                firstMirrorValue = true;
                WideMirrorAxis = false;
            }

            if (Main.mouseLeft && firstMirrorValue && !mirrorComplete)
            {
                //Update coords
                mirrorEnd = new Vector2(Player.tileTargetX, Player.tileTargetY);

                //Direction
                TopBottom = mirrorStart.Y <= mirrorEnd.Y;
                BottomTop = mirrorStart.Y >= mirrorEnd.Y;
                LeftRight = mirrorStart.X <= mirrorEnd.X;
                RightLeft = mirrorStart.X >= mirrorEnd.X;
                Horizontal = Math.Abs(mirrorStart.X - mirrorEnd.X) > Math.Abs(mirrorStart.Y - mirrorEnd.Y);

                VerticalLine = (TopBottom || BottomTop) && !Horizontal;
                HorizontalLine = (LeftRight || RightLeft) && Horizontal;


                //Limit coords based on mirror width
                if (VerticalLine)
                {
                    if (mirrorEnd.X == mirrorStart.X) { }
                    else if (mirrorEnd.X - mirrorStart.X > 1) //End Right side
                        mirrorEnd.X = mirrorStart.X + 1;
                    else if (mirrorEnd.X - mirrorStart.X < 1) //End Left side
                        mirrorEnd.X = mirrorStart.X - 1;

                    WideMirrorAxis = mirrorEnd.X != mirrorStart.X;

                }
                if (HorizontalLine)
                {
                    if (mirrorEnd.Y == mirrorStart.Y) { }
                    else if (mirrorEnd.Y - mirrorStart.Y > 1) //End Bottom side
                        mirrorEnd.Y = mirrorStart.Y + 1;
                    else if (mirrorEnd.Y - mirrorStart.Y < 1) //End Top side
                        mirrorEnd.Y = mirrorStart.Y - 1;

                    WideMirrorAxis = mirrorEnd.Y != mirrorStart.Y;
                }
            }

            if (Main.mouseLeftRelease && firstMirrorValue && !mirrorComplete)
            {
                firstMirrorValue = false;
                mirrorComplete = true;
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (selectionComplete && mirrorComplete && TransparentSelection.validPlacement && selectionStart != selectionEnd)
                BuilderEssentials.validMirrorWand = true;
        }
    }
}

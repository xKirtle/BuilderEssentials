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

        private int oneTimeCheck = 0;
        // int startTileX;
        // int startTileY;
        // int endTileX;
        // int endTileY;

        public static Vector2 start;
        public static Vector2 end;
        public static bool OperationComplete = false;
        public override bool AltFunctionUse(Player player) //Right click selects are that will mirror stuff
        {
            if (++oneTimeCheck == 1 && start.X != Player.tileTargetX && start.Y != Player.tileTargetY)
            {
                start.X = Player.tileTargetX;
                start.Y = Player.tileTargetY;
            }
            OperationComplete = false;
            return false;
        }

        public override void HoldItem(Player player)
        {
            int posX = Player.tileTargetX;
            int posY = Player.tileTargetY;
            Tile tile = Main.tile[posX, posY];
            //-----------------------------

            if (Main.mouseRight)
            {
                end.X = Player.tileTargetX;
                end.Y = Player.tileTargetY;
            }

            if (Main.mouseRightRelease && oneTimeCheck > 1)
            {
                float some = Vector2.Distance(start, end);
                oneTimeCheck = 0;
                Main.NewText("Start Coordinate: " + start.X + ", " + start.Y + " / End Coordinate: " + end.X + ", " + end.Y);
                OperationComplete = true;
            }
        }
    }
}

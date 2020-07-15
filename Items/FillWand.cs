using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    public class FillWand : ModItem
    {
        int toolRange;
        public static int fillSelectionSize = 3;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fills Holes");
        }

        public override void SetDefaults()
        {
            item.height = 32;
            item.width = 32;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            toolRange = 8;
        }

        public override void HoldItem(Player player)
        {
            //Main.NewText(fillSelectionSize);

            //TODO: DECIDE HOW TO INPUT WHAT ITEM IN INVENTORY WILL BE USED TO PLACE BLOCKS


            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.mouseRight && Tools.IsUIAvailable() && player.HeldItem.IsTheSameAs(item))
                    RightClick();
            }
        }

        public override bool UseItem(Player player)
        {

            for (int i = 0; i < FillWand.fillSelectionSize; i++)
            {
                for (int j = 0; j < FillWand.fillSelectionSize; j++)
                {
                    //Vanilla automatically checks for empty spaces, instead of replacing
                    WorldGen.PlaceTile(Player.tileTargetX + j, Player.tileTargetY - i, 0);
                }
            }

            return true;
        }

        private void RightClick()
        {
            for (int i = 0; i < FillWand.fillSelectionSize; i++)
            {
                for (int j = 0; j < FillWand.fillSelectionSize; j++)
                {
                    Tile tile = Main.tile[Player.tileTargetX + j, Player.tileTargetY - i];

                    if (tile.type == TileID.Dirt)
                        tile.active(false);
                }
            }
        }
    }
}
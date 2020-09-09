using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class FillWand : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/FillWand";

        int oldPosX;
        int oldPosY;
        public static Item customItem = new Item();
        public static int selectedTileItemType = 0;
        public static int fillSelectionSize = 3;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fills Holes" +
            "\nLeft Click to place" +
            "\nRigth Click to remove" +
            "\nMiddle Click on tiles to select working tiles");
        }

        public override void SetDefaults()
        {
            item.height = 46;
            item.width = 46;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MoltenHamaxe);
            recipe.AddIngredient(ItemID.DirtRod);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
                LeftClick();
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();

                //Right Mouse Button
                if (Main.mouseRight && Tools.IsUIAvailable() && selectedTileItemType != -1)
                    RightClick();

                //Middle Mouse Button
                if (Main.mouseMiddle && !player.mouseInterface &&
                    (modPlayer.pointedTilePos.X != oldPosX || modPlayer.pointedTilePos.Y != oldPosY))
                {
                    selectedTileItemType = Tools.PickItem(modPlayer.pointedTile, false);
                    if (selectedTileItemType != -1)
                    {
                        oldPosX = (int)modPlayer.pointedTilePos.X;
                        oldPosY = (int)modPlayer.pointedTilePos.Y;
                    }
                }

                player.showItemIcon = true;

                if (selectedTileItemType != -1)
                    player.showItemIcon2 = selectedTileItemType;
            }
        }

        private void LeftClick()
        {
            bool tilePlaced = false;

            if (oldPosX != Player.tileTargetX || oldPosY != Player.tileTargetY)
            {
                for (int i = 0; i < fillSelectionSize; i++)
                {
                    for (int j = 0; j < fillSelectionSize; j++)
                    {
                        tilePlaced = false;
                        int posX = Player.tileTargetX + j;
                        int posY = Player.tileTargetY - i;
                        Tile tile = Main.tile[posX, posY];

                        //Vanilla automatically checks for empty spaces, instead of replacing
                        if (selectedTileItemType != -1 && tile.type == 0 && !tile.active())
                        {
                            customItem.SetDefaults(selectedTileItemType);
                            if (customItem.createTile != -1 && customItem.createWall == -1)
                            {
                                if (Tools.InfinitePlacement || Tools.ReduceItemStack(customItem.type))
                                {
                                    WorldGen.PlaceTile(posX, posY, customItem.createTile);
                                    tilePlaced = true;
                                }
                            }
                            else if (customItem.createTile == -1 && customItem.createWall != -1)
                            {
                                if (Tools.InfinitePlacement || Tools.ReduceItemStack(customItem.type))
                                {
                                    WorldGen.PlaceWall(posX, posY, customItem.createWall);
                                    tilePlaced = true;
                                }
                            }

                            if (tilePlaced && Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendTileSquare(-1, posX, posY, 1);
                        }
                    }
                }

                if (tilePlaced)
                {
                    oldPosX = Player.tileTargetX;
                    oldPosY = Player.tileTargetY;
                }
            }
        }

        private void RightClick()
        {
            for (int i = 0; i < fillSelectionSize; i++)
            {
                for (int j = 0; j < fillSelectionSize; j++)
                {
                    int posX = Player.tileTargetX + j;
                    int posY = Player.tileTargetY - i;
                    Tile tile = Main.tile[posX, posY];
                    customItem.SetDefaults(selectedTileItemType);

                    if (tile.type == customItem.createTile)
                        WorldGen.KillTile(posX, posY);
                    else if (tile.wall == customItem.createWall)
                        WorldGen.KillWall(posX, posY);

                    //Add support in MirrorWand to remove mirrored blocks here
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, posX, posY, 1);
                }
            }
        }
    }
}
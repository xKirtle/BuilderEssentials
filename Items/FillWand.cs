using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class FillWand : ModItem
    {
        int toolRange; //Should it have range?

        int oldPosX;
        int oldPosY;
        public static Item customItem = new Item();
        public static int selectedTileItemType = 0;
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
            if (player.whoAmI == Main.myPlayer)
            {
                BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();

                //Right Mouse Button
                if (Main.mouseRight && Tools.IsUIAvailable() && player.HeldItem.IsTheSameAs(item) &&
                    selectedTileItemType != -1)
                    RightClick();

                //Middle Mouse Button
                if (Main.mouseMiddle && !player.mouseInterface && !Main.playerInventory &&
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

        public override bool UseItem(Player player)
        {
            for (int i = 0; i < fillSelectionSize; i++)
            {
                for (int j = 0; j < fillSelectionSize; j++)
                {
                    //Vanilla automatically checks for empty spaces, instead of replacing
                    //-1 == non existant
                    if (selectedTileItemType != -1)
                    {
                        customItem.SetDefaults(selectedTileItemType);
                        if (customItem.createTile != -1 && customItem.createWall == -1) //Tile
                            WorldGen.PlaceTile(Player.tileTargetX + j, Player.tileTargetY - i, customItem.createTile);
                        else if (customItem.createTile == -1 && customItem.createWall != -1) //Wall
                            WorldGen.PlaceWall(Player.tileTargetX + j, Player.tileTargetY - i, customItem.createWall);
                    }
                }
            }

            return true;
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
                }
            }
        }
    }
}
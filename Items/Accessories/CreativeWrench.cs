using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    class CreativeWrench : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Useful for Building!");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }

        private int oldPosX;
        private int oldPosY;
        int mouseRightTimer = 0;
        BuilderPlayer modPlayer;
        bool autoHammerAlert = false;
        Tile previousClickedTile;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

                if (modPlayer.InfinitePlacementSelected)
                    player.AddBuff(mod.BuffType("InfinitePlacementBuff"), 10);

                player.blockRange += 55;
                player.wallSpeed += 10;
                player.tileSpeed += 50;
                Player.tileRangeX = 65;
                Player.tileRangeY = 55;

                if (modPlayer.InfinitePlacementSelected)
                    modPlayer.InfinitePlacement = true;

                //Thanks direwolf420 for the monstrosity checks
                if (Main.mouseRight && player.talkNPC == -1 && !Main.HoveringOverAnNPC && !player.showItemIcon && !Main.editSign
                    && !Main.editChest && !Main.blockInput && !player.dead && !Main.gamePaused && Main.hasFocus && !player.CCed
                    && (!player.mouseInterface || (BasePanel.creativeWheelUIOpen && CreativeWheelRework.CreativeWheelReworkPanel.IsMouseHovering))
                    && !BasePanel.paintingUIOpen && player.inventory[player.selectedItem].IsAir)
                {
                    if (++mouseRightTimer == 2)
                        BasePanel.creativeWheelUIOpen = !BasePanel.creativeWheelUIOpen;
                }

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;

                if (Main.mouseMiddle && modPlayer.colorPickerSelected && !player.mouseInterface)
                {
                    //Thanks Oli. B for the concept
                    int posX = Player.tileTargetX;
                    int posY = Player.tileTargetY;
                    Tile tile = Main.tile[posX, posY];
                    Item item = new Item();
                    bool foundItem = false;

                    if (oldPosX != posX || oldPosY != posY)
                    {
                        if (tile.type >= 0 && tile.active())
                        {
                            for (int i = 0; i < ItemLoader.ItemCount; i++)
                            {
                                item.SetDefaults(i);
                                if (item.createTile == tile.type)
                                {
                                    foundItem = true;
                                    break;
                                }
                            }
                        }
                        else if (tile.type >= 0 && tile.wall >= 0)
                        {
                            for (int i = 0; i < ItemLoader.ItemCount; i++)
                            {
                                item.SetDefaults(i);
                                if (item.createWall == tile.wall)
                                {
                                    foundItem = true;
                                    break;
                                }
                            }
                        }

                        //organize inventory
                        if (foundItem)
                        {
                            //Furniture Check
                            //If it is a furniture and has a different frame, item will be changed to the correct frame item
                            FurnitureFinder.FindFurniture(tile, ref item);

                            bool isItemInInventory = false;
                            for (int i = 0; i < 50; i++)
                            {
                                if (player.inventory[i].IsTheSameAs(item))
                                {
                                    //Finds item in inventory and switch with selected item
                                    Item selectedItem = player.inventory[player.selectedItem];
                                    player.inventory[player.selectedItem] = player.inventory[i];
                                    player.inventory[i] = selectedItem;
                                    isItemInInventory = true;
                                    break;
                                }
                            }

                            if (!isItemInInventory)
                            {
                                for (int i = 0; i < 50; i++)
                                {
                                    if (player.inventory[i].IsAir)
                                    {
                                        //Find first air space in inventory and switches selected item to there
                                        Item selectedItem = player.inventory[player.selectedItem];
                                        player.inventory[i] = selectedItem;

                                        player.inventory[player.selectedItem] = item;
                                        break;
                                    }
                                }
                            }

                            oldPosX = posX;
                            oldPosY = posY;
                        }
                    }
                }

                if (Main.mouseLeft && modPlayer.autoHammerSelected && player.inventory[player.selectedItem].IsAir
                    && CreativeWheel.creativeWheel != null && !player.mouseInterface && !Main.playerInventory)
                {
                    if (!CreativeWheel.creativeWheel.IsMouseHovering)
                    {
                        //DISABLE USE ON GAME INTERFACES WHEN USER CLICKS ON SETTINGS FOR EXAMPLE
                        int posX = Player.tileTargetX;
                        int posY = Player.tileTargetY;
                        Tile tile = Main.tile[posX, posY];

                        if (tile.type >= 0 && tile.active())
                        {
                            switch (modPlayer.autoHammerSelectedIndex)
                            {
                                case 0:
                                    tile.halfBrick(false);
                                    tile.slope(1);
                                    break;
                                case 1:
                                    tile.halfBrick(false);
                                    tile.slope(2);
                                    break;
                                case 2:
                                    tile.halfBrick(false);
                                    tile.slope(3);
                                    break;
                                case 3:
                                    tile.halfBrick(false);
                                    tile.slope(4);
                                    break;
                                case 4:
                                    tile.slope(0);
                                    tile.halfBrick(true);
                                    break;
                                case 5:
                                    tile.halfBrick(false);
                                    tile.slope(0);
                                    break;
                            }

                            WorldGen.SquareTileFrame(posX, posY, true);
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendTileSquare(-1, posX, posY, 1);

                            if (previousClickedTile != null)
                            {
                                if (!previousClickedTile.HasSameSlope(tile) || (oldPosX != posX || oldPosY != posY))
                                    Main.PlaySound(SoundID.Dig);
                            }
                            else
                                Main.PlaySound(SoundID.Dig);

                            previousClickedTile = tile;
                            oldPosX = posX;
                            oldPosY = posY;
                        }
                    }
                }
                else if (Main.mouseLeft && modPlayer.autoHammerSelected && !player.inventory[player.selectedItem].IsAir
                    && player.whoAmI == Main.myPlayer && !player.mouseInterface && !Main.playerInventory)
                {
                    if (!autoHammerAlert)
                    {
                        Main.NewText("Please use an empty slot on your quick bar when using the Auto Hammer!");
                        autoHammerAlert = true;
                    }
                }
            }
        }
    }

    public class InfinitePlacementTile : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, Item item)
        {
            item.consumable = Main.LocalPlayer.GetModPlayer<BuilderPlayer>().InfinitePlacement == true ? false : true;
        }
    }

    public class InfinitePlacementWall : GlobalWall
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            item.consumable = Main.LocalPlayer.GetModPlayer<BuilderPlayer>().InfinitePlacement == true ? false : true;
        }
    }
}

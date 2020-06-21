using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Utilities
{

    public class ItemPicker
    {
        public static void PickItem(ref int oldPosX, ref int oldPosY)
        {
            Player player = Main.LocalPlayer;
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
    }
}
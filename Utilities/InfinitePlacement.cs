using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Utilities
{
    //TODO: REDO THIS WHOLE THING BECAUSE IT SUCKS
    public class InfinitePlacementTile : GlobalTile
    {
        private int oldPosX;
        private int oldPosY;
        private List<Item> modifiedItemsConsumable = new List<Item>();
        private bool canPlace = true;
        private bool canMirror = true;
        public override bool CanPlace(int i, int j, int type)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            //Placement Anywhere
            if (Tools.PlacementAnywhere && !tile.active() && (oldPosX != i || oldPosY != j))
            {
                Item selectedItem = Main.LocalPlayer.HeldItem;
                WorldGen.PlaceTile(i, j, selectedItem.createTile, false, true, -1, selectedItem.placeStyle);
                tile = Framing.GetTileSafely(i, j);

                canPlace = false; //To make sure we don't call AutoReplaceStack on CanPlace and PlaceInWorld
                Tools.AutoReplaceStack(selectedItem, false);

                if (!Tools.InfinitePlacement)
                {
                    if (selectedItem.type == ItemID.LivingMahoganyWand || selectedItem.type == ItemID.LivingMahoganyLeafWand)
                        Tools.CanReduceItemStack(ItemID.RichMahogany);
                    else if (selectedItem.type == ItemID.LivingWoodWand || selectedItem.type == ItemID.LeafWand)
                        Tools.CanReduceItemStack(ItemID.Wood);
                    else if (selectedItem.type == ItemID.BoneWand)
                        Tools.CanReduceItemStack(ItemID.Bone);
                    else if (selectedItem.type == ItemID.HiveWand)
                        Tools.CanReduceItemStack(ItemID.Hive);
                    else if (selectedItem.type == ItemID.StaffofRegrowth) { } //Condition to make specific wands not removed
                    else
                    {
                        //Checks if tile was actually placed, since multi tiles were being reduced without actually being placed
                        if (tile.type == selectedItem.createTile || tile.type == selectedItem.createWall)
                        {
                            selectedItem.stack--;
                            oldPosX = i;
                            oldPosY = j;
                        }
                    }
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);

                canMirror = false;
                Tools.MirrorPlacement(i, j, selectedItem.type);

                return true;
            }

            return base.CanPlace(i, j, type);
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (!Tools.InfinitePlacement)
            {
                if (item.consumable == false && modifiedItemsConsumable.Contains(item))
                {
                    modifiedItemsConsumable.Remove(item);
                    item.consumable = true;
                }

                //Wands aren't consumable items
                if (item.type == ItemID.BoneWand || item.type == ItemID.HiveWand || item.type == ItemID.LeafWand ||
                item.type == ItemID.LivingMahoganyWand || item.type == ItemID.LivingMahoganyLeafWand ||
                item.type == ItemID.LivingWoodWand || item.type == ItemID.StaffofRegrowth)
                    item.consumable = false;

                if (canPlace) //avoid calling AutoReplaceStack twice if it has been called above
                    Tools.AutoReplaceStack(item);
                else
                    canPlace = true;
            }

            if (canMirror)
                Tools.MirrorPlacement(i, j, item.type);
            else
                canMirror = true;

            if (Tools.InfinitePlacement)
            {
                item.consumable = false;
                //Could be problematic checking for their names?
                if (!modifiedItemsConsumable.Contains(item) && !item.Name.Contains("Unlimited") && !item.Name.Contains("Infinite"))
                    modifiedItemsConsumable.Add(item);
            }
            else
                base.PlaceInWorld(i, j, item);
        }
    }

    public class InfinitePlacementWall : GlobalWall
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            item.consumable = !Tools.InfinitePlacement;
            Tools.MirrorPlacement(i, j, item.type);
            Tools.AutoReplaceStack(item);
        }
    }
}

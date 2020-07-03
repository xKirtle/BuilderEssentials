using BuilderEssentials.Items.Accessories;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static BuilderEssentials.BuilderPlayer;

namespace BuilderEssentials.Utilities
{
    public class InfinitePlacementTile : GlobalTile
    {
        private int oldPosX;
        private int oldPosY;
        private List<Item> modifiedItemsConsumable = new List<Item>();
        public override bool CanPlace(int i, int j, int type)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];

            //Placement Anywhere
            if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.PlacementAnywhere) && !tile.active() &&
                (oldPosX != i || oldPosY != j) && Tools.IsCreativeWrenchEquipped())
            {
                Item selectedItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, selectedItem.createTile, false, true, -1, selectedItem.placeStyle);
                tile = Main.tile[Player.tileTargetX, Player.tileTargetY];

                if (!modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement)
                && !modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade))
                {
                    if (selectedItem.type == ItemID.LivingMahoganyWand || selectedItem.type == ItemID.LivingMahoganyLeafWand)
                        Tools.ReduceItemStack(ItemID.RichMahogany);
                    else if (selectedItem.type == ItemID.LivingWoodWand || selectedItem.type == ItemID.LeafWand)
                        Tools.ReduceItemStack(ItemID.Wood);
                    else if (selectedItem.type == ItemID.BoneWand)
                        Tools.ReduceItemStack(ItemID.Bone);
                    else if (selectedItem.type == ItemID.HiveWand)
                        Tools.ReduceItemStack(ItemID.Hive);
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

                if (modPlayer.mirrorWandEffects)
                    Tools.MirrorWandPlacement(Player.tileTargetX, Player.tileTargetY, selectedItem, -1);

                return base.CanPlace(i, j, type);
            }

            //Doesn't work for walls?
            // if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.PlacementAnywhere))// && tile.wall >= 0)
            // {
            //     Item selectedItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
            //     WorldGen.PlaceWall(Player.tileTargetX, Player.tileTargetY, selectedItem.createWall);
            //     return true;
            // }

            return base.CanPlace(i, j, type);
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            if (!modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement) &&
                !modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade))
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
            }

            if (modPlayer.mirrorWandEffects)
                Tools.MirrorWandPlacement(i, j, item, -1);

            if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement) ||
                modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade))
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
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade) ||
                modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement))
                item.consumable = false;
            else
                item.consumable = true;

            if (modPlayer.mirrorWandEffects)
                Tools.MirrorWandPlacement(i, j, item, type);
        }
    }
}

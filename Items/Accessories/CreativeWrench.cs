using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static BuilderEssentials.BuilderPlayer;

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

                if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement))
                    player.AddBuff(mod.BuffType("InfinitePlacementBuff"), 10);

                player.blockRange += 55;
                player.wallSpeed += 10;
                player.tileSpeed += 50;
                Player.tileRangeX = 65;
                Player.tileRangeY = 55;

                //Thanks direwolf420 for the monstrosity checks
                //Right click timer
                if (Main.mouseRight && UIUtilities.IsUIAvailable()
                    && (!player.mouseInterface || CreativeWheelRework.CreativeWheelReworkPanel.IsMouseHovering)
                    && !BasePanel.paintingUIOpen && player.inventory[player.selectedItem].IsAir)
                {
                    if (++mouseRightTimer == 2)
                        BasePanel.creativeWheelUIOpen = !BasePanel.creativeWheelUIOpen;
                }

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;

                //ItemPicker
                if (Main.mouseMiddle && modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.ItemPicker)
                && !player.mouseInterface && !Main.playerInventory)
                    ItemPicker.PickItem(ref oldPosX, ref oldPosY);

                //AutoHammer
                if (Main.mouseLeft && modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.AutoHammer)
                && player.inventory[player.selectedItem].IsAir && CreativeWheelRework.CreativeWheelReworkPanel != null && !player.mouseInterface
                && !Main.playerInventory)
                {
                    if (!CreativeWheelRework.CreativeWheelReworkPanel.IsMouseHovering)
                        AutoHammer.ChangeSlope(ref oldPosX, ref oldPosY, ref previousClickedTile);
                }
                else if (Main.mouseLeft && modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.AutoHammer)
                && !player.inventory[player.selectedItem].IsAir && !player.mouseInterface && !Main.playerInventory)
                {
                    if (!autoHammerAlert)
                    {
                        Main.NewText("Please use an empty slot on your quick bar when using the Auto Hammer!");
                        autoHammerAlert = true;
                    }
                }

                //PlacementAnywhere
                if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.PlacementAnywhere))
                {

                }
            }
        }

        public override void AddRecipes()
        {
            //Should it have a recipe or only be obtainable with cheats?
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(mod.GetItem("InfiniteWrench"));
            modRecipe.AddIngredient(ItemID.LunarBar, 50);
            modRecipe.AddTile(TileID.LunarCraftingStation);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }

    public class InfinitePlacementTile : GlobalTile
    {
        private int oldPosX;
        private int oldPosY;
        public override bool CanPlace(int i, int j, int type)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];

            //Placement Anywhere
            if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.PlacementAnywhere) && !tile.active() &&
                oldPosX != i && oldPosY != j)
            {
                Item selectedItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, selectedItem.createTile, false, false, -1, selectedItem.placeStyle);

                if (!modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement)
                && !modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade))
                {
                    if (selectedItem.type == ItemID.BoneWand || selectedItem.type == ItemID.HiveWand ||
                        selectedItem.type == ItemID.LeafWand || selectedItem.type == ItemID.LivingMahoganyWand ||
                        selectedItem.type == ItemID.LivingMahoganyLeafWand || selectedItem.type == ItemID.LivingWoodWand ||
                        selectedItem.type == ItemID.StaffofRegrowth)
                    {
                        //I'm sorry but it's just easier this way
                        //Wands are infinite, might need to loop through the inventory and check if they have ammo to decrease their stack?
                    }
                    else //TODO: This is still reducing the stack by 1 when trying to place multi tiles in the air
                    {
                        selectedItem.stack--;
                        oldPosX = i;
                        oldPosY = j;
                    }
                }

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);

                if (modPlayer.mirrorWandEffects)
                    UIUtilities.MirrorWandPlacement(Player.tileTargetX, Player.tileTargetY, selectedItem, -1);

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

        //Infinite Placement Stuff
        public override void PlaceInWorld(int i, int j, Item item)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            if (!modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement)
            && !modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade))
            {
                if (item.consumable == false)
                    item.consumable = true;

                //Wands will decrease their stack either way, but at least won't be consumed.
                //Perhaps make my own Multi Wand to compensate for that?
                if (item.type == ItemID.BoneWand || item.type == ItemID.HiveWand || item.type == ItemID.LeafWand ||
                    item.type == ItemID.LivingMahoganyWand || item.type == ItemID.LivingMahoganyLeafWand ||
                    item.type == ItemID.LivingWoodWand || item.type == ItemID.StaffofRegrowth)
                    item.consumable = false;
            }

            if (modPlayer.mirrorWandEffects)
                UIUtilities.MirrorWandPlacement(i, j, item, -1);

            if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement)
            || modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade))
                item.consumable = false;
            else
                base.PlaceInWorld(i, j, item);
        }
    }

    public class InfinitePlacementWall : GlobalWall
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade)
            || modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement))
                item.consumable = false;
            else
                item.consumable = true;

            if (modPlayer.mirrorWandEffects)
                UIUtilities.MirrorWandPlacement(i, j, item, type);
        }
    }
}
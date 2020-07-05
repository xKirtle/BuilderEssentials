using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
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

                //Accessory Stats
                player.blockRange += 55;
                player.wallSpeed += 10;
                player.tileSpeed += 50;
                Player.tileRangeX = 65;
                Player.tileRangeY = 55;
                modPlayer.infiniteRange = true;

                //InfinitePickupRange
                if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePickupRange))
                    Player.defaultItemGrabRange = 1000000; //I have no idea how much it should be so that should suffice??

                //Infinite Placement
                if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement))
                    player.AddBuff(mod.BuffType("InfinitePlacementBuff"), 10);

                //Right click timer
                if (Main.mouseRight && Tools.IsUIAvailable() &&
                player.inventory[player.selectedItem].IsAir &&
                (!player.mouseInterface || CreativeWheelRework.CreativeWheelReworkPanel.IsMouseHovering))
                {
                    if (++mouseRightTimer == 2)
                        BasePanel.creativeWheelUIOpen = !BasePanel.creativeWheelUIOpen;
                }

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;

                //ItemPicker
                if (Main.mouseMiddle && modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.ItemPicker) &&
                    !player.mouseInterface && !Main.playerInventory)
                    Tools.PickItem(ref oldPosX, ref oldPosY);

                //AutoHammer
                if (Main.mouseLeft && modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.AutoHammer) &&
                player.inventory[player.selectedItem].IsAir && CreativeWheelRework.CreativeWheelReworkPanel != null &&
                !player.mouseInterface && !Main.playerInventory)
                {
                    if (!CreativeWheelRework.CreativeWheelReworkPanel.IsMouseHovering)
                        Tools.ChangeSlope(ref oldPosX, ref oldPosY, ref previousClickedTile);
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
}
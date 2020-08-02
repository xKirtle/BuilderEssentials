using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static BuilderEssentials.Utilities.Tools;

namespace BuilderEssentials.Items.Accessories
{
    class CreativeWrench : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/CreativeWrench";

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

                //Checking if player's heldItem is air
                if (!player.HeldItem.IsAir)
                    Tools.UIPanelLogic(CreativeWheel.CreativeWheelPanel, ref CreativeWheel.CreativeWheelUIOpen, ref CreativeWheel.IsCreativeWheelVisible);

                //Accessory Stats
                player.blockRange += 55;
                player.wallSpeed += 10;
                player.tileSpeed += 50;
                Player.tileRangeX = 65;
                Player.tileRangeY = 55;
                modPlayer.infiniteRange = true;

                //InfinitePickupRange
                if (modPlayer.creativeWheelSelectedIndex.Contains(CreativeWheelItem.InfinitePickupRange.ToInt()))
                    Player.defaultItemGrabRange = 1000000; //I have no idea how much it should be so that should suffice??

                //Infinite Placement
                if (modPlayer.creativeWheelSelectedIndex.Contains(CreativeWheelItem.InfinitePlacement.ToInt()))
                    player.AddBuff(mod.BuffType("InfinitePlacementBuff"), 10);

                //Right click timer
                if (Main.mouseRight && Tools.IsUIAvailable() && player.HeldItem.IsAir &&
                (!player.mouseInterface || CreativeWheel.CreativeWheelPanel.IsMouseHovering))
                {
                    if (++mouseRightTimer == 2)
                        CreativeWheel.CreativeWheelUIOpen = !CreativeWheel.CreativeWheelUIOpen;
                }

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;

                //ItemPicker
                if (Main.mouseMiddle && modPlayer.creativeWheelSelectedIndex.Contains(CreativeWheelItem.ItemPicker.ToInt()) &&
                    !player.mouseInterface && !Main.playerInventory && (modPlayer.pointedTilePos.X != oldPosX || modPlayer.pointedTilePos.Y != oldPosY))
                    if (player.HeldItem.type != ModContent.ItemType<FillWand>())
                    {
                        Tile tile = modPlayer.pointedTile;
                        if (Tools.PickItem(tile) != -1)
                        {
                            oldPosX = (int)modPlayer.pointedTilePos.X;
                            oldPosY = (int)modPlayer.pointedTilePos.Y;
                        }
                    }

                //AutoHammer
                if (Main.mouseLeft && modPlayer.creativeWheelSelectedIndex.Contains(CreativeWheelItem.AutoHammer.ToInt()) &&
                player.HeldItem.IsAir && CreativeWheel.CreativeWheelPanel != null &&
                !player.mouseInterface && !Main.playerInventory && !Main.ingameOptionsWindow)
                {
                    if (!CreativeWheel.CreativeWheelPanel.IsMouseHovering)
                        Tools.ChangeSlope(ref oldPosX, ref oldPosY, ref previousClickedTile, CreativeWheel.autoHammerSelectedIndex);
                }
                else if (Main.mouseLeft && modPlayer.creativeWheelSelectedIndex.Contains(CreativeWheelItem.AutoHammer.ToInt())
                && !player.HeldItem.IsAir && !player.mouseInterface && !Main.playerInventory)
                {
                    if (!autoHammerAlert)
                    {
                        Main.NewText("Please use an empty slot on your quick bar when using the Auto Hammer!");
                        autoHammerAlert = true;
                    }
                }
            }
        }

        public override void UpdateEquip(Player player) => player.GetModPlayer<BuilderPlayer>().isCreativeWrenchEquiped = true;

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
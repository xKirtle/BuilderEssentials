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
            //TODO: SEPARATE FUNCTIONALITY INTO SEPARATE FILES
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

                //TODO: Make a static bool in Utilities that evaluates if it is possible to open a UI to improve readability
                //Thanks direwolf420 for the monstrosity checks
                //Right click timer
                if (Main.mouseRight && UIUtilities.IsUIAvailable()
                    && (!player.mouseInterface || (BasePanel.creativeWheelUIOpen && CreativeWheelRework.CreativeWheelReworkPanel.IsMouseHovering))
                    && !BasePanel.paintingUIOpen && player.inventory[player.selectedItem].IsAir && !Main.playerInventory)
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
    }


    //Infinite Placement Stuff
    public class InfinitePlacementTile : GlobalTile
    {
        public override bool CanPlace(int i, int j, int type)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            if (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.PlacementAnywhere) && !tile.active())
            {
                Item selectedItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, selectedItem.createTile, false, false, -1, selectedItem.placeStyle);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);

                return true;
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
            item.consumable = modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement) ? false : true;
        }
    }

    public class InfinitePlacementWall : GlobalWall
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            item.consumable = modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement) ? false : true;
        }
    }
}

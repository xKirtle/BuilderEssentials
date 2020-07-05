using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static BuilderEssentials.BuilderPlayer;

namespace BuilderEssentials.Items
{
    class MultiWand : ModItem
    {
        int toolRange;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Contains all wands");
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
            item.noMelee = false;
            toolRange = 8;
        }

        int mouseRightTimer = 0;
        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (BasePanel.wandsWheelPanel != null && !player.inventory[player.selectedItem].IsTheSameAs(item))
                {
                    BasePanel.wandsWheelPanel.Remove();
                    BasePanel.wandsWheelUIOpen = false;
                }

                if (Main.mouseRight && Tools.IsUIAvailable()
                        && (!player.mouseInterface || (BasePanel.wandsWheelUIOpen && BasePanel.wandsWheelPanel.IsMouseHovering))
                        && player.inventory[player.selectedItem].IsTheSameAs(item))
                {
                    if (++mouseRightTimer == 2)
                        BasePanel.wandsWheelUIOpen = !BasePanel.wandsWheelUIOpen;
                }

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;
            }
        }

        int oldPosX;
        int oldPosY;
        public override bool UseItem(Player player)
        {
            BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();
            bool tilePlaced = false;
            int posX = Player.tileTargetX;
            int posY = Player.tileTargetY;
            Tile tile = Main.tile[posX, posY];

            bool infinitePlacement = Tools.IsCreativeWrenchEquipped() &&
                (modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinitePlacement) ||
                modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.InfinityUpgrade));
            bool placementAnywhere = Tools.IsCreativeWrenchEquipped() &&
                modPlayer.creativeWheelSelectedIndex.Contains((int)CreativeWheelItem.PlacementAnywhere);

            if ((modPlayer.infiniteRange || Tools.ToolHasRange(toolRange)) && (posX != oldPosX || posY != oldPosY) && !tile.active())
            {
                oldPosX = posX;
                oldPosY = posY;

                if (placementAnywhere || Tools.HasTileAround(posX, posY))
                {
                    switch (modPlayer.wandWheelSelectedIndex)
                    {
                        case 0: //living wood
                            if (infinitePlacement || Tools.ReduceItemStack(ItemID.Wood))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.LivingWood);
                                tilePlaced = true;
                            }
                            break;
                        case 1: //bone
                            if (infinitePlacement || Tools.ReduceItemStack(ItemID.Bone))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.BoneBlock);
                                tilePlaced = true;
                            }
                            break;
                        case 2: //leaf
                            if (infinitePlacement || Tools.ReduceItemStack(ItemID.Wood))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.LeafBlock);
                                tilePlaced = true;
                            }
                            break;
                        case 3: //hive
                            if (infinitePlacement || Tools.ReduceItemStack(ItemID.Hive))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.Hive);
                                tilePlaced = true;
                            }
                            break;
                        case 4: //rich mahogany
                            if (infinitePlacement || Tools.ReduceItemStack(ItemID.RichMahogany))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.LivingMahogany);
                                tilePlaced = true;
                            }
                            break;
                        case 5: //living wood leaf
                            if (infinitePlacement || Tools.ReduceItemStack(ItemID.RichMahogany))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.LivingMahoganyLeaves);
                                tilePlaced = true;
                            }
                            break;
                    }

                    if (tilePlaced)
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);

                        return true;
                    }
                }
            }

            return false;
        }

        public override void HoldItem(Player player)
        {
            BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();
            if (modPlayer.infiniteRange || Tools.ToolHasRange(toolRange))
            {
                player.showItemIcon = true;
                switch (modPlayer.wandWheelSelectedIndex)
                {
                    case 0:
                            player.showItemIcon2 = ItemID.LivingWoodWand;
                        break;
                    case 1:
                            player.showItemIcon2 = ItemID.BoneWand;
                        break;
                    case 2:
                            player.showItemIcon2 = ItemID.LeafWand;
                        break;
                    case 3:
                        player.showItemIcon2 = ItemID.HiveWand;
                        break;
                    case 4:
                        player.showItemIcon2 = ItemID.LivingMahoganyWand;
                        break;
                    case 5:
                        player.showItemIcon2 = ItemID.LivingMahoganyLeafWand;
                        break;
                }
            }
        }
    }
}

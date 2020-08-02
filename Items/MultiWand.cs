﻿using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static BuilderEssentials.BuilderPlayer;

namespace BuilderEssentials.Items
{
    class MultiWand : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/MultiWand";

        int toolRange;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Contains all building wands");
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
                if (MultiWandWheel.MultiWandWheelPanel != null && !player.HeldItem.IsTheSameAs(item))
                {
                    MultiWandWheel.MultiWandWheelPanel.Remove();
                    MultiWandWheel.WandsWheelUIOpen = false;
                }

                if (Main.mouseRight && Tools.IsUIAvailable()
                        && (!player.mouseInterface || (MultiWandWheel.WandsWheelUIOpen && MultiWandWheel.MultiWandWheelPanel.IsMouseHovering))
                        && player.HeldItem.IsTheSameAs(item))
                {
                    if (++mouseRightTimer == 2)
                        MultiWandWheel.WandsWheelUIOpen = !MultiWandWheel.WandsWheelUIOpen;
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
            Item newItem = new Item();

            if ((modPlayer.infiniteRange || Tools.ToolHasRange(toolRange)) && (posX != oldPosX || posY != oldPosY) && !tile.active())
            {
                oldPosX = posX;
                oldPosY = posY;

                if ((Tools.PlacementAnywhere || Tools.ValidTilePlacement(posX, posY)) &&
                (MultiWandWheel.MultiWandWheelPanel == null || !MultiWandWheel.MultiWandWheelPanel.IsMouseHovering)
                && !MultiWandWheel.IsWandsUIVisible)
                {
                    switch (MultiWandWheel.selectedIndex)
                    {
                        case 0: //living wood
                            if (Tools.InfinitePlacement || Tools.ReduceItemStack(ItemID.Wood))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.LivingWood);
                                newItem.SetDefaults(ItemID.Wood);
                                Tools.MirrorPlacement(Player.tileTargetX, Player.tileTargetY, newItem.type);
                                tilePlaced = true;
                            }
                            break;
                        case 1: //bone
                            if (Tools.InfinitePlacement || Tools.ReduceItemStack(ItemID.Bone))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.BoneBlock);
                                newItem.SetDefaults(ItemID.Bone);
                                Tools.MirrorPlacement(Player.tileTargetX, Player.tileTargetY, newItem.type);
                                tilePlaced = true;
                            }
                            break;
                        case 2: //leaf
                            if (Tools.InfinitePlacement || Tools.ReduceItemStack(ItemID.Wood))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.LeafBlock);
                                newItem.SetDefaults(ItemID.Wood);
                                Tools.MirrorPlacement(Player.tileTargetX, Player.tileTargetY, newItem.type);
                                tilePlaced = true;
                            }
                            break;
                        case 3: //hive
                            if (Tools.InfinitePlacement || Tools.ReduceItemStack(ItemID.Hive))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.Hive);
                                newItem.SetDefaults(ItemID.Hive);
                                Tools.MirrorPlacement(Player.tileTargetX, Player.tileTargetY, newItem.type);
                                tilePlaced = true;
                            }
                            break;
                        case 4: //rich mahogany
                            if (Tools.InfinitePlacement || Tools.ReduceItemStack(ItemID.RichMahogany))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.LivingMahogany);
                                newItem.SetDefaults(ItemID.RichMahogany);
                                Tools.MirrorPlacement(Player.tileTargetX, Player.tileTargetY, newItem.type);
                                tilePlaced = true;
                            }
                            break;
                        case 5: //living wood leaf
                            if (Tools.InfinitePlacement || Tools.ReduceItemStack(ItemID.RichMahogany))
                            {
                                WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, TileID.LivingMahoganyLeaves);
                                newItem.SetDefaults(ItemID.RichMahogany);
                                Tools.MirrorPlacement(Player.tileTargetX, Player.tileTargetY, newItem.type);
                                tilePlaced = true;
                            }
                            break;
                    }

                    if (tilePlaced && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);

                    return true;
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
                switch (MultiWandWheel.selectedIndex)
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

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LivingWoodWand);
            recipe.AddIngredient(ItemID.BoneWand);
            recipe.AddIngredient(ItemID.LeafWand);
            recipe.AddIngredient(ItemID.HiveWand);
            recipe.AddIngredient(ItemID.LivingMahoganyWand);
            recipe.AddIngredient(ItemID.LivingMahoganyLeafWand);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

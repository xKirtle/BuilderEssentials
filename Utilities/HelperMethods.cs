using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameInput;
using Terraria.Localization;
using BuilderEssentials.UI.UIStates;
using Terraria.Cinematics;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace BuilderEssentials.Utilities
{
    public static partial class HelperMethods
    {
        internal static bool ValidTilePlacement(int i, int j)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            if (mp.PlacementAnywhere) return true;

            int[] treeTypes =
            {
                //[TAG 1.4] Implements new trees
                TileID.Trees,
                TileID.PalmTree,
                TileID.PineTree,
                TileID.MushroomTrees,
                TileID.ChristmasTree,
            };

            Tile middle = Framing.GetTileSafely(i, j);
            Tile top = Framing.GetTileSafely(i, j - 1);
            Tile right = Framing.GetTileSafely(i + 1, j);
            Tile bottom = Framing.GetTileSafely(i, j + 1);
            Tile left = Framing.GetTileSafely(i - 1, j);

            return (!middle.active() || !Main.tileSolid[middle.type]) &&
                   !treeTypes.Contains(middle.type) &&
                   (
                       (top.active() && Main.tileSolid[top.type]) ||
                       (right.active() && Main.tileSolid[right.type]) ||
                       (bottom.active() && Main.tileSolid[bottom.type]) ||
                       (left.active() && Main.tileSolid[left.type]) ||
                       middle.wall != 0
                   );
        }

        internal static bool ToolHasRange(Point range)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            if (mp.InfinitePlacementRange || mp.InfinitePlayerRange) return true;

            Point playerCenter = Main.LocalPlayer.Center.ToTileCoordinates();
            //range = new Point(Main.screenWidth / 16, Main.screenHeight / 16);

            return Math.Abs(playerCenter.X - mp.PointedTileCoord.X) <= range.X &&
                   Math.Abs(playerCenter.Y - mp.PointedTileCoord.Y) <= range.Y;
        }

        internal static bool CanReduceItemStack(int itemType, int amount = 1, bool reduceStack = true)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            if (mp.InfinitePlacement) return true;
            
            bool reducingWand = MultiWand.wandTypes.Contains(itemType);
            int materialType = reducingWand ? MultiWand.wandMaterials[Array.IndexOf(MultiWand.wandTypes, itemType)] : 0;

            foreach (Item item in Main.LocalPlayer.inventory)
            {
                if ((!reducingWand && item.type == itemType && item.stack >= amount) ||
                    (reducingWand && item.type == materialType && item.stack >= amount))
                {
                    if (reduceStack)
                        item.stack -= amount;
                    
                    return true;
                }
            }
            
            return false;
        }

        internal static bool ValidTileCoordinates(int i, int j)
            => i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY;

        internal enum ItemTypes
        {
            Air,
            Tile,
            Wall
        }

        internal static int ToInt(this ItemTypes itemTypes) => (int) itemTypes;

        internal static ItemTypes WhatIsThisItem(int itemType)
        {
            //Settint item defaults with -1 will throw an IOOB
            if (itemType == -1) return ItemTypes.Air;

            Item item = new Item();
            item.SetDefaults(itemType);

            if (item.createTile != -1 && item.createWall == -1)
                return ItemTypes.Tile;
            else if (item.createTile == -1 && item.createWall != -1)
                return ItemTypes.Wall;
            else
                return ItemTypes.Air;
        }

        internal static Tile PlaceTile(int i, int j, int itemType, bool forced = false, bool sync = true,
            int placeStyle = -1)
            => PlaceTile(i, j, WhatIsThisItem(itemType), itemType, forced, sync, placeStyle);

        internal static Tile PlaceTile(int i, int j, ItemTypes itemTypes, int type, bool forced = false,
            bool sync = true, int placeStyle = -1)
        {
            //TODO: MAKE DIRECTIONAL TILES BE PLACED ACCORDING TO PLAYER DIRECTION

            if (type == 0 || !ValidTileCoordinates(i, j))
                return new Tile();

            Item item = new Item();
            item.SetDefaults(type);
            Tile tile = Framing.GetTileSafely(i, j);

            if ((itemTypes == HelperMethods.ItemTypes.Tile && tile.active() && tile.collisionType != -1) ||
                (itemTypes == HelperMethods.ItemTypes.Wall && tile.wall != 0) ||
                !CanReduceItemStack(type, reduceStack: false))
                return new Tile();

            if (forced && tile.collisionType == -1 && tile.type != TileID.DemonAltar && tile.type != 21)
                RemoveTile(i, j, dropItem: false);

            CanReduceItemStack(type); //We know it's true since it passed the condition above

            switch (itemTypes)
            {
                case ItemTypes.Air:
                    break;
                case ItemTypes.Tile:
                    if (placeStyle == -1)
                        placeStyle = item.placeStyle;
                    WorldGen.PlaceTile(i, j, item.createTile, forced: forced, style: placeStyle);
                    break;
                case ItemTypes.Wall:
                    WorldGen.PlaceWall(i, j, item.createWall);
                    break;
            }

            if (sync && Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, i, j, 1);

            //Attempts to mirror it in case there's a mirror wand selection
            HelperMethods.MirrorPlacement(i, j, item.type);

            //TODO: Compensate for vanilla's wall auto placement?

            return Framing.GetTileSafely(i, j);
        }

        internal static void PlaceTilesInArea(int startX, int startY, int endX, int endY, int itemType,
            bool forced = false)
        {
            //This whole method exists so I don't spam NetMessages to sync each tile individually but rather an area

            if (!ValidTileCoordinates(startX, startY) || !ValidTileCoordinates(endX, endY)) return;

            if (startX > endX)
            {
                int temp = startX;
                startX = endX;
                endX = temp;
            }

            if (startY > endY)
            {
                int temp = startY;
                startY = endY;
                endY = temp;
            }

            int horizontal = endX - startX;
            int vertical = endY - startY;

            for (int i = startX; i < startX + horizontal; i++)
            for (int j = startY; j < startY + vertical; j++)
                PlaceTile(i, j, itemType, forced, false);

            //Keeping syncSize an odd number since SendTileSquare as a bias towards up and left for even-numbers sizes
            int syncSize = horizontal > vertical ? horizontal : vertical;
            syncSize += 1 + (syncSize % 2);

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, startX, startY, syncSize);
        }

        internal static void RemoveTile(int i, int j, bool removeTile = true,
            bool removeWall = false, bool dropItem = true, int itemToDrop = -1, bool sync = true)
        {
            if (!ValidTileCoordinates(i, j)) return;

            int number = -1;
            if (dropItem)
            {
                Tile tile = Framing.GetTileSafely(i, j);

                //Default behaviour, can be laggy if doing a lot of iterations since PickItem is costly
                if (itemToDrop == -1)
                    itemToDrop = HelperMethods.PickItem(tile, false);

                if (itemToDrop == -1)
                    goto InvalidItemToDrop; //The selected tile doesn't have an associated item to spawn it 

                Item item = new Item();
                item.SetDefaults(itemToDrop);

                if (itemToDrop != -1 && (item.createTile == tile.type || item.createWall == tile.type) &&
                    tile.active() && Main.netMode == NetmodeID.MultiplayerClient)
                {
                    number = Item.NewItem(i * 16, j * 16, 16, 16, itemToDrop, 1, false, -1, false, false);
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number, 0f, 0f, 0f, 0, 0, 0);
                }
            }

            InvalidItemToDrop:
            if (removeTile)
                WorldGen.KillTile(i, j, !dropItem);

            if (removeWall)
                WorldGen.KillWall(i, j);

            if (sync && Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, i, j, 1); //syncs whether the tile is there or not
        }

        internal static void RemoveTilesInArea(int startX, int startY, int endX, int endY,
            bool dropItem = true, int itemToDrop = -1)
        {
            //This whole method exists so I don't spam NetMessages to sync each tile individually but rather an area

            if (startX > endX)
            {
                int temp = startX;
                startX = endX;
                endX = temp;
            }

            if (startY > endY)
            {
                int temp = startY;
                startY = endY;
                endY = temp;
            }

            int horizontal = endX - startX;
            int vertical = endY - startY;

            List<Point16> multiTileCoords = new List<Point16>();
            for (int i = startX; i < startX + horizontal; i++)
            for (int j = startY; j < startY + vertical; j++)
            {
                if (multiTileCoords.Contains(new Point16(i, j))) continue;

                //Check if multitile, and if yes, add it to a list of blacklisted coords
                Tile tile = Framing.GetTileSafely(i, j);
                TileObjectData data = TileObjectData.GetTileData(tile);

                //If data != null, is multi tile. Prevents more than one drop per multi tile
                if (data != null)
                {
                    Point16 topLeft = new Point16(i, j) - data.Origin;
                    for (int k = 0; k < data.CoordinateFullWidth / 18; k++)
                    for (int l = 0; l < data.CoordinateFullHeight / 18; l++)
                        multiTileCoords.Add(new Point16(k, l));
                }

                Item item = new Item();
                item.SetDefaults(itemToDrop);

                bool removeTile = tile.type == item.createTile;
                bool removeWall = tile.wall == item.createWall;
                RemoveTile(i, j, removeTile, removeWall, dropItem, itemToDrop, false);
            }

            //Keeping syncSize an odd number since SendTileSquare has a bias towards up and left for even-numbers sizes
            int syncSize = horizontal > vertical ? horizontal : vertical;
            syncSize += 1 + (syncSize % 2);

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, startX, startY, syncSize);
        }

        internal static void ChangeTileFraming(int i, int j, bool alternate)
        {
            //TODO: Statues are moving one tile to the right?
            
            if (!ValidTileCoordinates(i, j)) return;
            Tile tile = Framing.GetTileSafely(i, j);
            TileObjectData data = TileObjectData.GetTileData(tile);
            
            int[] directionFraming = new int[]
            { TileID.Chairs, TileID.Bathtubs, TileID.Beds, TileID.Mannequin, TileID.Womannequin };
            if (!directionFraming.Contains(tile.type) || data == null) return;

            Vector2 topLeft = new Vector2(Player.tileTargetX, Player.tileTargetY) - data.Origin.ToVector2();
            int fullWidth = data.CoordinateFullWidth / 18;
            int fullHeight = data.CoordinateFullHeight / 18;
            int style = TileObjectData.GetTileStyle(tile);
            
            int magicNumber = 0;
            if (tile.type == TileID.Beds || tile.type == TileID.Bathtubs)
                magicNumber = 36;
            else if (tile.type == TileID.Mannequin || tile.type == TileID.Womannequin)
                magicNumber = 54;
            else if (tile.type == TileID.Chairs)
                magicNumber = 40;
            
            for (int k = 0; k < fullWidth; k++)
            {
                for (int l = 0; l < fullHeight; l++)
                {
                    Tile tempTile = Framing.GetTileSafely((int) (topLeft.X + k), (int) (topLeft.Y + l));
                    tempTile.frameX = (short) (18 * fullWidth * Convert.ToInt32(alternate) + 18 * k);
                    tempTile.frameY = (short) (magicNumber * style + l * 18);
                    Main.tile[(int) (topLeft.X + k), (int) (topLeft.Y + l)] = tempTile;
                }
            }
        }

        //Taken from https://github.com/hamstar0/tml-hamstarhelpers-mod/blob/master/HamstarHelpers/Helpers/UI/UIHelpers.cs#L59
        internal static bool IsUIAvailable(
            bool notTabbedAway = true,
            bool gameNotPaused = true,
            bool mouseNotInUse = true,
            bool keyboardNotInVanillaUI = true,
            bool keyboardNotInCustomUI = true,
            bool playerAvailable = true,
            bool playerNotWieldingItem = true,
            bool playerNotTalkingToNPC = true,
            bool noFullscreenMap = true,
            bool notShowingMouseIcon = true)
        {
            Player player = Main.LocalPlayer;
            // if( !(!notTabbedAway || Main.hasFocus) ) { Main.NewText( "hasFocus" ); }
            // if( !(!gameNotPaused || !Main.gamePaused) ) { Main.NewText( "gamePaused" ); }
            // if( !(!mouseNotInUse || !player.mouseInterface) ) { Main.NewText( "mouseInterface" ); }
            // if( !(!keyboardNotInVanillaUI || !Main.drawingPlayerChat) ) { Main.NewText( "drawingPlayerChat" ); }
            // if( !(!keyboardNotInVanillaUI || !Main.editSign) ) { Main.NewText( "editSign" ); }
            // if( !(!keyboardNotInVanillaUI || !Main.editChest) ) { Main.NewText( "editChest" ); }
            // if( !(!keyboardNotInCustomUI || !PlayerInput.WritingText) ) { Main.NewText( "WritingText" ); }
            // if( !(!keyboardNotInCustomUI || !Main.blockInput) ) { Main.NewText( "blockInput" ); }
            // if( !(!playerAvailable || !player.dead) ) { Main.NewText( "dead" ); }
            // if( !(!playerAvailable || !player.CCed) ) { Main.NewText( "CCed" ); }
            // if( !(!playerNotTalkingToNPC || player.talkNPC == -1) ) { Main.NewText( "talkNPC" ); }
            // if( !(!playerNotWieldingItem || (player.itemTime == 0 && player.itemAnimation == 0)) ) { Main.NewText( "itemTime" ); }
            // if( !(!noFullscreenMap || !Main.mapFullscreen) ) { Main.NewText( "mapFullscreen" ); }
            // if( !(!notShowingMouseIcon || !Main.HoveringOverAnNPC) ) { Main.NewText( "HoveringOverAnNPC" ); }
            // if( !(!notShowingMouseIcon || !player.showItemIcon) ) { Main.NewText( "showItemIcon" ); }
            return (!notTabbedAway || Main.hasFocus) &&
                   (!gameNotPaused || !Main.gamePaused) &&
                   (!gameNotPaused || !Main.ingameOptionsWindow) &&
                   (!mouseNotInUse || !player.mouseInterface) &&
                   (!keyboardNotInVanillaUI || !Main.drawingPlayerChat) &&
                   (!keyboardNotInVanillaUI || !Main.editSign) &&
                   (!keyboardNotInVanillaUI || !Main.editChest) &&
                   (!keyboardNotInCustomUI || !PlayerInput.WritingText) &&
                   (!keyboardNotInCustomUI || !Main.blockInput) &&
                   (!playerAvailable || !player.dead) &&
                   (!playerAvailable || !player.CCed) &&
                   (!playerNotTalkingToNPC || player.talkNPC == -1) &&
                   (!playerNotWieldingItem || (player.itemTime == 0 && player.itemAnimation == 0)) &&
                   (!noFullscreenMap || !Main.mapFullscreen) &&
                   (!notShowingMouseIcon || !Main.HoveringOverAnNPC) &&
                   (!notShowingMouseIcon || !player.showItemIcon);
        }

        internal static void CreateRecipeGroup(int[] items, string text)
        {
            RecipeGroup recipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + text, items);
            RecipeGroup.RegisterGroup("BuilderEssentials:" + text, recipeGroup);
        }

        internal static void ChangeSlope(int slopeType)
        {
            int i = Player.tileTargetX;
            int j = Player.tileTargetY;
            Tile tile = Framing.GetTileSafely(i, j);

            if ((slopeType + 1 == tile.slope() && !tile.halfBrick()) ||
                (slopeType == 4 && tile.halfBrick()) ||
                (slopeType == 5 && tile.slope() == 0 && !tile.halfBrick()))
                return;

            if (tile.type >= 0 && tile.active())
            {
                switch (slopeType)
                {
                    case 0: //Bottom Right Slope
                        tile.halfBrick(false);
                        tile.slope(1);
                        break;
                    case 1: //Bottom Left Slope
                        tile.halfBrick(false);
                        tile.slope(2);
                        break;
                    case 2: //Top Right Slope
                        tile.halfBrick(false);
                        tile.slope(3);
                        break;
                    case 3: //Top Left Slope
                        tile.halfBrick(false);
                        tile.slope(4);
                        break;
                    case 4: //Half Tile
                        tile.halfBrick(true);
                        tile.slope(0);
                        break;
                    case 5: //Full Tile
                        tile.halfBrick(false);
                        tile.slope(0);
                        break;
                    default:
                        break;
                }

                Main.PlaySound(SoundID.Dig);
                WorldGen.SquareTileFrame(i, j, false);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, i, j, 1);
            }
        }

        internal static int PaintItemTypeToIndex(int paintType)
        {
            //The outputed indexes are not the paint color byte values. For those just increment one.

            //[TAG 1.4] Implements new paints and changes item types

            if (paintType >= 1073 && paintType <= 1099)
                return paintType - 1073;
            else if (paintType >= 1966 && paintType <= 1968)
                return paintType - 1939;

            return -1; //it will never reach here
        }

        internal static int ColorByteToPaintItemType(byte color)
        {
            //[TAG 1.4] Implements new paints and changes item types

            if (color >= 1 && color <= 27)
                return (color - 1) + 1073;
            else if (color >= 28 && color <= 30)
                return (color - 1) + 1939;

            return -1; //it will never reach here
        }

        internal static void PaintTileOrWall(byte color, int selectedTool, bool infinitePaint = false)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            if (color < 0 || color > 30 || selectedTool == 2) return;
            bool needsSync = false;

            if (selectedTool == 0 && mp.PointedTile.active() && mp.PointedTile.color() != color)
            {
                if (infinitePaint || CanReduceItemStack(ColorByteToPaintItemType(color), reduceStack: true))
                {
                    mp.PointedTile.color(color);
                    needsSync = true;
                }
            }
            else if (selectedTool == 1 && mp.PointedTile.wall != 0 && mp.PointedTile.wallColor() != color)
            {
                if (infinitePaint || CanReduceItemStack(ColorByteToPaintItemType(color), reduceStack: true))
                {
                    mp.PointedTile.wallColor(color);
                    needsSync = true;
                }
            }

            if (needsSync && Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
        }

        internal static void ScrapPaint()
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            bool needsSync = false;

            if (mp.PointedTile.color() != 0)
            {
                mp.PointedTile.color(0);
                needsSync = true;
            }

            if (mp.PointedTile.wallColor() != 0)
            {
                mp.PointedTile.wallColor(0);
                needsSync = true;
            }

            if (needsSync && Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.PaintTile, number: Player.tileTargetX, number2: Player.tileTargetY);
                NetMessage.SendData(MessageID.PaintWall, number: Player.tileTargetX, number2: Player.tileTargetY);
            }
        }

        public enum WrenchUpgrade
        {
            FastPlacement,
            InfPlacementRange,
            InfPlayerRange,
            PlacementAnywhere,
            InfPlacement,
            InfPickupRange,

            UpgradesCount
        }

        public static int ToInt(this WrenchUpgrade wrenchUpgrade) => (int) wrenchUpgrade;

        internal static bool IsWithinRange(float number, float value1, float value2, bool equal = false)
        {
            if (!equal)
                return (number > value1 && number < value2) || (number < value1 && number > value2);
            else
                return (number >= value1 && number <= value2) || (number <= value1 && number >= value2);
        }
    }
}
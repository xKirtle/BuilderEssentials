using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameInput;
using Terraria.Localization;
using BuilderEssentials.UI.UIStates;

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
            if (mp.InfinitePlacementRange) return true;

            Point playerCenter = Main.LocalPlayer.Center.ToTileCoordinates();
            //range = new Point(Main.screenWidth / 16, Main.screenHeight / 16);

            return Math.Abs(playerCenter.X - mp.PointedTileCoord.X) <= range.X &&
                   Math.Abs(playerCenter.Y - mp.PointedTileCoord.Y) <= range.Y;
        }

        internal static bool CanReduceItemStack(int itemType, bool reduceStack = true)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            if (mp.InfinitePlacement) return true;

            foreach (Item item in Main.LocalPlayer.inventory)
            {
                if (item.type == itemType && item.stack >= 1)
                {
                    if (reduceStack) --item.stack;
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

        internal static Tile PlaceTile(int i, int j, int itemType)
        {
            if (itemType == -1 || !ValidTileCoordinates(i, j))
                return new Tile();

            ItemTypes itemTypes = WhatIsThisItem(itemType);
            Item item = new Item();
            item.SetDefaults(itemType);

            switch (itemTypes)
            {
                case ItemTypes.Air:
                    break;
                case ItemTypes.Tile:
                    WorldGen.PlaceTile(i, j, item.createTile, style: item.placeStyle);
                    break;
                case ItemTypes.Wall:
                    WorldGen.PlaceWall(i, j, item.createWall);
                    break;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, i, j, 1);

            return Framing.GetTileSafely(i, j);
        }

        internal static Tile PlaceTile(int i, int j, ItemTypes itemTypes, int type)
        {
            if (type == -1 || !ValidTileCoordinates(i, j))
                return new Tile();

            switch (itemTypes)
            {
                case ItemTypes.Air:
                    break;
                case ItemTypes.Tile:
                    WorldGen.PlaceTile(i, j, type);
                    break;
                case ItemTypes.Wall:
                    WorldGen.PlaceWall(i, j, type);
                    break;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, i, j, 1);

            return Framing.GetTileSafely(i, j);
        }

        internal static void RemoveTile(int i, int j, bool removeTile = true, bool removeWall = false,
            bool fail = false, bool dropItem = true)
        {
            if (!ValidTileCoordinates(i, j)) return;

            if (removeTile)
                WorldGen.KillTile(i, j, fail, !dropItem);

            if (removeWall)
                WorldGen.KillWall(i, j, fail);

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, i, j, 1);
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
                if (infinitePaint || CanReduceItemStack(ColorByteToPaintItemType(color), true))
                {
                    mp.PointedTile.color(color);
                    needsSync = true;
                }
            }
            else if (selectedTool == 1 && mp.PointedTile.wall != 0 && mp.PointedTile.wallColor() != color)
            {
                if (infinitePaint || CanReduceItemStack(ColorByteToPaintItemType(color), true))
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

            UpgradesCount
        }

        public static int ToInt(this WrenchUpgrade wrenchUpgrade) => (int) wrenchUpgrade;
    }
}
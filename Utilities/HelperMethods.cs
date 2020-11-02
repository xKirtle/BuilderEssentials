using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace BuilderEssentials.Utilities
{
    internal static partial class HelperMethods
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
            if (mp.InfiniteRange) return true;
            
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

        internal enum ItemTypes
        {
            Air,
            Tile,
            Wall
        }

        internal static int ToInt(this ItemTypes itemTypes) => (int) itemTypes;

        internal static ItemTypes WhatIsThisItem(int itemType)
        {
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
    }
}
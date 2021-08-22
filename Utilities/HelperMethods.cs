using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Utilities
{
    internal static partial class HelperMethods
    {
        //Quadratic bezier
        internal static float X(float t, float x0, float x1, float x2)
        {
            return (float) (
                x0 * Math.Pow(1 - t, 2) +
                x1 * 2 * t * (1 - t) +
                x2 * Math.Pow(t, 2)
            );
        }

        internal static float Y(float t, float y0, float y1, float y2)
        {
            return (float) (
                y0 * Math.Pow(1 - t, 2) +
                y1 * 2 * t * (1 - t) +
                y2 * Math.Pow(t, 2)
            );
        }

        internal static Vector2 TraverseBezier(Vector2 startPoint, Vector2 controlPoint, Vector2 endPoint, float t)
        {
            float x = X(t, startPoint.X, controlPoint.X, endPoint.X);
            float y = Y(t, startPoint.Y, controlPoint.Y, endPoint.Y);
            return new Vector2(x, y);
        }

        //Cubic bezier
        internal static float X(float t, float x0, float x1, float x2, float x3)
        {
            return (float) (
                x0 * Math.Pow((1 - t), 3) +
                x1 * 3 * t * Math.Pow((1 - t), 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }

        internal static float Y(float t, float y0, float y1, float y2, float y3)
        {
            return (float) (
                y0 * Math.Pow((1 - t), 3) +
                y1 * 3 * t * Math.Pow((1 - t), 2) +
                y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                y3 * Math.Pow(t, 3)
            );
        }

        internal static Vector2 TraverseBezier(Vector2 startPoint, Vector2 controlPoint, Vector2 controlPoint2,
            Vector2 endPoint, float t)
        {
            float x = X(t, startPoint.X, controlPoint.X, controlPoint2.X, endPoint.X);
            float y = Y(t, startPoint.Y, controlPoint.Y, controlPoint2.Y, endPoint.Y);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns true if inside the bounds of the world
        /// </summary>
        /// <param name="i">X axis tile coordinates</param>
        /// <param name="j">Y axis tile coordinates</param>
        internal static bool ValidTileCoordinates(int i, int j)
            => i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY;

        /// <param name="items">Array of itemID's to include in the recipe group</param>
        /// <param name="text">Name of the recipe group. Used to reference it when calling it in a recipe</param>
        internal static void CreateRecipeGroup(int[] items, string text)
        {
            RecipeGroup recipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + text, items);
            RecipeGroup.RegisterGroup("BuilderEssentials:" + text, recipeGroup);
        }

        /// <param name="layerToInsert">Name of the layer you want your new interface to be below of</param>
        /// <param name="layerName">Name used to find the layer that will be created in the list of layers</param>
        /// <param name="gameTime">Current game tick</param>
        /// <param name="userInterface">Instance of the userInterface object</param>
        /// <param name="scaleType">Whether UI will scale based on UI Scale, Zoom or nothing</param>
        internal static void InsertInterfaceLayer(ref List<GameInterfaceLayer> layers, string layerToInsert,
            string layerName,
            GameTime gameTime, UserInterface userInterface, InterfaceScaleType scaleType)
        {
            int interfaceLayer = layers.FindIndex(layer => layer.Name.Equals(layerToInsert));
            if (interfaceLayer != -1)
            {
                layers.Insert(interfaceLayer, new LegacyGameInterfaceLayer(layerName,
                    delegate
                    {
                        if (gameTime != null && userInterface?.CurrentState != null)
                            userInterface.Draw(Main.spriteBatch, gameTime);

                        return true;
                    },
                    scaleType));
            }
        }

        /// <summary>
        /// Returns true if cursor is within range of the tool
        /// </summary>
        /// <param name="range">Horizontal and vertical range from the center of the player (2 tiles above ground)</param>
        /// <returns></returns>
        internal static bool ToolHasRange(Vector2 range)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            if (mp.InfinitePlayerRange) return true;

            Point playerCenter = Main.LocalPlayer.Center.ToTileCoordinates();
            //range = new Point(Main.screenWidth / 16, Main.screenHeight / 16);

            return Math.Abs(playerCenter.X - mp.PointedTileCoord.X) <= range.X &&
                   Math.Abs(playerCenter.Y - mp.PointedTileCoord.Y) <= range.Y;
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
                   (!notShowingMouseIcon || !player.cursorItemIconEnabled);
        }

        /// <summary>
        /// Modify the slope of the tile in the given coordinates
        /// </summary>
        /// <param name="i">X axis tile coordinates</param>
        /// <param name="j">Y axis tile coordinates</param>
        /// <param name="slopeType">New SlopeType of the tile</param>
        /// <param name="IsHalfBlock">Whether tile will be a half block</param>
        internal static void ChangeSlope(int i, int j, SlopeType slopeType, bool IsHalfBlock = false)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            if (tile != null && Main.tileSolid[tile.type] && tile.type >= 0 && tile.IsActive)
            {
                //If there are no changes, return so it doesn't play feedback sound and tries to sync unnecessary info
                if (tile.Slope == slopeType && tile.IsHalfBlock == IsHalfBlock)
                    return;

                tile.Slope = slopeType;
                tile.IsHalfBlock = slopeType == SlopeType.Solid ? IsHalfBlock : false;

                SoundEngine.PlaySound(SoundID.Dig);
                WorldGen.SquareTileFrame(i, j, false);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, i, j, 1);
            }
        }

        //Simple wrapper to keep me sane
        internal static Asset<Texture2D> RequestTexture(string path,
            AssetRequestMode requestMode = AssetRequestMode.ImmediateLoad) =>
            ModContent.Request<Texture2D>(path, requestMode);

        /// <summary>
        /// Sets the player range
        /// </summary>
        /// <param name="range">Horizontal and vertical range from the center of the player (2 tiles above ground)</param>
        internal static void SetPlayerRange(int tileX, int tileY, int blockRange)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();

            Player.tileRangeX = tileX;
            Player.tileRangeY = tileY;
            mp.Player.blockRange = blockRange;
        }
        
        /// <summary>
        /// Returns true if it is a valid tile placement position
        /// </summary>
        /// <param name="i">X axis tile coordinates</param>
        /// <param name="j">Y axis tile coordinates</param>
        internal static bool ValidTilePlacement(int i, int j)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            if (mp.PlacementAnywhere) return true;

            int[] treeTypes =
            {
                TileID.Trees, TileID.PalmTree, TileID.PineTree, TileID.MushroomTrees,
                TileID.ChristmasTree, TileID.TreeAmber, TileID.TreeAmethyst, TileID.TreeDiamond,
                TileID.TreeEmerald, TileID.TreeRuby, TileID.TreeSapphire, TileID.TreeTopaz,
                TileID.TreeNymphButterflyJar, TileID.VanityTreeSakura, TileID.VanityTreeSakuraSaplings,
                TileID.VanityTreeYellowWillow, TileID.VanityTreeWillowSaplings
            };

            Tile middle = Framing.GetTileSafely(i, j);
            Tile top = Framing.GetTileSafely(i, j - 1);
            Tile right = Framing.GetTileSafely(i + 1, j);
            Tile bottom = Framing.GetTileSafely(i, j + 1);
            Tile left = Framing.GetTileSafely(i - 1, j);
            
            //TODO: Break this return in some meaningful bools
            return (!middle.IsActive || !Main.tileSolid[middle.type]) && 
                   !treeTypes.Contains(middle.type) &&
                   (
                       (top.IsActive && Main.tileSolid[top.type]) ||
                       (right.IsActive && Main.tileSolid[right.type]) ||
                       (bottom.IsActive && Main.tileSolid[bottom.type]) ||
                       (left.IsActive && Main.tileSolid[left.type]) ||
                       middle.wall != 0
                   );
        }

        /// <summary>
        /// Returns true if a given itemType can be reduced by a given amount
        /// </summary>
        /// <param name="itemType">Item type of the item to be reduced</param>
        /// <param name="amount">Amount to be reduced by</param>
        /// <param name="reduceStack">Whether it actually reduces the stack or just checks if it's possible</param>
        /// <param name="itemShouldBeInHand">Checks HeldItem instead of going through the whole inventory first</param>
        internal static bool CanReduceItemStack(int itemType, int amount = 1, bool reduceStack = true, bool itemShouldBeInHand = false)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            if (mp.InfinitePlacement) return true;

            //Checking heldItem first before looping through the inventory
            //sometimes we might know it has to be in hand (like a tile being placed..)
            if (itemShouldBeInHand)
            {
                if (Main.LocalPlayer.HeldItem.type == itemType && Main.LocalPlayer.HeldItem.stack >= amount)
                {
                    if (reduceStack)
                        Main.LocalPlayer.HeldItem.stack -= amount;
                    return true;
                }
            }

            foreach (Item item in Main.LocalPlayer.inventory)
            {
                if (item.type == itemType && item.stack >= amount)
                {
                    if (reduceStack)
                        item.stack -= amount;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if tile in given coordinates can be broken when trying to place a new tile in the same position
        /// </summary>
        /// <param name="i">X axis tile coordinates</param>
        /// <param name="j">Y axis tile coordinates</param>
        internal static bool CanBreakToPlaceTile(int i, int j)
        {
            //Wasn't able to get a dynamic solution working making use of
            //collisionType, whether tile is (not) active or Main.tileSolid..
            //afaik there's no array that vanilla uses for this thing
            //everything's hardcoded in the WorldGen.PlaceTile() method
            
            Tile tile = Framing.GetTileSafely(i, j);
            int[] breakableTilesWithoutCollision =
            {
                TileID.Plants, TileID.Plants2, TileID.LongMoss, TileID.SmallPiles,
                TileID.LargePiles, TileID.LargePiles2, TileID.MushroomPlants, TileID.SeaOats, 
                TileID.OasisPlants, TileID.Pots, TileID.Stalactite, TileID.CorruptThorns,
                TileID.CorruptPlants, TileID.CrimsonThorns, TileID.CrimsonPlants,
                TileID.CrimsonVines, TileID.JungleThorns, TileID.JunglePlants, 
                TileID.JunglePlants2, TileID.JungleVines, TileID.Vines, /*VineFlowers?*/
                TileID.PlantDetritus, TileID.LilyPad, TileID.BeeHive, TileID.Cobweb,
                TileID.BeachPiles, 
            };

            return breakableTilesWithoutCollision.Contains(tile.type);
        }
        
        /*------------------------------------------------------------------------------------------------------------*/
        internal static int PaintItemTypeToIndex(int paintType)
        {
            //The outputed indexes are not the paint color byte values. For those just increment one.

            if (paintType >= 1073 && paintType <= 1099)
                return paintType - 1073;
            else if (paintType >= 1966 && paintType <= 1968)
                return paintType - 1939;
            else if (paintType >= 4668)
                return paintType - 4638;

            return -1; //it will never reach here
        }

        internal static int ColorByteToPaintItemType(byte color)
        {
            if (color >= 1 && color <= 27)
                return (color - 1) + 1073;
            else if (color >= 28 && color <= 30)
                return (color - 1) + 1939;
            else if (color == 31)
                return (color - 1) + 4638;

            return -1; //it will never reach here
        }
        
        internal static void PaintTileOrWall(byte color, int selectedTool, Vector2 coords, bool infinitePaint = false)
        {
            //TODO: Add vanilla's painting particles? They're the color of the color being used
            
            Tile tile = Framing.GetTileSafely(coords);
            if (color < 0 || color > 32 || selectedTool == 2) return;
            bool needsSync = false;

            if (selectedTool == 0 && tile.IsActive && tile.Color != color)
            {
                if (infinitePaint || CanReduceItemStack(ColorByteToPaintItemType(color), reduceStack: true))
                {
                    tile.Color = color;
                    needsSync = true;
                }
            }
            else if (selectedTool == 1 && tile.wall != 0 && tile.WallColor != color)
            {
                if (infinitePaint || CanReduceItemStack(ColorByteToPaintItemType(color), reduceStack: true))
                {
                    tile.WallColor = color;
                    needsSync = true;
                }
            }

            if (needsSync && Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, (int)coords.X, (int)coords.Y, 1);
        }

        internal static void ScrapPaint(Vector2 coords)
        {
            Tile tile = Framing.GetTileSafely(coords);
            bool needsSync = false;

            if (tile.Color != 0)
            {
                tile.Color = 0;
                needsSync = true;
            }

            if (tile.WallColor != 0)
            {
                tile.WallColor = 0;
                needsSync = true;
            }

            if (needsSync && Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.PaintTile, number: (int)coords.X, number2: (int)coords.Y);
                NetMessage.SendData(MessageID.PaintWall, number: (int)coords.X, number2: (int)coords.Y);
            }
        }
    }
}
using System;
using System.Collections.Generic;
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

        internal static void ChangeSlope(SlopeType slopeType, bool IsHalfBlock = false)
        {
            int i = Player.tileTargetX;
            int j = Player.tileTargetY;
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
    }
}
using System;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria;

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
            return (float)(
                x0 * Math.Pow((1 - t), 3) +
                x1 * 3 * t * Math.Pow((1 - t), 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }
        internal static float Y(float t, float y0, float y1, float y2, float y3)
        {
            return (float)(
                y0 * Math.Pow((1 - t), 3) +
                y1 * 3 * t * Math.Pow((1 - t), 2) +
                y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                y3 * Math.Pow(t, 3)
            );
        }
        
        internal static Vector2 TraverseBezier(Vector2 startPoint, Vector2 controlPoint, Vector2 controlPoint2, Vector2 endPoint, float t)
        {
            float x = X(t, startPoint.X, controlPoint.X, controlPoint2.X, endPoint.X);
            float y = Y(t, startPoint.Y, controlPoint.Y, controlPoint2.Y, endPoint.Y);
            return new Vector2(x, y);
        }
        
        internal static bool ValidTileCoordinates(int i, int j)
            => i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY;
        
        internal static void CreateRecipeGroup(int[] items, string text)
        {
            RecipeGroup recipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + text, items);
            RecipeGroup.RegisterGroup("BuilderEssentials:" + text, recipeGroup);
        }
    }
}
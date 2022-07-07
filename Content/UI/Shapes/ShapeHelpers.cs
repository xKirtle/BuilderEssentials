using System;
using System.Collections.Generic;
using BuilderEssentials.Common.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace BuilderEssentials.Content.UI;

public static class ShapeHelpers
{
    public static Color Blue = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
    public static Color Yellow = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f;
    public static Color Red = new Color(1f, 0f, 0f, .75f) * 0.8f;
    
    
    private static Asset<Texture2D> PixelTexture = TextureAssets.Extra[2];
    private static Rectangle Rectangle = new Rectangle(0, 0, 16, 16);

    public static void PlotPixel(int tileX, int tileY, Color color, HashSet<Vector2> visitedCoords, float scale = 1f) {
        Vector2 position = Main.ReverseGravitySupport(new Vector2(tileX, tileY) * 16 - Main.screenPosition);
        if (visitedCoords?.Add(position) ?? true) {
            position += new Vector2(8 - 8 * scale); //Make it not fix on the top left corner
            Main.spriteBatch.Draw(PixelTexture.Value, position, Rectangle,
                color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }

    //TODO: Make this chain of methods require an hashset to be passed as a ref, and if it is null, do nothing
    //if it exists, check if can add to it before drawing
    public static void PlotLine(Vector2 start, Vector2 end, Color color, HashSet<Vector2> visitedCoords, float scale = 1f)
        => PlotLine(start.X, start.Y, end.X, end.Y, color, visitedCoords, scale);
    public static void PlotLine(float x0, float y0, float x1, float y1, Color color, HashSet<Vector2> visitedCoords, float scale = 1f) 
        => PlotLine((int) x0, (int) y0, (int) x1, (int) y1, color, visitedCoords, scale);
    public static void PlotLine(int x0, int y0, int x1, int y1, Color color, HashSet<Vector2> visitedCoords, float scale = 1f) {
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;
        while (true) {
            PlotPixel(x0, y0, color, visitedCoords, scale);
            e2 = 2 * err;
            if (e2 >= dy) {
                if (x0 == x1) break;
                err += dy;
                x0 += sx;
            }

            if (e2 <= dx) {
                if (y0 == y1) break;
                err += dx;
                y0 += sy;
            }
        }
    }

    public static void PlotRectangle(Vector2 start, Vector2 end, Color color, HashSet<Vector2> visitedCoords,
        float scale = 1f, bool isFill = false, bool displaySize = true)
        => PlotRectangle(start.X, start.Y, end.X, end.Y, color, visitedCoords, scale, isFill, displaySize);

    public static void PlotRectangle(float x0, float y0, float x1, float y1, Color color, HashSet<Vector2> visitedCoords,
        float scale = 1f, bool isFill = false, bool displaySize = true)
        => PlotRectangle((int) x0, (int) y0, (int) x1, (int) y1, color, visitedCoords, scale, isFill, displaySize);

    public static void PlotRectangle(int x0, int y0, int x1, int y1, Color color, HashSet<Vector2> visitedCoords,
        float scale = 1f, bool isFill = false, bool displaySize = true) {
        int dx = Math.Abs(x0 - x1);
        int dy = Math.Abs(y0 - y1);
        int minX = Math.Min(x0, x1), maxX = Math.Max(x0, x1);
        int minY = Math.Min(y0, y1), maxY = Math.Max(y0, y1);

        if (dx == 0 && dy == 0) return;

        if ((dx == 0 && dy != 0) || (dx != 0 && dy == 0)) {
            PlotLine(x0, y0, x1, y1, color, visitedCoords, scale);
            return;
        }

        for (int x = minX; x <= minX + dx; x++) {
            PlotPixel(x, minY, color, visitedCoords, scale);
            PlotPixel(x, minY + dy, color, visitedCoords, scale);
        }

        for (int y = minY + 1; y <= minY + dy - 1; y++) {
            PlotPixel(minX , y, color, visitedCoords, scale);
            PlotPixel(minX + dx, y, color, visitedCoords, scale);
        }

        if (isFill) {
            for (int x = minX + 1; x <= minX + dx - 1; x++)
            for (int y = minY + 1; y <= minY + dy - 1; y++)
                PlotPixel(x, y, color, visitedCoords, scale);
        }

        //Middle helper lines
        Color tempColor = color;
        color *= 0.2f;

        //Vertical line
        if (dx % 2 == 0 && dx > 3) {
            int fixedX = minX + dx / 2;
            PlotLine(fixedX, minY, fixedX, minY + dy, color, visitedCoords, scale);
        }
        
        //Horizontal line
        if (dy % 2 == 0 && dy > 3) {
            int fixedY = minY + dy / 2;
            PlotLine(minX, fixedY, minX + dx, fixedY, color, visitedCoords, scale);
        }
        
        color = tempColor;

        if (displaySize)
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value,$"{dx + 1}x{dy + 1}",
            new Vector2(maxX, maxY) * 16 - Main.screenPosition + new Vector2(18f, 18f), Blue * 1.25f, 0f, Vector2.Zero, Vector2.One);
    }

#region Quadratic Bezier
    public static void PlotBezier(float dt, Vector2 startPoint, Vector2 controlPoint, Vector2 endPoint, 
        Color color, HashSet<Vector2> visitedCoords, float scale = 1f) {
        List<Vector2> points = new List<Vector2>();
        for (float t = 0.0f; t < 1.0f; t += dt)
            points.Add(TraverseBezier(startPoint, controlPoint, endPoint, t));

        points.Add(TraverseBezier(startPoint, controlPoint, endPoint, 1.0f));

        for (int i = 0; i < points.Count - 1; i++)
            PlotLine(points[i], points[i+1], color, visitedCoords, scale);
    }
    
    private static float X(float t, float x0, float x1, float x2)
        => (float) ((x0 * Math.Pow(1 - t, 2)) + (x1 * 2 * t * (1 - t)) + (x2 * Math.Pow(t, 2)));

    private static float Y(float t, float y0, float y1, float y2)
        => (float) (y0 * Math.Pow(1 - t, 2) + (y1 * 2 * t * (1 - t)) + (y2 * Math.Pow(t, 2)));

    private static Vector2 TraverseBezier(Vector2 startPoint, Vector2 controlPoint, Vector2 endPoint, float t) {
        Vector2 newControlPoint = GetControlPointOfTheoricalControlPoint(startPoint, controlPoint, endPoint);
        float x = X(t, startPoint.X, newControlPoint.X, endPoint.X);
        float y = Y(t, startPoint.Y, newControlPoint.Y, endPoint.Y);
        return new Vector2(x, y);
    }
    
    //Ellipses have their control points be rectangle and then call below method on each control point to get the real control point
    public static Vector2 GetControlPointOfTheoricalControlPoint(Vector2 startPoint, 
        Vector2 controlPoint, Vector2 endPoint) {
        float theoricalX = (4 * controlPoint.X - startPoint.X - endPoint.X) * 0.5f;
        float theoricalY = (4 * controlPoint.Y - startPoint.Y - endPoint.Y) * 0.5f;
        
        return new Vector2(theoricalX, theoricalY);
    }
#endregion

#region Cubic Bezier
    public static void PlotBezier(float dt, Vector2 startPoint, Vector2 controlPoint, Vector2 controlPoint2, 
        Vector2 endPoint, Color color, HashSet<Vector2> visitedCoords, float scale = 1f) {
        List<Vector2> points = new List<Vector2>();
        for (float t = 0.0f; t < 1.0f; t += dt)
            points.Add(TraverseBezier(startPoint, controlPoint, controlPoint2, endPoint, t));

        points.Add(TraverseBezier(startPoint, controlPoint, controlPoint2, endPoint, 1.0f));

        for (int i = 0; i < points.Count - 1; i++)
            PlotLine(points[i], points[i+1], color, visitedCoords, scale);
    }
    
    private static float X(float t, float x0, float x1, float x2, float x3)
        => (float) ((x0 * Math.Pow((1 - t), 3)) + (x1 * 3 * t * Math.Pow((1 - t), 2)) +
                    (x2 * 3 * Math.Pow(t, 2) * (1 - t)) + (x3 * Math.Pow(t, 3)));

    private static float Y(float t, float y0, float y1, float y2, float y3)
        => (float) ((y0 * Math.Pow((1 - t), 3)) + (y1 * 3 * t * Math.Pow((1 - t), 2)) +
                    (y2 * 3 * Math.Pow(t, 2) * (1 - t)) + (y3 * Math.Pow(t, 3)));

    private static Vector2 TraverseBezier(Vector2 startPoint, Vector2 controlPoint, 
        Vector2 controlPoint2, Vector2 endPoint, float t) {
        float x = X(t, startPoint.X, controlPoint.X, controlPoint2.X, endPoint.X);
        float y = Y(t, startPoint.Y, controlPoint.Y, controlPoint2.Y, endPoint.Y);
        return new Vector2(x, y);
    }
#endregion

    public static void PlotEllipse(Vector2 start, Vector2 end, Color color, HashSet<Vector2> visitedCoords, 
        ShapeSide shape = ShapeSide.All, float scale = 1f, bool isFill = false)
        => PlotEllipse(start.X, start.Y, end.X, end.Y, color, visitedCoords, shape, scale, isFill);

    public static void PlotEllipse(float x0, float y0, float x1, float y1, Color color, HashSet<Vector2> visitedCoords, 
        ShapeSide shape = ShapeSide.All, float scale = 1f, bool isFill = false)
        => PlotEllipse((int) x0, (int) y0, (int) x1, (int) y1, color, visitedCoords, shape, scale, isFill);

    //TODO: Make half shapes be drawn with beziers instead? start/end in corners, control points in opposite corners
    public static void PlotEllipse(int x0, int y0, int x1, int y1, Color color, HashSet<Vector2> visitedCoords,
        ShapeSide shape = ShapeSide.All, float scale = 1f, bool isFill = false) {
        int width = Math.Abs(x0 - x1);
        int height = Math.Abs(y0 - y1);
        if (width == 0 && height == 0) return;

        if ((shape & ShapeSide.All) == 0) {
            if ((shape & ShapeSide.Left) != 0)
                x0 += width;
            if ((shape & ShapeSide.Bottom) != 0)
                y0 += height;
            if ((shape & ShapeSide.Right) != 0)
                x0 -= width;
            if ((shape & ShapeSide.Top) != 0)
                y0 -= height;
        }
        
        int a = Math.Abs(x1-x0), b = Math.Abs(y1-y0), b1 = b&1;
        long dx = 4*(1-a)*b*b, dy = 4*(b1+1)*a*a;
        long err = dx+dy+b1*a*a, e2;
        
        if (x0 > x1) { x0 = x1; x1 += a; }
        if (y0 > y1) y0 = y1;
        y0 += (b+1)/2; y1 = y0-b1;
        a *= 8*a; b1 = 8*b*b;

        do {
            bool quadOne = false, quadTwo = false, quadThree = false, quadFour = false;
            
            if ((shape & ShapeSide.Top) != 0) {
                PlotPixel(x1, y0, color, visitedCoords, scale); //   I. Quadrant
                PlotPixel(x0, y0, color, visitedCoords, scale); //  II. Quadrant
                quadOne = quadTwo = true;
            }

            if ((shape & ShapeSide.Right) != 0) {
                if (!quadOne)
                    PlotPixel(x1, y0, color, visitedCoords, scale); //   I. Quadrant
                PlotPixel(x1, y1, color, visitedCoords, scale); //  IV. Quadrant
                quadOne = quadFour = true;
            }

            if ((shape & ShapeSide.Bottom) != 0) {
                PlotPixel(x0, y1, color, visitedCoords, scale); // III. Quadrant
                if (!quadFour)
                    PlotPixel(x1, y1, color, visitedCoords, scale); //  IV. Quadrant
                quadThree = quadFour = true;
            }
            
            if ((shape & ShapeSide.Left) != 0) {
                if (!quadTwo)
                    PlotPixel(x0, y0, color, visitedCoords, scale); //  II. Quadrant
                if (!quadThree)
                    PlotPixel(x0, y1, color, visitedCoords, scale); // III. Quadrant
                quadTwo = quadThree = true;
            }

            if (isFill) {
                //No half shape selected
                if ((shape & ShapeSide.All) != 0) {
                    //Horizontal
                    PlotLine(x0, y0, x1, y0, color, visitedCoords, scale);
                    PlotLine(x0, y1, x1, y1, color, visitedCoords, scale);
                    //Vertical
                    PlotLine(x0, y0, x0, y1, color, visitedCoords, scale);
                    PlotLine(x1, y0, x1, y1, color, visitedCoords, scale);
                    //Diagonals (sometimes a few blocks are left to be placed and this fills the remaining)
                    PlotLine(x0, y0, x1, y1, color, visitedCoords, scale);
                    PlotLine(x0, y1, x1, y0, color, visitedCoords, scale);
                    goto end;
                }
                
                // Half Shapes
                if ((shape & ShapeSide.Bottom) != 0 || (shape & ShapeSide.Top) != 0) {
                    int tempY = (shape & ShapeSide.Bottom) != 0 ? y0 : y1;
                    bool condition = (shape & ShapeSide.Bottom) != 0 ? tempY < y1 : tempY > y1;

                    do {
                        PlotLine(x0, tempY, x1, tempY, color, visitedCoords, scale);
                        tempY += condition ? 1 : -1;
                    } while (condition);
                }
                else if ((shape & ShapeSide.Left) != 0 || (shape & ShapeSide.Right) != 0) {
                    int tempX = (shape & ShapeSide.Left) != 0 ? x0 : x1;
                    bool condition = (shape & ShapeSide.Left) != 0 ? tempX > x1 : tempX < x1;

                    do {
                        PlotLine(tempX, y0, tempX, y1, color, visitedCoords, scale);
                        tempX += condition ? -1 : 1;
                    } while (condition);
                }
            }
            
            end:
            e2 = 2*err;
            if (e2 <= dy) { y0++; y1--; err += dy += a; }
            if (e2 >= dx || 2*err > dy) { x0++; x1--; err += dx += b1; }
        } while (x0 <= x1);

        while (y0-y1 < b) {
            PlotPixel(x0-1, y0, color, visitedCoords, scale);
            PlotPixel(x1+1, y0++, color, visitedCoords, scale);
            PlotPixel(x0-1, y1, color, visitedCoords, scale);
            PlotPixel(x1+1, y1--, color, visitedCoords, scale);
        }
    }
}
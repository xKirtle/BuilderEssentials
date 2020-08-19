using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace BuilderEssentials.UI.ShapesDrawing
{
    public class CircleShape : UIElement
    {
        ShapesMenu sm;
        public override void OnInitialize() => sm = ShapesMenu.Instance;

        //Taken from http://members.chello.at/easyfilter/bresenham.html. All credits go to Alois Zingl
        #region Ellipse Algorithm
        void DrawEllipse(int x0, int y0, int x1, int y1, SpriteBatch spriteBatch)
        {
            int a = Math.Abs(x1 - x0), b = Math.Abs(y1 - y0), b1 = b & 1; //values of diameter
            long dx = 4 * (1 - a) * b * b, dy = 4 * (b1 + 1) * a * a; //error increment 
            long err = dx + dy + b1 * a * a, e2; //error of 1.step

            if (x0 > x1) { x0 = x1; x1 += a; } //if called with swapped points
            if (y0 > y1) y0 = y1; //exchange them
            y0 += (b + 1) / 2; y1 = y0 - b1;   //starting pixel
            a *= 8 * a; b1 = 8 * b * b;

            do
            {
                SetRectangle(x1, y0, spriteBatch); //   I. Quadrant
                SetRectangle(x0, y0, spriteBatch); //  II. Quadrant
                SetRectangle(x0, y1, spriteBatch); // III. Quadrant
                SetRectangle(x1, y1, spriteBatch); //  IV. Quadrant
                e2 = 2 * err;
                if (e2 <= dy) { y0++; y1--; err += dy += a; }  //y step
                if (e2 >= dx || 2 * err > dy) { x0++; x1--; err += dx += b1; } //x step
            } while (x0 <= x1);

            while (y0 - y1 < b)
            {  //too early stop of flat ellipses a=1
                SetRectangle(x0 - 1, y0, spriteBatch); //-> finish tip of ellipse
                SetRectangle(x1 + 1, y0++, spriteBatch);
                SetRectangle(x0 - 1, y1, spriteBatch);
                SetRectangle(x1 + 1, y1--, spriteBatch);
            }
        }
        #endregion

        void SetRectangle(int x, int y, SpriteBatch spriteBatch)
        {
            Texture2D texture = Main.extraTexture[2];
            Rectangle value = new Rectangle(0, 0, 16, 16);
            Color color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
            Vector2 position = new Vector2(x, y) * 16 - Main.screenPosition;

            spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        void EllipseToCircleCoordsFix()
        {
            int distanceX = (int)(sm.endDrag.X - sm.startDrag.X);
            int distanceY = (int)(sm.endDrag.Y - sm.startDrag.Y);

            //Turning rectangle (startDrag->endDrag) into a square
            if (Math.Abs(distanceX) > Math.Abs(distanceY)) //Horizontal Ellipse
            {
                if (distanceX > 0) //I. and IV. Quadrant
                    sm.endDrag.X = sm.startDrag.X + Math.Abs(distanceY);
                else //II. and III. Quadrant
                    sm.endDrag.X = sm.startDrag.X - Math.Abs(distanceY);
            }
            else //Vertical Ellipse
            {
                if (distanceY > 0) //III. and IV. Quadrant
                    sm.endDrag.Y = sm.startDrag.Y + Math.Abs(distanceX);
                else //I. and II. Quadrant
                    sm.endDrag.Y = sm.startDrag.Y - Math.Abs(distanceX);

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (sm.dragging && sm.shiftPressed) //Can only be circle if player is still making the selection
                EllipseToCircleCoordsFix();

            if (sm.startDrag != sm.endDrag)
                DrawEllipse((int)sm.startDrag.X, (int)sm.startDrag.Y, (int)sm.endDrag.X, (int)sm.endDrag.Y, spriteBatch);
        }
    }
}
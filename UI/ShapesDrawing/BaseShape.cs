using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using BuilderEssentials.Items;
using BuilderEssentials.Utilities;
using static BuilderEssentials.Utilities.Tools;
using Terraria.ID;
using IL.Terraria.World.Generation;

namespace BuilderEssentials.UI.ShapesDrawing
{
    class BaseShape : UIElement
    {
        public static BaseShape Instance;
        public ShapesState sd;
        public Color color;
        public override void OnInitialize()
        {
            Instance = this;
            sd = ShapesState.Instance;
        }

        //Taken from http://members.chello.at/easyfilter/bresenham.html. All credits go to Alois Zingl
        #region Algorithms by Alois Zingl (adapted)
        public void DrawEllipse(int x0, int y0, int x1, int y1)
        {
            var sel = ShapesMenu.optionSelected;
            bool quadOne = (sel[4] && sel[5]) || (sel[3] && !sel[5]);
            bool quadTwo = (sel[4] && !sel[5]) || (sel[3] && !sel[5]);
            bool quadThree = (sel[3] && sel[5]) || (sel[4] && !sel[5]);
            bool quadFour = (sel[3] && sel[5]) || (sel[4] && sel[5]);
            bool noQuad = quadOne == false && quadTwo == false && quadThree == false && quadFour == false;

            int a = Math.Abs(x1 - x0), b = Math.Abs(y1 - y0), b1 = b & 1; //values of diameter
            long dx = 4 * (1 - a) * b * b, dy = 4 * (b1 + 1) * a * a; //error increment 
            long err = dx + dy + b1 * a * a, e2; //error of 1.step

            if (x0 > x1) { x0 = x1; x1 += a; } //if called with swapped points
            if (y0 > y1) y0 = y1; //exchange them
            y0 += (b + 1) / 2; y1 = y0 - b1;   //starting pixel
            a *= 8 * a; b1 = 8 * b * b;

            do
            {
                if (noQuad || quadOne)
                    SetRectangle(x1, y0);    //   I. Quadrant
                if (noQuad || quadTwo)
                    SetRectangle(x0, y0);    //  II. Quadrant
                if (noQuad || quadThree)
                    SetRectangle(x0, y1);    // III. Quadrant
                if (noQuad || quadFour)
                    SetRectangle(x1, y1);    //  IV. Quadrant

                //Fill
                if (ShapesDrawer.channeling && ShapesMenu.optionSelected[2] && ShapesDrawer.selectedItemType != -1)
                {
                    //No half shape selected
                    if (noQuad)
                    {
                        //Horizontal
                        DrawLine(x0, y0, x1, y0);
                        DrawLine(x0, y1, x1, y1);
                        //Vertical
                        DrawLine(x0, y0, x0, y1);
                        DrawLine(x1, y0, x1, y1);

                        //Diagonals (sometimes a few blocks are left to be placed and this fills the remaining)
                        DrawLine(x0, y0, x1, y1);
                        DrawLine(x0, y1, x1, y0);
                    }

                    //Half Shapes
                    if (ShapesMenu.optionSelected[3])
                    {
                        int tempY = !ShapesMenu.optionSelected[5] ? y0 : y1;
                        bool condition = !ShapesMenu.optionSelected[5] ? tempY < y1 : tempY > y1;

                        do
                        {
                            DrawLine(x0, tempY, x1, tempY);
                            tempY += condition ? 1 : -1;
                        }
                        while (condition);
                    }
                    else if (ShapesMenu.optionSelected[4])
                    {
                        int tempX = !ShapesMenu.optionSelected[5] ? x0 : x1;
                        bool condition = !ShapesMenu.optionSelected[5] ? tempX > x1 : tempX < x1;

                        do
                        {
                            DrawLine(tempX, y0, tempX, y1);
                            tempX += condition ? -1 : 1;
                        }
                        while (condition);
                    }
                }

                e2 = 2 * err;
                if (e2 <= dy) { y0++; y1--; err += dy += a; }  //y step
                if (e2 >= dx || 2 * err > dy) { x0++; x1--; err += dx += b1; } //x step

            } while (x0 <= x1);

            while (y0 - y1 < b)
            {  //too early stop of flat ellipses a=1
                SetRectangle(x0 - 1, y0); //-> finish tip of ellipse
                SetRectangle(x1 + 1, y0++);
                SetRectangle(x0 - 1, y1);
                SetRectangle(x1 + 1, y1--);
            }
        }

        public void DrawLine(int x0, int y0, int x1, int y1)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2; //error value e_xy
            for (; ; )
            { // loop
                SetRectangle(x0, y0);
                e2 = 2 * err;
                if (e2 >= dy)
                { // e_xy+e_x > 0
                    if (x0 == x1) break;
                    err += dy; x0 += sx;
                }
                if (e2 <= dx)
                { // e_xy+e_y < 0
                    if (y0 == y1) break;
                    err += dx; y0 += sy;
                }
            }
        }
        #endregion

        public void DrawRectangle(int x0, int y0, int x1, int y1)
        {
            int selectedQuarter = SelectedQuarter(x0, y0, x1, y1);

            //Maybe there's a better way to do this?
            //If vertical is always full lenght and horizontal stays in the middle?
            switch (selectedQuarter)
            {
                case 0:
                    DrawLine(x1, y1, x0 - 1, y1); //Top Line
                    DrawLine(x0, y1, x0, y0 - 1); //Right Line
                    DrawLine(x0, y0, x1 + 1, y0); //Bottom Line
                    DrawLine(x1, y0, x1, y1 + 1); //Left Line
                    break;
                case 1:
                    DrawLine(x0, y1, x1 - 1, y1); //Top Line
                    DrawLine(x1, y1, x1, y0 - 1); //Right Line
                    DrawLine(x1, y0, x0 + 1, y0); //Bottom Line
                    DrawLine(x0, y0, x0, y1 + 1); //Left Line
                    break;
                case 2:
                    DrawLine(x1, y0, x0 - 1, y0); //Top Line
                    DrawLine(x0, y0, x0, y1 - 1); //Right Line
                    DrawLine(x0, y1, x1 + 1, y1); //Bottom Line
                    DrawLine(x1, y1, x1, y0 + 1); //Left Line
                    break;
                case 3:
                    DrawLine(x0, y0, x1 - 1, y0); //Top Line
                    DrawLine(x1, y0, x1, y1 - 1); //Right Line
                    DrawLine(x1, y1, x0 + 1, y1); //Bottom Line
                    DrawLine(x0, y1, x0, y0 + 1); //Left Line
                    break;
                default:
                    break;
            }

            //Fill
            if (ShapesDrawer.channeling && ShapesMenu.optionSelected[2] && ShapesDrawer.selectedItemType != -1)
            {
                do
                {
                    y0++;
                    DrawLine(x0, y0, x1, y0);
                }
                while (y0 < y1);
            }
        }

        public int SelectedQuarter(int x0, int y0, int x1, int y1)
        {
            //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
            int selectedQuarter = -1;

            if (x0 <= x1 && y0 <= y1)
                selectedQuarter = 3;
            else if (x0 <= x1 && y0 >= y1)
                selectedQuarter = 1;
            else if (x0 >= x1 && y0 >= y1)
                selectedQuarter = 0;
            else if (x0 >= x1 && y0 <= y1)
                selectedQuarter = 2;

            return selectedQuarter;
        }

        //Draws "pixels" on screen
        public void SetRectangle(int x, int y)
        {
            Texture2D texture = Main.extraTexture[2];
            Rectangle value = new Rectangle(0, 0, 16, 16);
            Vector2 position = new Vector2(x, y) * 16 - Main.screenPosition;

            if (ShapesDrawer.channeling && ShapesDrawer.selectedItemType != -1)
            {
                //TODO: MAKE RESOURCES DEPLETE
                //TODO: EXCLUDE TILES BIGGER THAN 1x1

                Item myItem = new Item();
                myItem.SetDefaults(ShapesDrawer.selectedItemType);
                //Can't get tileObjectData to work?

                switch (WhatIsThisItem(ShapesDrawer.selectedItemType))
                {
                    case ItemTypes.Air:
                        break;
                    case ItemTypes.Tile:
                        WorldGen.PlaceTile(x, y, myItem.createTile);
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendTileSquare(-1, x, y, 1);
                        break;
                    case ItemTypes.Wall:
                        WorldGen.PlaceWall(x, y, myItem.createWall);
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendTileSquare(-1, x, y, 1);
                        break;
                }
            }

            Main.spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void SquareCoords()
        {
            int distanceX = (int)(sd.endDrag.X - sd.startDrag.X);
            int distanceY = (int)(sd.endDrag.Y - sd.startDrag.Y);

            //Turning rectangle (startDrag->endDrag) into a square
            if (Math.Abs(distanceX) < Math.Abs(distanceY)) //Horizontal Ellipse
            {
                if (distanceX > 0) //I. and IV. Quadrant
                    sd.endDrag.X = sd.startDrag.X + Math.Abs(distanceY);
                else //II. and III. Quadrant
                    sd.endDrag.X = sd.startDrag.X - Math.Abs(distanceY);
            }
            else //Vertical Ellipse
            {
                if (distanceY > 0) //III. and IV. Quadrant
                    sd.endDrag.Y = sd.startDrag.Y + Math.Abs(distanceX);
                else //I. and II. Quadrant
                    sd.endDrag.Y = sd.startDrag.Y - Math.Abs(distanceX);
            }
        }
    }
}
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuilderEssentials.UI.ShapesDrawing
{
    class EllipseShape : BaseShape
    {
        void FixHalfShapesOffset()
        {
            //TODO: FIX HALF SHAPES HORIZONTAL/VERTICAL OFFSETS (KEEP LEFT TOP CORNER FIXED)

            if (ShapesMenu.optionSelected[3])
            {
                //Preventing half shape from "mirroring" to the other quadrant
                if (ShapesMenu.optionSelected[5] && sd.endDrag.Y >= sd.startDrag.Y)
                    sd.endDrag.Y = sd.startDrag.Y;
                else if (!ShapesMenu.optionSelected[5] && sd.endDrag.Y <= sd.startDrag.Y)
                    sd.endDrag.Y = sd.startDrag.Y;
            }
            else if (ShapesMenu.optionSelected[4])
            {
                //Preventing half shape from "mirroring" to the other quadrant
                if (ShapesMenu.optionSelected[5] && sd.endDrag.X <= sd.startDrag.X)
                    sd.endDrag.X = sd.startDrag.X;
                else if (!ShapesMenu.optionSelected[5] && sd.endDrag.X >= sd.startDrag.X)
                    sd.endDrag.X = sd.startDrag.X;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sb = spriteBatch;

            //Disable drawing if conditions met
            if (!ShapesMenu.SDEquipped || !ShapesMenu.optionSelected[0])
                return;

            if (sd.dragging && sd.shiftPressed) //Can only be circle if player is still making the selection
                SquareCoords();

            if (sd.dragging && (ShapesMenu.optionSelected[3] || ShapesMenu.optionSelected[4]))
                FixHalfShapesOffset();

            color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f; //Blue
            if (ShapesMenu.optionSelected[2])
                color = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f; //Yellow

            if (sd.startDrag != sd.endDrag)
                DrawEllipse((int)sd.startDrag.X, (int)sd.startDrag.Y, (int)sd.endDrag.X, (int)sd.endDrag.Y);
        }
    }
}
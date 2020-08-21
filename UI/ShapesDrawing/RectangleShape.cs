using Microsoft.Xna.Framework.Graphics;

namespace BuilderEssentials.UI.ShapesDrawing
{
    class RectangleShape : BaseShape
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ShapesMenu.optionSelected[1])
            {
                if (sd.dragging && sd.shiftPressed) //Can only be square if player is still making the selection
                    SquareCoords();

                DrawRectangle((int)sd.startDrag.X, (int)sd.startDrag.Y, (int)sd.endDrag.X, (int)sd.endDrag.Y);
            }
        }
    }
}

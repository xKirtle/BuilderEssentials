using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuilderEssentials.UI.ShapesDrawing
{
    class RectangleShape : BaseShape
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            sb = spriteBatch;

            //Disable drawing if conditions met
            if (!ShapesMenu.SDEquipped || !ShapesMenu.optionSelected[1])
                return;

            if (sd.dragging && sd.shiftPressed) //Can only be square if player is still making the selection
                SquareCoords();

            color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f; //Blue
            if (ShapesMenu.optionSelected[2])
                color = new Color(0.9f, 0.8f, 0.24f, 1f) * 0.8f; //Yellow

            if (sd.startDrag != sd.endDrag)
                DrawRectangle((int)sd.startDrag.X, (int)sd.startDrag.Y, (int)sd.endDrag.X, (int)sd.endDrag.Y);
        }
    }
}

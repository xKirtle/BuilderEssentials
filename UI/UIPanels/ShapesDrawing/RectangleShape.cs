using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace BuilderEssentials.UI.UIPanels.ShapesDrawing
{
    public class RectangleShape : BaseShape
    {
        private CoordsSelection cs;
        public RectangleShape()
        {
            cs = UIStateLogic1.mirrorWandCoords;
            color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f; //Blue
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.HeldItem.type != cs.itemType) return;
            PlotRectangle((int)cs.RMBStart.X, (int)cs.RMBStart.Y, (int)cs.RMBEnd.X, (int)cs.RMBEnd.Y);
        }
    }
}
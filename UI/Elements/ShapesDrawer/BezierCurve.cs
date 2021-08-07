using System.Collections.Generic;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace BuilderEssentials.UI.Elements.ShapesDrawer
{
    internal class BezierCurve : BaseShape
    {
        public CoordsSelection cs;
        public void DrawQuadraticBezier(float dt, Vector2 startPoint, Vector2 controlPoint, Vector2 endPoint)
        {
            List<Vector2> points = new List<Vector2>();
            for (float t = 0.0f; t < 1.0f; t += dt)
                points.Add(HelperMethods.TraverseBezier(startPoint, controlPoint, endPoint, t));

            points.Add(HelperMethods.TraverseBezier(startPoint, controlPoint, endPoint, 1.0f));

            for (int i = 0; i < points.Count - 1; i++)
                PlotLine((int)points[i].X, (int)points[i].Y, (int)points[i+1].X, (int)points[i+1].Y);
        }

        public void DrawCubicBezier(float dt, Vector2 startPoint, Vector2 controlPoint, Vector2 controlPoint2, Vector2 endPoint)
        {
            List<Vector2> points = new List<Vector2>();
            for (float t = 0.0f; t < 1.0; t += dt)
                points.Add(HelperMethods.TraverseBezier(startPoint, controlPoint, controlPoint2, endPoint, t));
            
            points.Add(HelperMethods.TraverseBezier(startPoint, controlPoint, controlPoint2, endPoint, 1.0f));

            for (int i = 0; i < points.Capacity - 1; i++)
                PlotLine((int)points[i].X, (int)points[i].Y, (int)points[i+1].X, (int)points[i+1].Y);
        }

        public BezierCurve()
        {
            cs = new CoordsSelection(ItemID.None, BaseUIState.Instance);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Prevents drawing and coord updates if Improved Ruler is not equipped
           // if (!Main.LocalPlayer.GetModPlayer<BEPlayer>().improvedRulerEquipped) return;
            
            cs.UpdateCoords(true);
            color = Blue;
            
            if (cs.LMBStart != cs.LMBEnd) //&& ImprovedRuler is equipped
                DrawQuadraticBezier(0.5f, cs.LMBStart, cs.RMBEnd, cs.LMBEnd);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using BuilderEssentials.Items;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels.ShapesDrawerUI
{
    public class BezierCurve : BaseShape
    {
        public CoordsSelection cs;

        public void DrawBezier(float dt, Vector2 startPoint, Vector2 controlPoint, Vector2 endPoint)
        {
            List<Vector2> points = new List<Vector2>();
            for (float t = 0.0f; t < 1.0; t += dt)
                points.Add(HelperMethods.TraverseBezier(startPoint, controlPoint, endPoint, t));

            points.Add(HelperMethods.TraverseBezier(startPoint, controlPoint, endPoint, 1.0f));

            for (int i = 0; i < points.Count - 1; i++)
                PlotLine((int)points[i].X, (int)points[i].Y, (int)points[i+1].X, (int)points[i+1].Y);
        }
        
        public BezierCurve()
        {
            cs = new CoordsSelection(ItemID.None);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            cs.UpdateCoords(true);
            color = Blue;
            
            if (cs.LMBStart != cs.LMBEnd) //&& ImprovedRuler is equipped
                DrawBezier(0.5f, cs.LMBStart, cs.RMBEnd, cs.LMBEnd);
        }
    }
}
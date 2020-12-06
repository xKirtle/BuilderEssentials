using System;
using BuilderEssentials.Items;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace BuilderEssentials.UI.UIPanels.ShapesDrawerUI
{
    public class ShapesDrawerSelection : BaseShape
    {
        public CoordsSelection cs;
        public bool[] selected;

        public ShapesDrawerSelection()
        {
            cs = new CoordsSelection(ModContent.ItemType<ShapesDrawer>());
            selected = UIStateLogic1.menuPanel.selected;
        }

        public override void DrawRectangle(int x, int y)
        {
            base.DrawRectangle(x, y);

            if (cs.LMBDown && ShapesDrawer.selectedItemType != -1)
                HelperMethods.PlaceTile(x, y, ShapesDrawer.selectedItemType);
        }
    }
}
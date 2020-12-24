using System;
using BuilderEssentials.Items;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace BuilderEssentials.UI.UIPanels.ShapesDrawerUI
{
    public class ShapesDrawerSelection : BaseShape
    {
        public CoordsSelection cs;
        public bool[] selected;

        public ShapesDrawerSelection()
        {
            cs = new CoordsSelection(ModContent.ItemType<ShapesDrawer>());
            selected = UIStateLogic4.menuPanel.selected;
        }

        public override void DrawRectangle(int x, int y)
        {
            base.DrawRectangle(x, y);
            
            //Can't call player.mouseInterface here because its value was not updated yet when doing draw code
            if (ShapesDrawer.LMBDown && ShapesDrawer.selectedItemType != -1)
                HelperMethods.PlaceTile(x, y, ShapesDrawer.selectedItemType);
        }
    }
}
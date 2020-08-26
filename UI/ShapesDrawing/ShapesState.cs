using BuilderEssentials.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.UI.ShapesDrawing
{
    class ShapesState : UIState
    {
        public static ShapesState Instance;
        public override void OnInitialize()
        {
            Instance = this;

            OnRightMouseDown += DragStart;
            OnRightMouseUp += DragEnd;

            EllipseShape circleShape = new EllipseShape();
            Append(circleShape);

            RectangleShape rectangleShape = new RectangleShape();
            Append(rectangleShape);
        }

        public bool dragging;
        public Vector2 startDrag = Vector2.Zero;
        public Vector2 endDrag = Vector2.Zero;
        public bool shiftPressed;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!Main.LocalPlayer.mouseInterface && Main.LocalPlayer.HeldItem.type == ItemType<ShapesDrawer>() 
                && (ShapesMenu.optionSelected[0] || ShapesMenu.optionSelected[1]))
            {
                dragging = true;
                startDrag = endDrag = new Vector2(Player.tileTargetX, Player.tileTargetY);
            }
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            dragging = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (dragging)
                endDrag = new Vector2(Player.tileTargetX, Player.tileTargetY);

            //Disable ingame shift while shiftPressed?
            shiftPressed = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
        }
    }
}

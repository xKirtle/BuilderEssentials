using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using BuilderEssentials.UI.ShapesDrawing;

namespace BuilderEssentials.UI.ItemsUI
{
    class ImprovedRulerUI : UIState
    {
        public static ImprovedRulerUI Instance;
        public static bool IREquipped;

        public override void OnInitialize()
        {
            Instance = this;

            OnRightMouseDown += RMBDragDown;
            OnRightMouseUp += RMBDragUp;

            OnMouseDown += LMBDragDown;
            OnMouseUp += LMBDragUp;

            UIElement improvedRuler = new ImprovedRuler();
            Append(improvedRuler);
        }

        public bool RMBDragging;
        public Vector2 RMBPos = Vector2.Zero;
        public bool LMBDragging;
        public Vector2 LMBPos = Vector2.Zero;

        private void RMBDragDown(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.HeldItem.IsAir)
            {
                RMBDragging = true;
                RMBPos = new Vector2(Player.tileTargetX, Player.tileTargetY);
            }
        }

        private void RMBDragUp(UIMouseEvent evt, UIElement listeningElement)
        {
            RMBDragging = false;
        }

        private void LMBDragDown(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.HeldItem.IsAir)
            {
                LMBDragging = true;
                LMBPos = new Vector2(Player.tileTargetX, Player.tileTargetY);
            }
        }

        private void LMBDragUp(UIMouseEvent evt, UIElement listeningElement)
        {
            LMBDragging = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (RMBDragging)
                RMBPos = new Vector2(Player.tileTargetX, Player.tileTargetY);


            if (LMBDragging)
                LMBPos = new Vector2(Player.tileTargetX, Player.tileTargetY);
        }
    }

    class ImprovedRuler : BaseShape
    {
        //TODO: Bezier curve
        ImprovedRulerUI ir = ImprovedRulerUI.Instance;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ir.RMBPos == Vector2.Zero || ir.LMBPos == Vector2.Zero) return;

            if (ImprovedRulerUI.IREquipped && ir.RMBPos != ir.LMBPos)
            {
                color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f; //Blue
                DrawLine((int)ir.LMBPos.X, (int)ir.LMBPos.Y, (int)ir.RMBPos.X, (int)ir.RMBPos.Y);
            }    
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BuilderEssentials.UI.Elements
{
    internal class CustomUIImage : UIImage
    {
        public CustomUIImage(Asset<Texture2D> texture) : base(texture) { }
        public CustomUIImage(Texture2D nonReloadingTexture) : base(nonReloadingTexture) { }
        
        /// <summary>
        /// Get the current visibility state.
        /// </summary>
        public bool Visible { get; private set; } = true;
        private UIElement _parent;
        
        
        /// <summary>
        /// Hides the element if visible or shows the element if not.
        /// </summary>
        public virtual void Toggle()
        {
            if (Visible)
                Hide();
            else
                Show();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
        }

        #region Events

        new public delegate void ElementEvent(UIElement affectedElement);

        public event ElementEvent OnShow;
        public event ElementEvent OnHide;

        /// <summary>
        /// Makes the element render and re-enables all of its functionality.
        /// </summary>
        public virtual void Show()
        {
            if (Visible) return;
            
            Visible = true;
            _parent?.Append(this);

            OnShow?.Invoke(this);
        }

        /// <summary>
        /// Makes the element not render and disables all of its functionality.
        /// </summary>
        public virtual void Hide()
        {
            if (!Visible) return;
            
            Visible = false;
            _parent = Parent;
            Remove();

            OnHide?.Invoke(this);
        }

        #endregion
    }
}
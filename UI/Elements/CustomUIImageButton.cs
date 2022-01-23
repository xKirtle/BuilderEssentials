using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace BuilderEssentials.UI.Elements
{
    public class CustomUIImageButton : UIImageButton
    {
        /// <summary>
        /// Whether the UIElement will behave like a toggleable or not.
        /// </summary>
        public bool Toggleable { get; private set; }
        
        /// <summary>
        /// Whether the UIElement was toggled or not.
        /// </summary>
        public bool Toggled { get; private set; }
        
        public CustomUIImageButton(Asset<Texture2D> texture) : base(texture)
        {
            OnMouseOver += (__, _) =>
            {
                if (Main.LocalPlayer.whoAmI != Main.myPlayer) return;
                SoundEngine.PlaySound(SoundID.MenuTick);
            };
            
            SetOpacity(.45f);

            OnMouseDown += (__, _) =>
            {
                if (Toggleable) Toggled = !Toggled;
                SetOpacity(Toggled ? 1f : .75f);
            };
            OnMouseOver += (__, _) => { if (!Toggled) SetOpacity(.75f); };
            OnMouseOut += (__, _) => { if (!Toggled) SetOpacity(.45f); };
        }
        
        public void SetOpacity(float opacity) => SetVisibility(opacity, opacity);

        /// <summary>
        /// Sets the Toggleable state of the UIElement.
        /// </summary>
        /// <param name="isToggleable">If true, UIElement is toggleable.</param>
        public void SetToggleable(bool isToggleable)
        {
            Toggleable = isToggleable;
        }

        /// <summary>
        /// Used to manually modify the Toggled value. (Should not be used unless necessary)
        /// </summary>
        /// <param name="toggleValue">If true, UIelement is toggled on.</param>
        public void SetToggled(bool toggleValue)
        {
            Toggled = toggleValue;
        }
    }
}
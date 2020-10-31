using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    public class BaseUIState : UIState
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
            UIImage image = new UIImage(Main.itemTexture[ItemID.BeamSword]);
            image.Left.Set(Main.screenWidth / 4, 0);
            image.Top.Set(Main.screenHeight / 2, 0);
            Append(image);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
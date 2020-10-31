using BuilderEssentials.UI.UIPanels;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    public class ItemsUIState : UIState
    {
        public static ItemsUIState Instance;
        private static MultiWandWheel multiWandWheel;
        public override void OnInitialize()
        {
            Instance = this;
            someInitMethod();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            multiWandWheel?.UpdateHoverText();
        }

        static void someInitMethod()
        {
            multiWandWheel = new MultiWandWheel();
            multiWandWheel.Activate();
            Instance.Append(multiWandWheel);
        }
    }
}
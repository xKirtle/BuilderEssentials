using BuilderEssentials.UI.UIPanels;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BuilderEssentials.UI.UIStates
{
    public class ItemsUIState : UIState
    {
        public static ItemsUIState Instance;
        public static MultiWandWheel multiWandWheel;
        public static AutoHammerWheel autoHammerWheel;
        public static PaintWheel paintWheel;
        public static FillWandSelection fillWandSelection;

        public override void OnInitialize()
        {
            Instance = this;
            
            multiWandWheel = new MultiWandWheel();
            Instance.Append(multiWandWheel);
            multiWandWheel.Hide();
            
            autoHammerWheel = new AutoHammerWheel();
            Instance.Append(autoHammerWheel);
            autoHammerWheel.Hide();
            
            paintWheel = new PaintWheel();
            Instance.Append(paintWheel);
            paintWheel.Hide();
            
            fillWandSelection = new FillWandSelection();
            Instance.Append(fillWandSelection);
            fillWandSelection.Hide();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            multiWandWheel?.Update();
            autoHammerWheel?.Update();
            paintWheel?.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            paintWheel?.UpdateColors();
            
            
            
            //Main DrawBuilderAccToggles
            //On.Terraria.Main.DrawBuilderAccToggles
            
            //y = 0 is never used?
            //Default value 16, 16
            
            //Ruler 0,0 && 0,1
            //Laser Ruler 1,0 && 1,1
            //Actuation device 2,0 && 2,1
            //Paint Sprayer 3,0 && 3,1
            //Red Wire 4,0 && 4,1
            //Blue Wire 5,0 && 5,1
            //Green Wire 6,0 && 6,1
            //Yellow Wire 7,0 && 7,1
            //Mechanical display 8,1 (force show wires thing)
            //Actuators 9,1
            
            //TODO: Implement DrawBuilderAccToggles myself from vanilla so I can add more options to it
            //Main DrawBuilderAccToggles
            //On.Terraria.Main.DrawBuilderAccToggles

            // Texture2D builderIcons = Main.builderAccTexture; //ModContent.GetTexture("Terraria/UI/BuilderIcons");
            // builderIcons.Frame(10, 2, 16, 16); //returns a rectangle used in the sourceRect in sb drawing
            
            // string text = "";
            // foreach (var stat in player.builderAccStatus)
            // {
            //     text += " " + stat;
            // }
            //
            // Main.NewText(text);
            
            // Vector2 position = new Vector2(Player.tileTargetX, Player.tileTargetY) * 16 - Main.screenPosition;
            // spriteBatch.Draw(Main.builderAccTexture, position, new Rectangle(0, 0, 16, 16), Color.White);
        }
    }
}
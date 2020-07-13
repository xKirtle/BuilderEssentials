// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using Terraria.UI;
// using Terraria;
// using Terraria.ModLoader;
// using BuilderEssentials.Items;
// using System;
// namespace BuilderEssentials.UI
// {
//     public class FillWandUI : UIElement
//     {
//         Texture2D texture = Main.extraTexture[2];
//         Rectangle value = new Rectangle(0, 0, 16, 16);
//         Color color = new Color(0.24f, 0.8f, 0.9f, 1f) * 0.8f;
//         Vector2 position = new Vector2();

//         public override void Draw(SpriteBatch spriteBatch)
//         {

//             for (int i = 0; i < FillWand.fillSelectionSize; i++)
//             {
//                 for (int j = 0; j < FillWand.fillSelectionSize; j++)
//                 {
//                     position = new Vector2(Main.mouseX - FillWand.fillSelectionSize / 2, Main.mouseY - FillWand.fillSelectionSize / 2) * 16;
//                     spriteBatch.Draw(texture, position, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
//                 }
//             }
//         }
//     }
// }
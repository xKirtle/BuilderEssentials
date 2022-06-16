using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace BuilderEssentials.Content.UI.PixelShapes;

public class BasePixelShapesState : UIState
{
    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        // Vector2 start = Main.LocalPlayer.Center.ToTileCoordinates().ToVector2();
        // Vector2 end = BEPlayer.PointedTileCoords;
        // Pixel.PlotLine(start, end);
    }
}
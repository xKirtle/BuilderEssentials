using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace BuilderEssentials.Content.UI.PixelShapes;

public class BasePixelShapesState : BaseUIState
{
    public override int[] BoundItemType => new int[0];

    public override void Dispose() {
        
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        Vector2 start = Main.LocalPlayer.Center.ToTileCoordinates().ToVector2();
        Vector2 end = BEPlayer.PointedTileCoords;
        Pixel.PlotLine(start, end);
    }
}
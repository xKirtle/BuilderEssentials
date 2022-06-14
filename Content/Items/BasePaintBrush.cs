using BuilderEssentials.Common;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(false)]
public abstract class BasePaintBrush : BaseItemToggleableUI
{
    public override UIStateType UiStateType => UIStateType.PaintBrush;
    
    public override void SetDefaults() {
        base.SetDefaults();
        
        Item.height = 44;
        Item.width = 44;
        Item.useTime = 1;
        Item.useAnimation = 1;
        Item.useTurn = true;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.value = Item.sellPrice(silver: 80);
        Item.rare = ItemRarityID.Red;
        Item.autoReuse = true;
    }
    
    public override Vector2? HoldoutOffset() => new Vector2(5, -8);

    public override bool CanUseItem(Player player) {
        if (!base.CanUseItem(player)) return false;

        var panel = PaintBrushState.Instance.menuPanel;
        byte selectedColor = (byte) (panel.colorIndex + 1);
        int toolIndex = panel.toolIndex;

        if (toolIndex == 0 || toolIndex == 1) {
            PaintTileOrWall(selectedColor, toolIndex, BEPlayer.PointedCoord);
        }
        else ScrapPaint(BEPlayer.PointedCoord);

        return true;
    }

    public static int PaintItemTypeToColorIndex(int paintType)
    {
        //The outputed indexes are not the paint color byte values. For those just increment one.

        if (paintType >= 1073 && paintType <= 1099)
            return paintType - 1073;
        else if (paintType >= 1966 && paintType <= 1968)
            return paintType - 1939;
        else if (paintType >= 4668)
            return paintType - 4638;

        return -1; //it will never reach here
    }
    
    public static int ColorByteToPaintItemType(byte color)
    {
        if (color >= 1 && color <= 27)
            return (color - 1) + 1073;
        else if (color >= 28 && color <= 30)
            return (color - 1) + 1939;
        else if (color == 31)
            return (color - 1) + 4638;

        return -1; //it will never reach here
    }
    
    public static void PaintTileOrWall(byte color, int selectedTool, Vector2 coords)
    {
        Tile tile = Framing.GetTileSafely(coords);
        if (color < 1 || color > 32 || selectedTool < 0 || selectedTool > 1) return;
        bool needSync = false;

        if (selectedTool == 0 && tile.HasTile && tile.TileType >= 0 && tile.TileColor != color) {
            WorldGen.paintEffect((int)coords.X / 16, (int)coords.Y / 16, color, tile.TileColor);
            tile.TileColor = color;
            needSync = true;
        }
        else if (selectedTool == 1 && !tile.HasTile && tile.WallType > 0 && tile.WallColor != color) {
            WorldGen.paintEffect((int)coords.X / 16, (int)coords.Y / 16, color, tile.WallColor);
            tile.WallColor = color;
            needSync = true;
        }

        if (needSync && Main.netMode != NetmodeID.SinglePlayer)
            NetMessage.SendTileSquare(-1, (int)coords.X, (int)coords.Y, 1);
    }

    public static void ScrapPaint(Vector2 coords) {
        Tile tile = Framing.GetTileSafely(coords);
        bool needSync = false;

        if (tile.TileColor != 0) {
            WorldGen.paintEffect((int)coords.X / 16, (int)coords.Y / 16, 0, tile.TileColor);
            tile.TileColor = 0;
            needSync = true;
        }
        else if (tile.WallColor != 0) {
            WorldGen.paintEffect((int)coords.X / 16, (int)coords.Y / 16, 0, tile.WallColor);
            tile.WallColor = 0;
            needSync = true;
        }

        if (needSync && Main.netMode != NetmodeID.SinglePlayer) {
            NetMessage.SendData(MessageID.PaintTile, number: (int)coords.X, number2: (int)coords.Y);
            NetMessage.SendData(MessageID.PaintWall, number: (int)coords.X, number2: (int)coords.Y);
        }
    }
}
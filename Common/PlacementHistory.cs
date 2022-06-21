using Microsoft.Xna.Framework;
using Terraria;

namespace BuilderEssentials.Common;

public struct PlacementHistory
{
    public Point Coordinate;
    public MinimalTile PreviousTile;
    public MinimalTile PlacedTile;
    public Item SelectedItem;

    public PlacementHistory(Point coordinate, MinimalTile previousTile, MinimalTile placedTile, Item selectedItem) {
        Coordinate = coordinate;
        PreviousTile = previousTile;
        PlacedTile = placedTile;
        SelectedItem = selectedItem;
    }
}

public struct MinimalTile
{
    public int TileType;
    public int WallType;
    public bool HasTile;
    public int PlaceStyle;
    public bool IsWall { get; }

    public MinimalTile(int tileType, int wallType, bool hasTile, int placeStyle) {
        TileType = tileType;
        WallType = wallType;
        HasTile = hasTile;
        PlaceStyle = placeStyle;
        IsWall = WallType > 0 && !HasTile && TileType <= 0;
    }

    public override string ToString() {
        return $"TileType: {TileType} WallType: {WallType} HasTile: {HasTile} IsWall: {IsWall}";
    }
}
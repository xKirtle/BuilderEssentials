using Microsoft.Xna.Framework;
using Terraria;

namespace BuilderEssentials.Common;

public struct PlacementHistory
{
    public Point Coordinate;
    public MinimalTile PreviousTile;
    public MinimalTile PlacedTile;

    public PlacementHistory(Point coordinate, MinimalTile previousTile, MinimalTile placedTile) {
        Coordinate = coordinate;
        PreviousTile = previousTile;
        PlacedTile = placedTile;
    }
}

public struct MinimalTile
{
    public int TileType;
    public int WallType;
    public bool HasTile;
    public bool IsWall { get; }

    public MinimalTile(int tileType, int wallType, bool hasTile) {
        TileType = tileType;
        WallType = wallType;
        HasTile = hasTile;
        IsWall = WallType > 0 && !HasTile && TileType <= 0;
    }

    public override string ToString() {
        return $"TileType: {TileType} WallType: {WallType} HasTile: {HasTile} IsWall: {IsWall}";
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Common.DataStructures;

//TODO: Unified solution instead of scattered structs?

public struct MinimalTile
{
    public int TileType;
    public int WallType;
    public bool HasTile;
    public int PlaceStyle;
    public bool HasWall { get; }

    public MinimalTile(int tileType, int wallType, bool hasTile, int placeStyle) {
        TileType = tileType;
        WallType = wallType;
        HasTile = hasTile;
        PlaceStyle = placeStyle;
        HasWall = WallType > 0;
    }

    public override string ToString() {
        return $"TileType: {TileType} WallType: {WallType} HasTile: {HasTile} IsWall: {HasWall}";
    }
}

public readonly record struct MinimalTileData(int TileType, int WallType, bool HasTile, SlopeType Slope, bool IsHalfBlock, Point coord)
{
    public MinimalTileData(Tile tile, Point coord) : this(tile.TileType, tile.WallType, tile.HasTile, tile.Slope,
        tile.IsHalfBlock, coord) {
    }
}

public readonly record struct TileData(TileTypeData TileTypeData, WallTypeData WallTypeData,
		TileWallWireStateData TileWallWireStateData, LiquidData LiquidData, Point coord)
{
	public TileData(Tile tile, Point coord) : this(tile.Get<TileTypeData>(), tile.Get<WallTypeData>(), 
		tile.Get<TileWallWireStateData>(), tile.Get<LiquidData>(), coord) {
	}

	public void CopyToTile(Tile tile, bool tileData = false, bool wallData = false, 
		bool wireData = false, bool liquidData = false, bool paintOnly = false) {
		var newTileData = tile.Get<TileWallWireStateData>();

		if (paintOnly) {
			newTileData.TileColor = TileWallWireStateData.TileColor;
			newTileData.WallColor = TileWallWireStateData.WallColor;
			
			tile.Get<TileWallWireStateData>() = newTileData;
			return;
		}
		
		if (tileData) {
			tile.Get<TileTypeData>() = TileTypeData;
			newTileData.HasTile = TileWallWireStateData.HasTile;
			newTileData.IsHalfBlock = TileWallWireStateData.IsHalfBlock;
			newTileData.Slope = TileWallWireStateData.Slope;
			newTileData.TileColor = TileWallWireStateData.TileColor;
			newTileData.TileFrameNumber = TileWallWireStateData.TileFrameNumber;
			newTileData.TileFrameX = TileWallWireStateData.TileFrameX;
			newTileData.TileFrameY = TileWallWireStateData.TileFrameY;
		}

		if (wallData) {
			tile.Get<WallTypeData>() = WallTypeData;
			newTileData.WallColor = TileWallWireStateData.WallColor;
			newTileData.WallFrameNumber = TileWallWireStateData.WallFrameNumber;
			newTileData.WallFrameX = TileWallWireStateData.WallFrameX;
			newTileData.WallFrameY = TileWallWireStateData.WallFrameY;
		}

		if (wireData) {
			newTileData.IsActuated = TileWallWireStateData.IsActuated;
			newTileData.HasActuator = TileWallWireStateData.HasActuator;
			newTileData.WireData = TileWallWireStateData.WireData;
			newTileData.RedWire = TileWallWireStateData.RedWire;
			newTileData.BlueWire = TileWallWireStateData.BlueWire;
			newTileData.GreenWire = TileWallWireStateData.GreenWire;
			newTileData.YellowWire = TileWallWireStateData.YellowWire;
		};

		if (liquidData)
			tile.Get<LiquidData>() = LiquidData;
		
		tile.Get<TileWallWireStateData>() = newTileData;
	}
}
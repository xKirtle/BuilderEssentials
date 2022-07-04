using BuilderEssentials.Common.DataStructures;
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
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Common;

public enum TypeOfItem
{
    Air,
    Tile,
    Wall
}

public static class PlacementHelpers
{
    public static bool ValidTileCoordinates(int i, int j)
        => i >= 0 && i < Main.maxTilesX && j >= 0 && j < Main.maxTilesY;
    
    
    public static bool CanReduceItemStack(Item item, int amount = 1, bool reduceStack = true, bool shouldBeHeld = false) {
        if (!ItemLoader.ConsumeItem(item, Main.LocalPlayer)) return true;

        if (shouldBeHeld && Main.LocalPlayer.HeldItem.type == item.type && Main.LocalPlayer.HeldItem.stack >= amount) {
            if (reduceStack)
                Main.LocalPlayer.HeldItem.stack -= amount;

            return true;
        }

        foreach (Item itemInv in Main.LocalPlayer.inventory) {
            if (itemInv.type == item.type && itemInv.stack >= amount) {
                if (reduceStack)
                    itemInv.stack -= amount;

                return true;
            }
        }

        return false;
    }

    public static int GetMaxPickPower() {
        int maxPickPower = 0;

        foreach (Item item in Main.LocalPlayer.inventory) {
            if (item.pick > maxPickPower)
                maxPickPower = item.pick;
        }

        return maxPickPower;
    }


    public static TypeOfItem WhatIsThisItem(int itemType) {
        if (itemType <= 0 || itemType >= ItemLoader.ItemCount) return TypeOfItem.Air;
        return WhatIsThisItem(new Item(itemType));
    }

    public static TypeOfItem WhatIsThisItem(Item item) {
        if (item.createTile >= TileID.Dirt && item.createWall <= WallID.Stone)
            return TypeOfItem.Tile;
        else if (item.createTile < TileID.Dirt && item.createWall > WallID.Stone)
            return TypeOfItem.Wall;

        return TypeOfItem.Air;
    }

    public static Tile PlaceTile(int x, int y, Item item, bool mute = false, bool forced = false, bool sync = true) {
        if (!ValidTileCoordinates(x, y)) return new Tile();

        Tile tile = Framing.GetTileSafely(x, y);
        bool replaceTilesEnabled = Main.LocalPlayer.builderAccStatus[10] == 0;
        TypeOfItem typeOfItem = WhatIsThisItem(item);

        if (typeOfItem == TypeOfItem.Tile) {
            TileObjectData data = TileObjectData.GetTileData(item.createTile, item.placeStyle);
            if (data != null) return new Tile(); //TODO: Place MultiTile
        }
        
        //No need to (re)place if the tile is already the desired 
        if ((typeOfItem == TypeOfItem.Tile && tile.TileType == item.createTile) ||
            (typeOfItem == TypeOfItem.Wall && tile.WallType == item.createWall))
            return tile;

        //Check if enough materials
        if (!CanReduceItemStack(item)) return tile;
        
        if (replaceTilesEnabled) {
            //Can't replace Air
            if (typeOfItem == TypeOfItem.Tile && !tile.HasTile)
                goto noReplacement;

            if (typeOfItem == TypeOfItem.Tile)
                if (!WorldGen.ReplaceTile(x, y, (ushort) item.createTile, item.placeStyle))
                    goto noReplacement;
            else if (typeOfItem == TypeOfItem.Wall)
                if (!WorldGen.ReplaceWall(x, y, (ushort) item.createWall))
                    goto noReplacement;

            return tile;
        }

        noReplacement:
        if (typeOfItem == TypeOfItem.Tile && (forced || !tile.HasTile))
            WorldGen.PlaceTile(x, y, item.createTile, mute, forced, style: item.placeStyle);
        else if (typeOfItem == TypeOfItem.Wall)
            WorldGen.PlaceWall(x, y, item.createWall, mute);

        if (sync && Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(-1, x, y, 1);

        return tile;
    }

    public static void PlaceTilesInArea(Point start, Point end, Item item, bool mute = false, bool forced = false,
        bool sync = true) {
        if (!ValidTileCoordinates(start.X, start.Y) || !ValidTileCoordinates(end.X, end.Y)) return;

        int dx = Math.Abs(start.X - end.X);
        int dy = Math.Abs(start.Y - end.Y);
        int minX = Math.Min(start.X, end.X);
        int minY = Math.Min(start.Y, end.Y);

        for (int y = minY; y < minY + dy; y++)
        for (int x = minX; x < minX + dx; x++)
            PlaceTile(x, y, item, mute, forced, sync: false);

        //Keeping syncSize an odd number since SendTileSquare as a bias towards up and left for even-numbers sizes
        int syncSize = Math.Max(dx, dy);
        syncSize += 1 + (syncSize % 2);

        if (sync && Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(-1, minX, minY, syncSize);
    }

    public static void RemoveTile(int x, int y, bool removeTile = true, bool removeWall = false, bool dropItem = true,
        int itemToDrop = -1, bool sync = true, bool needPickPower = false) {

        if (!ValidTileCoordinates(x, y)) return;

        Tile tile = Framing.GetTileSafely(x, y);
        if (needPickPower && TileLoader.GetTile(tile.TileType)?.MinPick > GetMaxPickPower()) return;
        
        if (removeTile && (WorldGen.CanKillTile(x, y, out _)))
            WorldGen.KillTile(x, y, noItem: !dropItem);

        //Can't not drop items when killing walls for some reason
        if (removeWall)
            WorldGen.KillWall(x, y);
        
        if (sync && Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(-1, x, y, 1);
    }
}
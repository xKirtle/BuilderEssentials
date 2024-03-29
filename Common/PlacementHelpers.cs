﻿using System;
using System.Linq;
using BuilderEssentials.Common.DataStructures;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Common;

public static class PlacementHelpers
{
    public static bool ValidTileCoordinates(int i, int j) => i >= 0 && i < Main.maxTilesX && j >= 0 && j < Main.maxTilesY;


    public static bool CanReduceItemStack(int type, int amount = 1, bool reduceStack = true, bool shouldBeHeld = false) {
        Item item = new(type);
        if (!ItemLoader.ConsumeItem(item, Main.LocalPlayer))
            return true;

        if (item.tileWand >= 0) {
            int wandIndex = MultiWand.WandPlacedTiles.ToList().IndexOf(item.createTile);
            type = MultiWand.WandMaterials[wandIndex];
        }

        if (shouldBeHeld && Main.LocalPlayer.HeldItem.type == type && Main.LocalPlayer.HeldItem.stack >= amount) {
            if (reduceStack)
                Main.LocalPlayer.HeldItem.stack -= amount;

            return true;
        }

        foreach (Item itemInv in Main.LocalPlayer.inventory) {
            if (itemInv.type == type && itemInv.stack >= amount) {
                if (reduceStack)
                    itemInv.stack -= amount;

                return true;
            }
        }

        return false;
    }

    public static TypeOfItem WhatIsThisItem(int itemType) {
        if (itemType <= 0 || itemType >= ItemLoader.ItemCount)
            return TypeOfItem.Air;
        return WhatIsThisItem(new Item(itemType));
    }

    public static TypeOfItem WhatIsThisItem(Item item) {
        if (item.createTile >= TileID.Dirt && item.createWall < WallID.Stone)
            return TypeOfItem.Tile;
        else if (item.createTile < TileID.Dirt && item.createWall >= WallID.Stone)
            return TypeOfItem.Wall;

        return TypeOfItem.Air;
    }

    public static bool PlaceMultiTile(int x, int y, Item item, bool mute = false, bool forced = false, bool sync = true)
        //Call PlaceTile_PlaceIt?
        => false;

    public static bool PlaceTile(int x, int y, Item item, bool mute = false, bool forced = false, bool sync = true) {
        if (!ValidTileCoordinates(x, y))
            return false;

        Tile tile = Framing.GetTileSafely(x, y);
        bool replaceTilesEnabled = Main.LocalPlayer.TileReplacementEnabled;
        TypeOfItem typeOfItem = WhatIsThisItem(item);

        if (typeOfItem == TypeOfItem.Tile) {
            TileObjectData data = TileObjectData.GetTileData(item.createTile, item.placeStyle);

            if (data != null) { return PlaceMultiTile(x, y, item, mute, forced, sync); }
        }

        //No need to (re)place if the tile is already the desired 
        if (typeOfItem == TypeOfItem.Tile && tile.TileType == item.createTile && tile.HasTile ||
            typeOfItem == TypeOfItem.Wall && tile.WallType == item.createWall)
            return false;

        //Check if enough materials
        if (!CanReduceItemStack(item.type))
            return false;

        if (replaceTilesEnabled) {
            //Can't replace Air
            if (typeOfItem == TypeOfItem.Tile && !tile.HasTile)
                goto noReplacement;

            if (typeOfItem == TypeOfItem.Tile) {
                if (!WorldGen.ReplaceTile(x, y, (ushort) item.createTile, item.placeStyle))
                    goto noReplacement;
            }
            else if (typeOfItem == TypeOfItem.Wall) {
                if (!WorldGen.ReplaceWall(x, y, (ushort) item.createWall))
                    goto noReplacement;
            }

            return true;
        }

        noReplacement:
        bool tilePlaced = false;
        if (typeOfItem == TypeOfItem.Tile && (forced || !tile.HasTile))
            tilePlaced = WorldGen.PlaceTile(x, y, item.createTile, mute, forced, style: item.placeStyle);
        else if (typeOfItem == TypeOfItem.Wall) {
            WorldGen.PlaceWall(x, y, item.createWall, mute);
            tilePlaced = true; //LOL, no way of knowing?
        }

        if (sync && Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(-1, x, y, 1);

        return tilePlaced;
    }

    public static void PlaceTilesInArea(Point start, Point end, Item item, bool mute = false, bool forced = false,
        bool sync = true) {
        if (!ValidTileCoordinates(start.X, start.Y) || !ValidTileCoordinates(end.X, end.Y))
            return;

        int dx = Math.Abs(start.X - end.X);
        int dy = Math.Abs(start.Y - end.Y);
        int minX = Math.Min(start.X, end.X);
        int minY = Math.Min(start.Y, end.Y);

        for (int y = minY; y < minY + dy; y++)
        for (int x = minX; x < minX + dx; x++)
            PlaceTile(x, y, item, mute, forced, false);

        //Keeping syncSize an odd number since SendTileSquare has a bias towards up and left for even-numbers sizes
        int syncSize = Math.Max(dx, dy);
        syncSize += 1 + syncSize % 2;

        if (sync && Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(-1, minX, minY, syncSize);
    }

    public static bool RemoveTile(int x, int y, bool removeTile = true, bool removeWall = false, bool dropItem = true,
        bool sync = true, bool needPickPower = false) {

        if (!ValidTileCoordinates(x, y))
            return false;
        if (needPickPower && !Main.LocalPlayer.HasEnoughPickPowerToHurtTile(x, y))
            return false;

        Tile tile = Framing.GetTileSafely(x, y);
        int itemType = ItemPicker.PickItem(tile);

        if (Main.netMode == NetmodeID.MultiplayerClient) {
            int itemID = Item.NewItem(new EntitySource_DropAsItem(null), x * 16, y * 16, 16, 16, itemType, noBroadcast: true);
            NetMessage.SendData(MessageID.SyncItem, number: itemID);
        }

        if (removeTile && WorldGen.CanKillTile(x, y, out _))
            WorldGen.KillTile(x, y, noItem: !dropItem);

        //Can't not drop items when killing walls for some reason
        if (removeWall)
            WorldGen.KillWall(x, y);

        if (sync && Main.netMode == NetmodeID.MultiplayerClient) { NetMessage.SendTileSquare(-1, x, y, 1); }

        return true;
    }

    public static bool RemoveTileWithMask(int x, int y, int maskItemType, bool dropItem = true, bool sync = true, bool needPickPower = false) {
        Item item = new Item(maskItemType);
        TypeOfItem typeOfItem = WhatIsThisItem(item);
        Tile tile = Main.tile[x, y];

        bool canRemoveItem = ((tile.HasTile && tile.TileType == item.createTile) ||
            (!tile.HasTile && tile.WallType == item.createWall) ||
            (typeOfItem == TypeOfItem.Air));

        if (!canRemoveItem)
            return false;

        bool removeTile = typeOfItem == TypeOfItem.Tile || typeOfItem == TypeOfItem.Air;
        bool removeWall = typeOfItem == TypeOfItem.Wall || typeOfItem == TypeOfItem.Air;
        
        return RemoveTile(x, y, removeTile, removeWall, dropItem, sync, needPickPower);
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using BuilderEssentials.Common.DataStructures;
using BuilderEssentials.Content.Items;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ObjectData;

namespace BuilderEssentials.Common;

public static class MirrorPlacementDetours
{
	public static Point16 MirrorPlacementAction(Action<Point16> action, Point16 tileCoords = default, Item item = null,
		bool shouldReduceStack = false, int amount = 1) {
		if (Main.dedServ) return tileCoords;
		item ??= Main.LocalPlayer.HeldItem;

		var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
		if (panel.IsVisible && panel.IsMouseWithinSelection()) {
			Point16 mirroredCoords = panel.GetMirroredTileTargetCoordinate(tileCoords.ToVector2(), item.createTile,
				item.placeStyle, Main.LocalPlayer.direction).ToPoint16();

			Player.tileTargetX = mirroredCoords.X;
			Player.tileTargetY = mirroredCoords.Y;
			
			if (PlacementHelpers.CanReduceItemStack(item.type, amount, shouldReduceStack, true)) {
				if (tileCoords != mirroredCoords)
					action?.Invoke(mirroredCoords);
			}

			return mirroredCoords;
		}

		return tileCoords;
	}
	
	public static void PlayerPostUpdate() {
		//TODO: MP Sync
		while (queue.Count != 0) {
			Tuple<Point, Item> dequeue = queue.Dequeue();
			Point coord = dequeue.Item1;
			Item item = dequeue.Item2;
			
			Tile tile = Main.tile[coord.X, coord.Y];
			TypeOfItem typeOfItem = PlacementHelpers.WhatIsThisItem(item);
			
			TileObject tileObject = new TileObject() {
				type = typeOfItem == TypeOfItem.Tile ? item.createTile : item.createWall,
				style = item.placeStyle,
			};
			TileObjectData tileData = TileObjectData.GetTileData(tileObject.type, tileObject.style, tileObject.alternate);

			TileData[] placedTile = new TileData[1];
			Point tileSize = Point.Zero;
			
			if (tileData != null) {
				tileSize = new Point(tileData.CoordinateFullWidth / 16, tileData.CoordinateFullHeight / 16);
				placedTile = new TileData[tileSize.X * tileSize.Y];
				
				for (int x = 0; x < tileSize.X; x++)
				for (int y = 0; y < tileSize.Y; y++) {
					int k = tileSize.X * y + x;

					Point tileCoord = coord - tileData.Origin.ToPoint() + new Point(x, y);
					Tile iterTile = Main.tile[tileCoord.X, tileCoord.Y];
					placedTile[k] = new TileData(iterTile, tileCoord);
				}
			}
			else placedTile[0] = new TileData(tile, coord);
			
			MirrorPlacementAction(mirroredCoords => {
				Tile mirroredTile = Main.tile[mirroredCoords.X, mirroredCoords.Y];

				if (tileData != null) {
					Point16 topLeftTile = mirroredCoords - tileData.Origin;

					Console.WriteLine(topLeftTile);

					if (tile.TileType == TileID.TallGateClosed) {

						//iterTile.TileFrameY += (short) (18 * tileSize.Y * -Main.LocalPlayer.direction);
						
						return;
					}
					
					for (int x = 0; x < tileSize.X; x++)
					for (int y = 0; y < tileSize.Y; y++) {
						int k = tileSize.X * y + x;

						Point16 tileCoord = topLeftTile + new Point16(x, y);
						Tile iterTile = Main.tile[tileCoord.X, tileCoord.Y];
						
						if (tile.TileType == TileID.DisplayDoll || tile.TileType == TileID.Mannequin ||
						    tile.TileType == TileID.Womannequin || tile.TileType == TileID.ClosedDoor) {
							placedTile[k].CopyToTile(iterTile);
							iterTile.TileFrameX += (short) (18 * tileSize.X * -Main.LocalPlayer.direction);
							continue;
						}

						if (tileData.AlternatesCount > 0) {
							Main.LocalPlayer.direction *= -1;
							//PlaceTile's x/y are the tile's placement origin
							WorldGen.PlaceTile(mirroredCoords.X, mirroredCoords.Y, placedTile[k].TileTypeData.Type,
								plr: Main.LocalPlayer.whoAmI, style: TileObjectData.GetTileStyle(tile));
							Main.LocalPlayer.direction *= -1;
						}
						else placedTile[k].CopyToTile(iterTile);
					}
					
					//Handling specific cases with Tile Entities -> TEItemFrame, TEFoodPlatter, TEWeaponsRack, TEDisplayDoll, TEHatRack
					if (tile.TileType == 21)
						Chest.CreateChest(topLeftTile.X, topLeftTile.Y);
					
					if (tile.TileType == TileID.DisplayDoll || tile.TileType == TileID.Mannequin || tile.TileType == TileID.Womannequin)
						TEDisplayDoll.Place(topLeftTile.X, topLeftTile.Y);

					if (tile.TileType == TileID.WeaponsRack || tile.TileType == TileID.WeaponsRack2) {
						TEWeaponsRack.Place(topLeftTile.X, topLeftTile.Y);
					}
				}
				else {
					placedTile[0].CopyToTile(mirroredTile);
					WorldGen.SquareTileFrame(mirroredCoords.X, mirroredCoords.Y, true);
					WorldGen.SquareWallFrame(mirroredCoords.X, mirroredCoords.Y, true);
				}
				
				Console.WriteLine("new: " + GetTopLeftCoordOfTile(mirroredCoords.X, mirroredCoords.Y));
			}, new Point16(coord.X, coord.Y));
		}
	}

	public static UniqueQueue<Tuple<Point, Item>> queue = new();
	public static void LoadDetours() {
		//ApplyitemTime called, queue mirror placement based on HeldItem.
		//Tile has not been placed yet when this runs
		//Dequeue placement in an update method after

		On.Terraria.Player.ApplyItemTime += (orig, player, item, multiplier, useItem) => {
			orig.Invoke(player, item, multiplier, useItem);

			//Only want tile placements
			if (item.createTile < TileID.Dirt && item.createWall < WallID.Stone) return;
			queue.Enqueue(new Tuple<Point, Item>(new Point(Player.tileTargetX, Player.tileTargetY), item));
		};
	}

	
	// if (tileData.AlternatesCount > 0) {
	// 	//Alternate placements -> tileFrameX?
	// 	//Styles -> tileFrameY?
	// 	
	// }
	// else {
	// 	//Styles -> tileFrameX?
	// 	//Animation frames -> tileFrameY
	// }
	
	//TODO: Innacurate for relic tiles
	public static Point GetTopLeftCoordOfTile(int x, int y) {
		Tile tile = Main.tile[x, y];
		TileObjectData tileData = TileObjectData.GetTileData(tile);

		int tileFrameX = tile.TileFrameX;
		int tileFrameY = tile.TileFrameY;

		if (tileData != null) {
			//Ignoring styles
			tileFrameX %= tileData.CoordinateFullWidth;
			tileFrameY %= tileData.CoordinateFullHeight; 
		}

		return new Point(x - tileFrameX / 18, y - tileFrameY / 18);
	}
	
	//Thanks jopo https://github.com/JavidPack/CheatSheet/blob/1.4/TileData.cs
	public readonly record struct TileData(TileTypeData TileTypeData, WallTypeData WallTypeData,
		TileWallWireStateData TileWallWireStateData, LiquidData LiquidData, Point coord)
	{
		public TileData(Tile tile, Point coord) : this(tile.Get<TileTypeData>(), tile.Get<WallTypeData>(), 
			tile.Get<TileWallWireStateData>(), tile.Get<LiquidData>(), coord) {
		}

		public void CopyToTile(Tile tile) {
			tile.Get<TileTypeData>() = TileTypeData;
			tile.Get<WallTypeData>() = WallTypeData;
			tile.Get<TileWallWireStateData>() = TileWallWireStateData;
			tile.Get<LiquidData>() = LiquidData;
		}
	}
}
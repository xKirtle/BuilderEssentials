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
		if (panel.IsVisible && panel.validMirrorPlacement && panel.IsMouseWithinSelection() && panel.IsMouseAffectedByMirrorAxis()) {
			Point16 mirroredCoords = panel.GetMirroredTileTargetCoordinate(tileCoords.ToVector2(), item.createTile,
				item.placeStyle, Main.LocalPlayer.direction).ToPoint16();

			if (PlacementHelpers.CanReduceItemStack(item.type, amount, shouldReduceStack, true)) {
				if (tileCoords != mirroredCoords)
					action?.Invoke(mirroredCoords);
			}

			return mirroredCoords;
		}

		return tileCoords;
	}

	public static UniqueQueue<Tuple<Point, Item>> tilePlacementsQueue = new();
	public static void QueuedTilePlacements() {
		while (tilePlacementsQueue.Count != 0) {
			Tuple<Point, Item> dequeue = tilePlacementsQueue.Dequeue();
			Point placementCoords = dequeue.Item1;
			Item item = dequeue.Item2;
			
			Point topLeft = GetTopLeftCoordOfTile(placementCoords.X, placementCoords.Y);
			Tile tile = Main.tile[placementCoords.X, placementCoords.Y];
			int tileStyle = TileObjectData.GetTileStyle(tile);
			TypeOfItem typeOfItem = PlacementHelpers.WhatIsThisItem(item);

			bool isTilePlacement = typeOfItem == TypeOfItem.Tile;
			bool isWallPlacement = typeOfItem == TypeOfItem.Wall;
			bool isWirePlacement = true; //TODO: Hardcode item.types?
			bool isLiquidPlacement = true; //Also hardocde item.types like buckets?

			TileObject tileObject = new TileObject() {
				type = isTilePlacement ? item.createTile : item.createWall,
				style = item.placeStyle
			};

			TileObjectData tileData = null;
			if (isTilePlacement)
				tileData = TileObjectData.GetTileData(tileObject.type, tileObject.style, tileObject.alternate);

			TileData[] placedTile = new TileData[1];
			Point tileSize = Point.Zero;

			if (tileData != null) {
				tileSize = new Point(tileData.CoordinateFullWidth / 18, tileData.CoordinateFullHeight / 18);
				placedTile = new TileData[tileSize.X * tileSize.Y];
				
				//Saving tiles data
				for (int x = 0; x < tileSize.X; x++)
				for (int y = 0; y < tileSize.Y; y++) {
					int k = tileSize.X * y + x;

					Point tileCoord = topLeft + new Point(x, y);
					Tile iterTile = Main.tile[tileCoord.X, tileCoord.Y];
					placedTile[k] = new TileData(iterTile, tileCoord);
				}
			}
			else placedTile[0] = new TileData(tile, placementCoords);
			
			MirrorPlacementAction(mirroredCoords => {
				Tile mirroredTile = Main.tile[mirroredCoords.X, mirroredCoords.Y];
				Point mirroredTopLeft = mirroredCoords.ToPoint() - (placementCoords - topLeft);
				
				if (tileData != null) {
					//Copying saved tiles data to mirrored coordinates
					for (int x = 0; x < tileSize.X; x++)
					for (int y = 0; y < tileSize.Y; y++) {
						int k = tileSize.X * y + x;

						Point tileCoord = mirroredTopLeft + new Point(x, y);
						Tile iterTile = Main.tile[tileCoord.X, tileCoord.Y];
						
						//Tiles with a rotated variant
						if (tile.TileType == TileID.DisplayDoll || tile.TileType == TileID.Mannequin || 
						    tile.TileType == TileID.Womannequin || tile.TileType == TileID.HatRack ||
						    tile.TileType == TileID.TargetDummy) {
							placedTile[k].CopyToTile(iterTile, isTilePlacement, 
								isWallPlacement, isWirePlacement, isLiquidPlacement);
							iterTile.TileFrameX += (short) (18 * tileSize.X * -Main.LocalPlayer.direction);
							continue;
						}

						//Tiles breaking with WorldGen.PlaceTile (why?)
						if (tile.TileType == TileID.ClosedDoor || tile.TileType == TileID.TallGateClosed ||
						    tile.TileType == TileID.MasterTrophyBase || tile.TileType == TileID.ItemFrame ||
						    tile.TileType == TileID.WeaponsRack || tile.TileType == TileID.WeaponsRack2 ||
						    tile.TileType == TileID.LogicSensor) {
							placedTile[k].CopyToTile(iterTile, isTilePlacement, 
								isWallPlacement, isWirePlacement, isLiquidPlacement);
							continue;
						}

						//Spritesheet is different than other tiles with rotated variants
						if (tile.TileType == TileID.Statues) {
							placedTile[k].CopyToTile(iterTile, isTilePlacement, 
								isWallPlacement, isWirePlacement, isLiquidPlacement);
							iterTile.TileFrameY += (short) (162 * -Main.LocalPlayer.direction);
							continue;
						}

						if (tileData.AlternatesCount > 0) {
							Main.LocalPlayer.direction *= -1;
							//PlaceTile's x/y are the tile's placement origin
							WorldGen.PlaceTile(mirroredCoords.X, mirroredCoords.Y, placedTile[k].TileTypeData.Type,
								plr: Main.LocalPlayer.whoAmI, style: tileStyle);
							Main.LocalPlayer.direction *= -1;

							goto specificTileEntitiesCases;
						}
						else
							placedTile[k].CopyToTile(iterTile, isTilePlacement, 
								isWallPlacement, isWirePlacement, isLiquidPlacement);
					}
					
					//Handling specific cases with Tile Entities
					specificTileEntitiesCases:
					if (tile.TileType == 21)
						Chest.AfterPlacement_Hook(mirroredTopLeft.X, mirroredTopLeft.Y);
					
					if (tile.TileType == TileID.DisplayDoll || tile.TileType == TileID.Mannequin || tile.TileType == TileID.Womannequin)
						TEDisplayDoll.Hook_AfterPlacement(mirroredTopLeft.X, mirroredTopLeft.Y + 2);

					if (tile.TileType == TileID.ItemFrame)
						TEItemFrame.Hook_AfterPlacement(mirroredTopLeft.X, mirroredTopLeft.Y);
					
					if (tile.TileType == TileID.WeaponsRack || tile.TileType == TileID.WeaponsRack2)
						TEWeaponsRack.Hook_AfterPlacement(mirroredTopLeft.X, mirroredTopLeft.Y);

					if (tile.TileType == TileID.HatRack)
						TEHatRack.Hook_AfterPlacement(mirroredTopLeft.X + 1, mirroredTopLeft.Y + 3);
					
					if (tile.TileType == TileID.TargetDummy)
						TETrainingDummy.Hook_AfterPlacement(mirroredTopLeft.X + 1, mirroredTopLeft.Y + 2);

					if (tile.TileType == TileID.LogicSensor)
						TELogicSensor.Hook_AfterPlacement(mirroredTopLeft.X, mirroredTopLeft.Y);
					
					if (tile.TileType == TileID.FoodPlatter)
						TEFoodPlatter.Hook_AfterPlacement(mirroredTopLeft.X, mirroredTopLeft.Y);
					
					//TODO: Check pylons. Should it be added to map as well?
					}
				else {
					placedTile[0].CopyToTile(mirroredTile, isTilePlacement, 
						isWallPlacement, isWirePlacement, isLiquidPlacement);
					WorldGen.SquareTileFrame(mirroredCoords.X, mirroredCoords.Y, true);
					WorldGen.SquareWallFrame(mirroredCoords.X, mirroredCoords.Y, true);
				}

				if (Main.myPlayer == Main.LocalPlayer.whoAmI && Main.netMode == NetmodeID.MultiplayerClient) {
					NetMessage.SendObjectPlacment(Main.LocalPlayer.whoAmI, mirroredCoords.X, mirroredCoords.Y,
						tileObject.type, tileObject.style, tileObject.alternate, tileObject.random,
						Main.LocalPlayer.direction * -1);
				}
			}, new Point16(placementCoords.X, placementCoords.Y));
		}
	}
	
	public static void LoadDetours() {
		//ApplyitemTime called, queue mirror placement based on HeldItem.
		//Tile has not been placed yet when this runs
		//Dequeue placement in an update method after

		On.Terraria.Player.ApplyItemTime += (orig, player, item, multiplier, useItem) => {
			orig.Invoke(player, item, multiplier, useItem);

			if (item.createTile >= TileID.Dirt || item.createWall >= WallID.Stone) {
				tilePlacementsQueue.Enqueue(new Tuple<Point, Item>(new Point(Player.tileTargetX, Player.tileTargetY), item));
			}

			if (item.hammer > 0) {
				
			}
		};

		On.Terraria.TileObject.DrawPreview += (orig, spriteBatch, previewData, position) => {
			orig.Invoke(spriteBatch, previewData, position);

			MirrorPlacementAction(mirroredCoords => {
				TileObjectData data = TileObjectData.GetTileData(previewData.Type, previewData.Style, previewData.Alternate);
				previewData.Coordinates = mirroredCoords - data.Origin;
				previewData.Alternate = Main.LocalPlayer.direction == 1 ? 0 : 1;
				orig.Invoke(spriteBatch, previewData, position);
			});
		};
	}
	
	public static void PlayerPostUpdate() {
    	QueuedTilePlacements();
    }
	
	
	
	//Not sure where to put this yet
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
	
	public readonly record struct TileData(TileTypeData TileTypeData, WallTypeData WallTypeData,
		TileWallWireStateData TileWallWireStateData, LiquidData LiquidData, Point coord)
	{
		public TileData(Tile tile, Point coord) : this(tile.Get<TileTypeData>(), tile.Get<WallTypeData>(), 
			tile.Get<TileWallWireStateData>(), tile.Get<LiquidData>(), coord) {
		}

		public void CopyToTile(Tile tile, bool tileData = false, bool wallData = false, bool wireData = false, bool liquidData = false) {
			var newTileData = tile.Get<TileWallWireStateData>();
			
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

			tile.Get<TileWallWireStateData>() = newTileData;

			if (liquidData)
				tile.Get<LiquidData>() = LiquidData;
		}
	}
}
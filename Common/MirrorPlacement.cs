using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuilderEssentials.Common.DataStructures;
using BuilderEssentials.Content.Items;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Common;

public static class MirrorPlacement
{
	static readonly List<int> PaintToolsItemTypes = new() {
		ItemID.Paintbrush, ItemID.SpectrePaintbrush,
		ItemID.PaintRoller, ItemID.SpectrePaintRoller,
		ItemID.PaintScraper, ItemID.SpectrePaintScraper
	};
	
	public static void PlayerPostUpdate() {
		QueuedTilePlacements();
		QueuedHammerTile();
		QueuedTilePaint();
	}

	internal static UniqueQueue<Tuple<Point, Item>> tilePlacementsQueue = new();
	public static void QueuedTilePlacements() {
		while (tilePlacementsQueue.Count != 0) {
			(Point placementCoords, Item item) = tilePlacementsQueue.Dequeue();

			Point topLeft = GetTopLeftCoordOfTile(placementCoords.X, placementCoords.Y);
			Tile tile = Main.tile[placementCoords.X, placementCoords.Y];
			int tileStyle = TileObjectData.GetTileStyle(tile);
			TypeOfItem typeOfItem = PlacementHelpers.WhatIsThisItem(item);

			bool isTilePlacement = typeOfItem == TypeOfItem.Tile;
			bool isWallPlacement = typeOfItem == TypeOfItem.Wall;
			bool isWirePlacement = true; //TODO: Hardcode item.types? ItemCheck_UseWiringTools(item)
			bool isLiquidPlacement = true; //Also hardocde item.types like buckets? ItemCheck_UseBuckets(item)

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
					
					if (Main.netMode == NetmodeID.MultiplayerClient) {
						NetMessage.SendObjectPlacment(-1, mirroredCoords.X, mirroredCoords.Y,
							tileObject.type, tileObject.style, tileObject.alternate, tileObject.random,
							Main.LocalPlayer.direction * -1);
					}
				}
				else {
					placedTile[0].CopyToTile(mirroredTile, isTilePlacement, 
						isWallPlacement, isWirePlacement, isLiquidPlacement);
					WorldGen.SquareTileFrame(mirroredCoords.X, mirroredCoords.Y, true);
					WorldGen.SquareWallFrame(mirroredCoords.X, mirroredCoords.Y, true);
					
					if (Main.netMode == NetmodeID.MultiplayerClient)
						NetMessage.SendTileSquare(-1, mirroredCoords.X, mirroredCoords.Y, 1);
				}

				
			}, new Point16(placementCoords.X, placementCoords.Y), item, shouldReduceStack: true);
		}
	}

	internal static UniqueQueue<MinimalTileData> hammerTileQueue = new();
	public static void QueuedHammerTile() {
		while (hammerTileQueue.Count != 0) {
			MinimalTileData data = hammerTileQueue.Dequeue();
			Point coords = data.coord;
            Tile tile = Main.tile[coords.X, coords.Y];

            //Data is the information before the hammer was used
            if (data.Slope == tile.Slope && data.IsHalfBlock == tile.IsHalfBlock) return;

            MirrorPlacementAction(mirroredCoords => {
	            int[] mirroredSlopes = new[] {0, 2, 1, 4, 3};
	            AutoHammer.ChangeSlope(mirroredCoords, (SlopeType) mirroredSlopes[(int) tile.Slope], tile.IsHalfBlock);
            }, new Point16(coords));
		}
	}

	internal static UniqueQueue<Tuple<Point, Item>> paintTileQueue = new();
	public static void QueuedTilePaint() {
		while (paintTileQueue.Count != 0) {
			(Point coords, Item item) = paintTileQueue.Dequeue();
			Tile tile = Main.tile[coords.X, coords.Y];
			
			MirrorPlacementAction(mirredCoords => {
				int paintTool = (PaintToolsItemTypes.IndexOf(item.type) / 2);
				bool paintTile = paintTool == 0,  paintWall = paintTool == 1, scrapPaint = paintTool == 2;
				
				if (paintTile || paintWall)
					BasePaintBrush.PaintTileOrWall(paintTile ? tile.TileColor : tile.WallColor, paintTool, mirredCoords.ToPoint());
				else if (scrapPaint)
					BasePaintBrush.ScrapPaint(mirredCoords.ToPoint());
			});
		}
	}


	public static void LoadDetours() {
		//Runs before ApplyItemTime
		// On.Terraria.Player.ApplyItemAnimation_Item += (orig, player, item) => {
		// 	orig.Invoke(player, item);
		// 	Console.WriteLine("Apply Item Animation");
		// };
		
		On.Terraria.Player.ApplyItemTime += (orig, player, item, multiplier, useItem) => {
			orig.Invoke(player, item, multiplier, useItem);
			if (Main.netMode == NetmodeID.Server) return;
			
			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (!panel.IsVisible || !panel.validMirrorPlacement || 
			    !panel.IsMouseWithinSelection() || !panel.IsMouseAffectedByMirrorAxis()) return;
			
			Point coord = new Point(Player.tileTargetX, Player.tileTargetY);
			Tile tile = Main.tile[coord.X, coord.Y];
			MinimalTileData data = new MinimalTileData(tile, coord);
			TileData tileData = new TileData(tile, coord);
			
			if (item.createTile >= TileID.Dirt || item.createWall >= WallID.Stone)
				tilePlacementsQueue.Enqueue(new Tuple<Point, Item>(coord, item));

			//ApplyItemTime not called when hammer breaks walls?
			if (item.hammer > 0)
				hammerTileQueue.Enqueue(data);

			if (PaintToolsItemTypes.Contains(item.type))
				paintTileQueue.Enqueue(new Tuple<Point, Item>(coord, item));
		};

		On.Terraria.WorldGen.ReplaceTile += (orig, x, y, type, style) => {
			bool baseReturn = orig.Invoke(x, y, type, style);
			
			//Hacky solution since stack was not yet decreased from above call
			//Going with the queue route seems to not be working/dropping items?
			//Perhaps it's got to do with call order but idc
			Item item = Main.LocalPlayer.HeldItem;
			item.stack -= 1;
			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(mirroredCoords.X, mirroredCoords.Y, type, style);
			}, new Point16(x, y), item, false, 1);
			item.stack += 1;

			return baseReturn;
		};

		On.Terraria.WorldGen.ReplaceWall += (orig, x, y, type) => {
			bool baseReturn = orig.Invoke(x, y, type);
			
			//Same as above
			Item item = Main.LocalPlayer.HeldItem;
			item.stack -= 1;
			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(mirroredCoords.X, mirroredCoords.Y, type);
			}, new Point16(x, y), item, false, 1);
			item.stack += 1;
			
			return baseReturn;
		};

		//Remove vanilla behaviour that auto places walls (if available) to fill 3x1 spaces
		On.Terraria.Player.PlaceThing_Walls_FillEmptySpace += (orig, player) => {
			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.validMirrorPlacement && panel.IsMouseWithinSelection() && panel.IsMouseAffectedByMirrorAxis()) {
				//Do nothing
			}
			else orig.Invoke(player);
		};
		
		//Remove vanilla behaviour that hammers walls around the tile target based on coord inside tile targetted
		On.Terraria.Player.ItemCheck_UseMiningTools_TryFindingWallToHammer += (
			On.Terraria.Player.orig_ItemCheck_UseMiningTools_TryFindingWallToHammer orig, out int x, out int y) => {
			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.validMirrorPlacement && panel.IsMouseWithinSelection() && panel.IsMouseAffectedByMirrorAxis()) {
				x = Player.tileTargetX;
				y = Player.tileTargetY;
			}
			else orig.Invoke(out x, out y);
		};
		
		On.Terraria.Player.ItemCheck_UseMiningTools_TryHittingWall += (orig, player, item, x, y) => {
			orig.Invoke(player, item, x, y);
			
			MirrorPlacementAction(mirroredCoords => {
				player.controlUseItem = true;
				player.releaseUseItem = false;
		
				orig.Invoke(player, item, mirroredCoords.X, mirroredCoords.Y);
			}, new Point16(x, y));
		};
		
		On.Terraria.Player.PickTile += (orig, player, x, y, power) => {
			orig.Invoke(player, x, y, power);
			
			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(player, mirroredCoords.X, mirroredCoords.Y, power);
			}, new Point16(x, y));
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
}
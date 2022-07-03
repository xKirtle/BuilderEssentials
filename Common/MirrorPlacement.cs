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
			Point tileSize = new Point(1, 1);

			if (tileData != null) {
				tileSize = new Point(tileData.CoordinateFullWidth / 16, tileData.CoordinateFullHeight / 16);
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
				
				//Check for overlap with other existing tiles
				TileObject canPlaceObject = TileObject.Empty;
				if (isTilePlacement && (mirroredTile.HasTile || (!TileObject.CanPlace(mirroredCoords.X, mirroredCoords.Y, tileObject.type,
					    tileObject.style, Main.LocalPlayer.direction, out canPlaceObject, onlyCheck: true) && canPlaceObject.type != 0)) ||
				    isWallPlacement && mirroredTile.WallType >= WallID.Stone) return;
				
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
						    tile.TileType == TileID.TargetDummy || tile.TileType == TileID.Traps) {
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
						
						//Platforms
						if (tile.TileType == TileID.Platforms)
							WorldGen.PlatformProperSides(mirroredCoords.X, mirroredCoords.Y);

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
						Chest.AfterPlacement_Hook(mirroredCoords.X, mirroredCoords.Y);
					
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

				//Only reduce stack at the end, in case some condition prevents placement
				PlacementHelpers.CanReduceItemStack(item.type, 1, reduceStack: true, shouldBeHeld: true);
			}, new Point16(placementCoords.X, placementCoords.Y), item, shouldReduceStack: false);
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
			//Getting tile info before the replacement
			Tile tile = Main.tile[x, y];
			int itemType = ItemPicker.PickItem(tile);
			
			bool baseReturn = orig.Invoke(x, y, type, style);
			
			//Hacky solution since stack was not yet decreased from above call
			//Going with the queue route seems to not be working/dropping items?
			//Perhaps it's got to do with call order but idc
			Item item = Main.LocalPlayer.HeldItem;
			item.stack -= 1;
			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(mirroredCoords.X, mirroredCoords.Y, type, style);
				
				//Why are vanilla replace methods not dropping items in MP :(
				if (Main.netMode == NetmodeID.MultiplayerClient) {
					int itemID = Item.NewItem(new EntitySource_DropAsItem(null), mirroredCoords.X * 16,
						mirroredCoords.Y * 16, 16, 16, itemType, noBroadcast: true);
					NetMessage.SendData(MessageID.SyncItem, number: itemID);
				}
			}, new Point16(x, y), item, true, 1);
			item.stack += 1;

			return baseReturn;
		};

		On.Terraria.WorldGen.ReplaceWall += (orig, x, y, type) => {
			//Getting tile info before the replacement
			Tile tile = Main.tile[x, y];
			int itemType = ItemPicker.PickItem(tile);
			
			bool baseReturn = orig.Invoke(x, y, type);
			
			//Same hacky method as above
			Item item = Main.LocalPlayer.HeldItem;
			item.stack -= 1;
			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(mirroredCoords.X, mirroredCoords.Y, type);
				
				//Why are vanilla replace methods not dropping items in MP :(
				if (Main.netMode == NetmodeID.MultiplayerClient) {
					int itemID = Item.NewItem(new EntitySource_DropAsItem(null), mirroredCoords.X * 16,
						mirroredCoords.Y * 16, 16, 16, itemType, noBroadcast: true);
					NetMessage.SendData(MessageID.SyncItem, number: itemID);
				}
			}, new Point16(x, y), item, true, 1);
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
				//TODO: Check if this is needed
				player.controlUseItem = true;
				player.releaseUseItem = false;
		
				orig.Invoke(player, item, mirroredCoords.X, mirroredCoords.Y);
			}, new Point16(x, y));
		};
		
		On.Terraria.Player.PickTile += (orig, player, x, y, power) => {
			orig.Invoke(player, x, y, power);
			
			MirrorPlacementAction(mirroredCoords => {
				Tile mirroredTile = Main.tile[mirroredCoords.X, mirroredCoords.Y];
				if (TileID.Sets.IsATreeTrunk[mirroredTile.TileType] ||
				    TileID.Sets.CountsAsGemTree[mirroredTile.TileType]) return;
				
				orig.Invoke(player, mirroredCoords.X, mirroredCoords.Y, power);
			}, new Point16(x, y));
		};
		
		On.Terraria.TileObject.DrawPreview += (orig, spriteBatch, pData, position) => {
			DrawPreview(spriteBatch, pData, position);

			MirrorPlacementAction(mirroredCoords => {
				TileObjectData tileData = TileObjectData.GetTileData(pData.Type, pData.Style, pData.Alternate);
				pData.Coordinates = mirroredCoords - tileData.Origin;
				pData.Alternate = Main.LocalPlayer.direction == 1 ? 0 : 1;
				TileObject.CanPlace(mirroredCoords.X, mirroredCoords.Y, pData.Type, 
					pData.Style, pData.Alternate, out _, onlyCheck: true);
				
				DrawPreview(spriteBatch, pData, position, 0.5f);
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
	public static Point GetTopLeftCoordOfTile(int x, int y, bool isPlaced = true) {
		Tile tile = Main.tile[x, y];
		TileObjectData tData = TileObjectData.GetTileData(tile);

		if (!isPlaced)
			return new Point(x - tData?.Origin.X ?? 0, y - tData?.Origin.Y ?? 0);

		int tileFrameX = tile.TileFrameX;
		int tileFrameY = tile.TileFrameY;
		Point drawOffset = Point.Zero;

		if (tData != null) {
			//Ignoring styles
			tileFrameX %= tData.CoordinateFullWidth;
			tileFrameY %= tData.CoordinateFullHeight;
			drawOffset = new Point(0, tData.DrawStepDown);
		}

		return new Point(x - tileFrameX / 16 - drawOffset.X, y - tileFrameY / 16 - drawOffset.Y);
	}
	
	public static void DrawPreview(SpriteBatch sb, TileObjectPreviewData pData, Vector2 pos, float colorMultiplier = 1f) {
		Point16 tileCoords = pData.Coordinates;
		Texture2D tileTexture = TextureAssets.Tile[pData.Type].Value;
		TileObjectData tData = TileObjectData.GetTileData(pData.Type, pData.Style, pData.Alternate);
		if (tData == null) return;
		
		int tileWidth = 0, tileHeight = 0, styleMultiplier = 0, alternateMultiplier = 0;
		styleMultiplier = tData.CalculatePlacementStyle(pData.Style, pData.Alternate, pData.Random) + tData.DrawStyleOffset;
		int drawXOffset = tData.DrawXOffset, drawYOffset = tData.DrawYOffset;

		int styleWrapLimit = tData.StyleWrapLimitVisualOverride.HasValue
			? tData.StyleWrapLimitVisualOverride.Value : tData.StyleWrapLimit;

		int styleLineSkip = tData.styleLineSkipVisualOverride.HasValue
			? tData.styleLineSkipVisualOverride.Value : tData.StyleLineSkip;

		if (styleWrapLimit > 0) {
			alternateMultiplier = styleMultiplier / styleWrapLimit * styleLineSkip;
			styleMultiplier %= styleWrapLimit;
		}

		tileWidth = tData.CoordinateFullWidth * (tData.StyleHorizontal ? styleMultiplier : alternateMultiplier);
		tileHeight = tData.CoordinateFullHeight * (tData.StyleHorizontal ? alternateMultiplier : styleMultiplier);

		for (int i = 0; i < pData.Size.X; i++) {
			int x = tileWidth + (i - pData.ObjectStart.X) * (tData.CoordinateWidth + tData.CoordinatePadding);
			int y = tileHeight;

			for (int j = 0; j < pData.Size.Y; j++) {
				Point pCoord = new Point(tileCoords.X + i, tileCoords.Y + j);
				
				//DrawStepDown messing me with banners?
				if (j == 0 && tData.DrawStepDown != 0 && WorldGen.SolidTile(Main.tile[pCoord.X, pCoord.Y - 1]))
					drawYOffset += tData.DrawStepDown;

				if (pData.Type == TileID.GardenGnome)
					drawYOffset = ((j != 0) ? tData.DrawYOffset : (tData.DrawYOffset - 2));
				
				//Uses preview's data size, not tile coordinates
				Color drawColor = (pData[i, j] == 1 ? Color.White : Color.Red * 0.7f) * 0.5f * colorMultiplier;

				if (CoordSelection.IsWithinRange(i, pData.ObjectStart.X, pData.ObjectStart.X + tData.Width - 1, true) &&
				    CoordSelection.IsWithinRange(j, pData.ObjectStart.Y, pData.ObjectStart.Y + tData.Height - 1, true)) {
					SpriteEffects spriteEffects = SpriteEffects.None;
					if (tData.DrawFlipHorizontal && pCoord.X % 2 == 0)
						spriteEffects |= SpriteEffects.FlipHorizontally;

					if (tData.DrawFlipVertical && pCoord.Y % 2 == 0)
						spriteEffects |= SpriteEffects.FlipVertically;

					int currentCoordHeight = tData.CoordinateHeights[j - pData.ObjectStart.Y];
					if (pData.Type == TileID.TinkerersWorkbench && j == 1)
						currentCoordHeight += 2;

					Vector2 position = (pCoord.ToVector2() * 16) - pos +
					                   new Vector2((tData.CoordinateWidth - 16) / 2f, 0) +
					                   new Vector2(drawXOffset, drawYOffset);

					Rectangle sourceRectangle = new Rectangle(x, y, tData.CoordinateWidth, currentCoordHeight);
					sb.Draw(tileTexture, position, sourceRectangle, drawColor, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
					y += currentCoordHeight + tData.CoordinatePadding;
				}
			}
		}
	}
}
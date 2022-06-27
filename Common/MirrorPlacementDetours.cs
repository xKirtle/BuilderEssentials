using System;
using System.Threading.Tasks;
using BuilderEssentials.Content.Items;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace BuilderEssentials.Common;

public static class MirrorPlacementDetours
{
	//Prevents infinite loop in ApplyItemTime
	private static Point16 oldMirror = default;
	internal static void MirrorPlacementAction(Action<Point16> action, Vector2 tileCoords = default, Item item = null, bool shouldReduceStack = false) {
		if (WorldGen.gen || Main.dedServ) return;
		
		var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
		if (panel.IsVisible && panel.IsMouseWithinSelection()) {
			Point16 mirroredCoords = panel.GetMirroredTileTargetCoordinate(tileCoords, item?.createTile ?? 0,
				item?.placeStyle ?? 0, Main.LocalPlayer.direction).ToPoint16();
			
			Player.tileTargetX = mirroredCoords.X;
			Player.tileTargetY = mirroredCoords.Y;
			oldMirror = mirroredCoords;

			//Paints don't work if there's not enough paint
			//Tiles are being mirrored even though when stack is only 1
			
			//TODO: reducing stack not working -> Check if can invoke action and has enough stack
			// if (!shouldReduceStack || PlacementHelpers.CanReduceItemStack(itemType, shouldBeHeld: true))
			action?.Invoke(mirroredCoords);
		}
	}
	
    public static void LoadDetours() {
	    //TODO: ApplyItemTime will remove both items from stack but it's placed/mirrored before stack is checked

	    //Preventing infinite looping with oldMirror
	    Point16 oldMirror = default;
	    On.Terraria.Player.ApplyItemTime += (orig, player, item, multiplier, useItem) => {
		    orig.Invoke(player, item, multiplier, useItem);
		    //Only Tile Placements
		    if (item.createTile < TileID.Dirt && item.createWall < WallID.Stone) return;

		    if (oldMirror.X == Player.tileTargetX && oldMirror.Y == Player.tileTargetY) return;
			MirrorPlacementAction(mirroredCoords => {
				oldMirror = mirroredCoords;

				int itemTime = player.itemTime;
				player.itemTime = 0;
		
				player.direction *= -1;
				player.ItemCheck(player.whoAmI); //Would like to skip pre item check but oh well
				player.direction *= -1;

				player.itemTime = itemTime;
			}, default, item);
	    };

	    On.Terraria.WorldGen.PlaceWall += (orig, x, y, type, mute) => {
		    orig.Invoke(x, y, type, mute);

		    MirrorPlacementAction(mirroredCoords => {
			    orig.Invoke(mirroredCoords.X, mirroredCoords.Y, type, mute);
		    }, new Vector2(x, y));
	    };

	    On.Terraria.Player.PlaceThing_Walls_FillEmptySpace += (orig, player) => {
			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.IsMouseWithinSelection()) {
				//Messing with vanilla behaviour here for the sake of MirrorWand?
				
				//Do nothing
			}
			else orig.Invoke(player);
		};

	    //TODO: Replace tile/wall only removes 1 from stack
		On.Terraria.WorldGen.ReplaceTile += (orig, x, y, type, style) => {
			bool baseReturn = orig.Invoke(x, y, type, style);

			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(mirroredCoords.X, mirroredCoords.Y, type, style);
			}, new Vector2(x, y), shouldReduceStack: true);

			return baseReturn;
		};
		
		On.Terraria.WorldGen.ReplaceWall += (orig, x, y, type) => {
			bool baseReturn = orig.Invoke(x, y, type);

			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(mirroredCoords.X, mirroredCoords.Y, type);
			}, new Vector2(x, y), shouldReduceStack: true);

			return baseReturn;
		};

		// On.Terraria.WorldGen.KillTile += (orig, x, y, fail, only, item) => {
		// 	orig.Invoke(x, y, fail, only, item);
		// 	
		// 	MirrorPlacementAction(() => {
		// 		orig.Invoke(Player.tileTargetX, Player.tileTargetY, fail, only, item);
		// 	});
		// };

		//Works better than KillTile because of MultiTiles not breaking entirely on KillTile
		On.Terraria.Player.PickTile += (orig, player, x, y, power) => {
			orig.Invoke(player, x, y, power);
			
			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(player, mirroredCoords.X, mirroredCoords.Y, power);
			}, new Vector2(x, y));
		};

		On.Terraria.WorldGen.KillWall += (orig, x, y, fail) => {
			orig.Invoke(x, y, fail);

			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(mirroredCoords.X, mirroredCoords.Y, fail);
			}, new Vector2(x, y));
		};

		On.Terraria.Player.ItemCheck_UseMiningTools_TryPoundingTile += (
			On.Terraria.Player.orig_ItemCheck_UseMiningTools_TryPoundingTile orig, Player player, Item item, int id,
			ref bool wall, int x, int y) => {
			orig.Invoke(player, item, id, ref wall, x, y);
			
			Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
			if (wall) return;
			
			MirrorPlacementAction(mirroredCoords => {
				if (item.hammer > 0) {
					Tile mirrorTile = Framing.GetTileSafely(mirroredCoords.X, mirroredCoords.Y);
					int[] mirroredSlopes = new[] {0, 2, 1, 4, 3};
					AutoHammer.ChangeSlope((SlopeType) mirroredSlopes[(int) tile.Slope], tile.IsHalfBlock);
				}
			}, new Vector2(x, y));
		};

		On.Terraria.Player.ItemCheck_UseMiningTools_TryFindingWallToHammer += (
			On.Terraria.Player.orig_ItemCheck_UseMiningTools_TryFindingWallToHammer orig, out int x, out int y) => {
			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.IsMouseWithinSelection()) {
				//Messing with vanilla behaviour here for the sake of MirrorWand?
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
			}, new Vector2(x, y));
		};
		
		//TODO: Not running on vanilla paint tools because they just set tile.color()...
		On.Terraria.WorldGen.paintTile += (orig, x, y, color, sync) => {
			bool baseReturn = orig.Invoke(x, y, color, sync);

			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(mirroredCoords.X, mirroredCoords.Y, color, sync);
			}, new Vector2(x, y));
			
			return baseReturn;
		};
		
		On.Terraria.WorldGen.paintWall += (orig, x, y, color, sync) => {
			bool baseReturn = orig.Invoke(x, y, color, sync);

			MirrorPlacementAction(mirroredCoords => {
				orig.Invoke(mirroredCoords.X, mirroredCoords.Y, color, sync);
			}, new Vector2(x, y));
			
			return baseReturn;
		};
    }
}
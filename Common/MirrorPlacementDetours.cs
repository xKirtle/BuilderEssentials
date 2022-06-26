using System;
using System.Threading.Tasks;
using BuilderEssentials.Content.Items;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Common;

public static class MirrorPlacementDetours
{
	internal static void MirrorPlacementAction(Action action, bool shouldReduceStack = false, int itemType = 0) {
		if (WorldGen.gen) return;
		
		var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
		if (panel.IsVisible && panel.IsMouseWithinSelection()) {
			Vector2 mirroredCoords = panel.GetMirroredTileTargetCoordinate();
			Player.tileTargetX = (int) mirroredCoords.X;
			Player.tileTargetY = (int) mirroredCoords.Y;

			//Paints don't work if there's not enough paint
			//Tiles are being mirrored even though when stack is only 1
			
			//TODO: reducing stack not working -> Check if can invoke action and has enough stack
			// if (!shouldReduceStack || PlacementHelpers.CanReduceItemStack(itemType, shouldBeHeld: true))
			action?.Invoke();
		}
	}
	
    public static void LoadDetours() {
	    //TODO: ApplyItemTime will remove both items from stack but it's placed/mirrored before stack is checked
	    On.Terraria.Player.ApplyItemTime += (orig, player, item, multiplier, useItem) => {
		    orig.Invoke(player, item, multiplier, useItem);

		    //Only Tile Placements
		    if (item.createTile < TileID.Dirt && item.createWall < WallID.Stone) return;
		    
			MirrorPlacementAction(() => {
				int itemTime = player.itemTime;
				player.itemTime = 0;
		
				player.direction *= -1;
				player.ItemCheck(player.whoAmI); //Would like to skip pre item check but oh well
				player.direction *= -1;

				player.itemTime = itemTime;
			});
	    };

	    On.Terraria.WorldGen.PlaceWall += (orig, x, y, type, mute) => {
		    orig.Invoke(x, y, type, mute);
		    
		    MirrorPlacementAction(() => {
			    orig.Invoke(Player.tileTargetX, Player.tileTargetY, type, mute);
		    });
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

			MirrorPlacementAction(() => {
				orig.Invoke(Player.tileTargetX, Player.tileTargetY, type, style);
			}, true, type);

			return baseReturn;
		};
		
		On.Terraria.WorldGen.ReplaceWall += (orig, x, y, type) => {
			bool baseReturn = orig.Invoke(x, y, type);

			MirrorPlacementAction(() => {
				orig.Invoke(Player.tileTargetX, Player.tileTargetY, type);
			}, true, type);

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
			
			MirrorPlacementAction(() => {
				orig.Invoke(player, Player.tileTargetX, Player.tileTargetY, power);
			});
		};

		On.Terraria.WorldGen.KillWall += (orig, x, y, fail) => {
			orig.Invoke(x, y, fail);

			MirrorPlacementAction(() => {
				orig.Invoke(Player.tileTargetX, Player.tileTargetY, fail);
			});
		};

		On.Terraria.Player.ItemCheck_UseMiningTools_TryPoundingTile += (
			On.Terraria.Player.orig_ItemCheck_UseMiningTools_TryPoundingTile orig, Player player, Item item, int id,
			ref bool wall, int x, int y) => {
			orig.Invoke(player, item, id, ref wall, x, y);
			
			Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
			if (wall) return;
			
			MirrorPlacementAction(() => {
				if (item.hammer > 0) {
					Tile mirrorTile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
					int[] mirroredSlopes = new[] {0, 2, 1, 4, 3};
					AutoHammer.ChangeSlope((SlopeType) mirroredSlopes[(int) tile.Slope], tile.IsHalfBlock);
				}
			});
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
			
			MirrorPlacementAction(() => {
				player.controlUseItem = true;
				player.releaseUseItem = false;
		
				orig.Invoke(player, item, Player.tileTargetX, Player.tileTargetY);
			});
		};
		
		//TODO: Not running on vanilla paint tools because they just set tile.color()...
		On.Terraria.WorldGen.paintTile += (orig, x, y, color, sync) => {
			bool baseReturn = orig.Invoke(x, y, color, sync);

			MirrorPlacementAction(() => {
				orig.Invoke(Player.tileTargetX, Player.tileTargetY, color, sync);
			});
			
			return baseReturn;
		};
		
		On.Terraria.WorldGen.paintWall += (orig, x, y, color, sync) => {
			bool baseReturn = orig.Invoke(x, y, color, sync);

			MirrorPlacementAction(() => {
				orig.Invoke(Player.tileTargetX, Player.tileTargetY, color, sync);
			});
			
			return baseReturn;
		};
    }
}
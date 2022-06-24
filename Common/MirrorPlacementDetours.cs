using BuilderEssentials.Content.Items;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Common;

public static class MirrorPlacementDetours
{
    public static void LoadDetours() {
	    //Kirtle: Detour WorldGen.PlaceTile/Wall and WorldGen.RemoveTile/Wall,
	    //see if coords are inside selection and mirror them by invoking orig again??

		// On.Terraria.Player.ItemCheck_UseMiningTools_ActuallyUseMiningTool += (
		// 	On.Terraria.Player.orig_ItemCheck_UseMiningTools_ActuallyUseMiningTool orig, 
		// 	Terraria.Player player, Item item, out bool walls, int x, int y) => {
		// 	orig.Invoke(player, item, out walls, x, y);
		//
		// 	var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
		// 	if (!panel.IsVisible) return;
		// 		
		// 	Vector2 mirroredCords = panel.GetMirroredTileTargetCoordinate();
		//
		// 	if (item.hammer > 0) {
		// 		Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
		// 		// if (!tile.HasTile) return; -> !walls
		// 		
		// 		Player.tileTargetX = (int) mirroredCords.X;
		// 		Player.tileTargetY = (int) mirroredCords.Y;
		// 		
		// 		if (!walls) {
		//
		// 			int[] mirroredSlopes = new[] {0, 2, 1, 4, 3};
		// 			Tile mirrorTile = Framing.GetTileSafely(mirroredCords);
		// 			AutoHammer.ChangeSlope((SlopeType) mirroredSlopes[(int) tile.Slope], tile.IsHalfBlock);
		// 		}
		// 	}
		// 	else if (item.pick > 0) {
		// 		Player.tileTargetX = (int) mirroredCords.X;
		// 		Player.tileTargetY = (int) mirroredCords.Y;
		// 		
		// 		orig.Invoke(player, item, out walls, Player.tileTargetX, Player.tileTargetY);
		// 	}
		// };
		// 	
		// On.Terraria.Player.ItemCheck_UseMiningTools_TryFindingWallToHammer += (
		// 	On.Terraria.Player.orig_ItemCheck_UseMiningTools_TryFindingWallToHammer orig, out int x, out int y) => {
		// 	var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
		// 	if (panel.IsVisible && panel.IsMouseWithinSelection()) {
		// 		//Messing with vanilla behaviour here for the sake of MirrorWand?
		// 		x = Player.tileTargetX;
		// 		y = Player.tileTargetY;
		// 	}
		// 	else orig.Invoke(out x, out y);
		// };
		//
		// On.Terraria.Player.ItemCheck_UseMiningTools_TryHittingWall += (orig, player, item, x, y) => {
		// 	orig.Invoke(player, item, x, y);
		// 	
		// 	var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
		// 	if (!panel.IsVisible) return;
		// 	
		// 	Vector2 mirroredCords = panel.GetMirroredTileTargetCoordinate();
		// 	Tile tile = Framing.GetTileSafely(x, y);
		// 	Tile mirroredTile = Framing.GetTileSafely(mirroredCords);
		// 	
		// 	Player.tileTargetX = (int) mirroredCords.X;
		// 	Player.tileTargetY = (int) mirroredCords.Y;
		// 	player.controlUseItem = true;
		// 	player.releaseUseItem = false;
		//
		// 	orig.Invoke(player, item, Player.tileTargetX, Player.tileTargetY);
		// };
		//
		// int oldItemUse = 0;
		// //TODO: Something is causing materials to not be consumed in this detour
		// On.Terraria.Player.ApplyItemTime += (orig, player, item, multiplier, useItem) => {
		// 	oldItemUse = player.ItemUsesThisAnimation;
		// 	orig.Invoke(player, item, multiplier, useItem);
		// 	if (oldItemUse != player.ItemUsesThisAnimation) {
		// 		var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
		// 		if (!panel.IsVisible) return;
		// 		
		// 		Vector2 mirroredCords = panel.GetMirroredTileTargetCoordinate();
		// 		
		// 		//Tile Placements
		// 		if (item.createTile >= TileID.Dirt || item.createWall >= WallID.Stone) {
		// 			Player.tileTargetX = (int) mirroredCords.X;
		// 			Player.tileTargetY = (int) mirroredCords.Y;
		//
		// 			//If set to 0, makes it so ItemCheck_StartActualUse is called again
		// 			player.itemAnimation = item.useAnimation;
		// 			player.itemAnimationMax = item.useAnimation;
		// 			player.itemTime = 0;
		//
		// 			Main.LocalPlayer.direction *= -1;
		// 			player.ItemCheck(player.whoAmI); //Would like to skip pre item check but oh well
		// 			Main.LocalPlayer.direction *= -1;
		// 		}
		// 	}
		// };
		//
		// On.Terraria.Player.PlaceThing_Walls_FillEmptySpace += (orig, player) => {
		// 	var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
		// 	if (panel.IsVisible && panel.IsMouseWithinSelection()) {
		// 		//Messing with vanilla behaviour here for the sake of MirrorWand?
		// 		
		// 		//Do nothing
		// 	}
		// 	else orig.Invoke(player);
		// };
		
		//TODO: Check if can reduce stack once more in Placement methods before placing

		On.Terraria.WorldGen.PlaceTile += (orig, x, y, type, mute, forced, plr, style) => {
			bool baseReturn = orig.Invoke(x, y, type, mute, forced, plr, style);
			
			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.IsMouseWithinSelection()) {
				Vector2 mirroredCoords = panel.GetMirroredTileTargetCoordinate();

				Player.tileTargetX = (int) mirroredCoords.X;
				Player.tileTargetY = (int) mirroredCoords.Y;
				orig.Invoke(Player.tileTargetX, Player.tileTargetY, type, mute, forced, plr, style);
			}

			return baseReturn;
		};

		On.Terraria.WorldGen.PlaceWall += (orig, x, y, type, mute) => {
			orig.Invoke(x, y, type, mute);

			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.IsMouseWithinSelection()) {
				Vector2 mirroredCoords = panel.GetMirroredTileTargetCoordinate();
				
				Player.tileTargetX = (int) mirroredCoords.X;
				Player.tileTargetY = (int) mirroredCoords.Y;
				
				orig.Invoke(Player.tileTargetX, Player.tileTargetY, type, mute);
			}
		};
		
		On.Terraria.Player.PlaceThing_Walls_FillEmptySpace += (orig, player) => {
			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.IsMouseWithinSelection()) {
				//Messing with vanilla behaviour here for the sake of MirrorWand?
				
				//Do nothing
			}
			else orig.Invoke(player);
		};

		On.Terraria.WorldGen.KillTile += (orig, x, y, fail, only, item) => {
			orig.Invoke(x, y, fail, only, item);

			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.IsMouseWithinSelection()) {
				Vector2 mirroredCoords = panel.GetMirroredTileTargetCoordinate();

				Player.tileTargetX = (int) mirroredCoords.X;
				Player.tileTargetY = (int) mirroredCoords.Y;
				orig.Invoke(Player.tileTargetX, Player.tileTargetY, fail, only, item);
			}
		};
		
		On.Terraria.WorldGen.KillWall += (orig, x, y, fail) => {
			orig.Invoke(x, y, fail);

			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.IsMouseWithinSelection()) {
				Vector2 mirroredCoords = panel.GetMirroredTileTargetCoordinate();

				Player.tileTargetX = (int) mirroredCoords.X;
				Player.tileTargetY = (int) mirroredCoords.Y;
				orig.Invoke(Player.tileTargetX, Player.tileTargetY, fail);
			}
		};
		
		//TODO: Not working
		On.Terraria.WorldGen.SlopeTile += (orig, x, y, slope, effects) => {
			bool baseReturn = orig.Invoke(x, y, slope, effects);
			
			var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
			if (panel.IsVisible && panel.IsMouseWithinSelection()) {
				Vector2 mirroredCoords = panel.GetMirroredTileTargetCoordinate();
				Tile tile = Framing.GetTileSafely(x, y);
				int[] mirroredSlopes = new[] {0, 2, 1, 4, 3};
				
				Player.tileTargetX = (int) mirroredCoords.X;
				Player.tileTargetY = (int) mirroredCoords.Y;

				orig.Invoke(Player.tileTargetX, Player.tileTargetY, mirroredSlopes[(int)tile.Slope], effects);
			}
			
			return baseReturn;
		};
    }
}
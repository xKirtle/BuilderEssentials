using BuilderEssentials.Content.Items;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Common;

public static class MirrorPlacementDetours
{
    public static void LoadDetours() {
        On.Terraria.Player.ItemCheck_UseMiningTools_ActuallyUseMiningTool += (
				On.Terraria.Player.orig_ItemCheck_UseMiningTools_ActuallyUseMiningTool orig, 
				Terraria.Player player, Item item, out bool walls, int x, int y) => {
				orig.Invoke(player, item, out walls, x, y);

				var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
				if (!panel.IsVisible) return;
					
				Vector2 mirroredCords = panel.GetMirroredTileTargetCoordinate();

				if (item.hammer > 0) {
					Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
					// if (!tile.HasTile) return; -> !walls
					
					Player.tileTargetX = (int) mirroredCords.X;
					Player.tileTargetY = (int) mirroredCords.Y;
					
					if (!walls) {

						int[] mirroredSlopes = new[] {0, 2, 1, 4, 3};
						Tile mirrorTile = Framing.GetTileSafely(mirroredCords);
						AutoHammer.ChangeSlope((SlopeType) mirroredSlopes[(int) tile.Slope], tile.IsHalfBlock);
					}
				}
				else if (item.pick > 0) {
					Player.tileTargetX = (int) mirroredCords.X;
					Player.tileTargetY = (int) mirroredCords.Y;
					
					orig.Invoke(player, item, out walls, Player.tileTargetX, Player.tileTargetY);
				}
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
				
				var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
				if (!panel.IsVisible) return;
				
				Vector2 mirroredCords = panel.GetMirroredTileTargetCoordinate();
				Tile tile = Framing.GetTileSafely(x, y);
				Tile mirroredTile = Framing.GetTileSafely(mirroredCords);
				
				Player.tileTargetX = (int) mirroredCords.X;
				Player.tileTargetY = (int) mirroredCords.Y;
				player.controlUseItem = true;
				player.releaseUseItem = false;

				orig.Invoke(player, item, Player.tileTargetX, Player.tileTargetY);
			};
			
			int oldItemUse = 0;
			On.Terraria.Player.ApplyItemTime += (orig, player, item, multiplier, useItem) => {
				oldItemUse = player.ItemUsesThisAnimation;
				orig.Invoke(player, item, multiplier, useItem);
				if (oldItemUse != player.ItemUsesThisAnimation) {
					var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
					if (!panel.IsVisible) return;
					
					Vector2 mirroredCords = panel.GetMirroredTileTargetCoordinate();
					
					//Tile Placements
					if (item.createTile >= TileID.Dirt || item.createWall >= WallID.Stone) {
						Player.tileTargetX = (int) mirroredCords.X;
						Player.tileTargetY = (int) mirroredCords.Y;

						//If set to 0, makes it so ItemCheck_StartActualUse is called again
						player.itemAnimation = item.useAnimation;
						player.itemAnimationMax = item.useAnimation;
						player.itemTime = 0;

						Main.LocalPlayer.direction *= -1;
						player.ItemCheck(player.whoAmI); //Would like to skip pre item check but oh well
						Main.LocalPlayer.direction *= -1;
					}
				}
			};
    }
}
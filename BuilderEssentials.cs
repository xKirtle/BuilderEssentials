using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BuilderEssentials.Assets;
using BuilderEssentials.Common;
using BuilderEssentials.Content.Items;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using Newtonsoft.Json;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials
{
	public class BuilderEssentials : Mod
	{
		public static Dictionary<int, List<int>> TileToItems;
		public static Dictionary<int, List<int>> WallToItems;
		
		public override void PostSetupContent() {
			string tiles = Encoding.UTF8.GetString(GetFileBytes("CachedTiles.json"));
			TileToItems = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(tiles);
			
			string walls = Encoding.UTF8.GetString(GetFileBytes("CachedWalls.json"));
			WallToItems = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(walls);
			
			CacheModTiles();
		}
		
		private void CacheModTiles() {
			Item item = new();
			for (int i = TileToItems.Count; i < TileLoader.TileCount; i++) {
				List<int> tileItems = new();
				for (int j = 0; j < ItemLoader.ItemCount; j++) {
					item.SetDefaults(j);
					if (item.createTile == i)
						tileItems.Add(j);
				}
			
				TileToItems.Add(i, tileItems);
			}
			
			for (int i = WallToItems.Count; i < WallLoader.WallCount; i++) {
				List<int> wallItems = new();
				for (int j = 0; j < ItemLoader.ItemCount; j++) {
					item.SetDefaults(j);
					if (item.createWall == i)
						wallItems.Add(j);
				}
			
				WallToItems.Add(i, wallItems);
			}
		}

		public override void Load() {
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

			On.Terraria.Player.ItemCheck_UseMiningTools_TryHittingWall += (orig, player, item, x, y) => {
				orig.Invoke(player, item, x, y);
				
				var panel = ShapesUIState.GetUIPanel<MirrorWandPanel>();
				if (!panel.IsVisible) return;
				
				Vector2 mirroredCords = panel.GetMirroredTileTargetCoordinate();
				
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

					// if (item.hammer > 0) {
					// 	MethodInfo UseMiningTools = Code.GetType().GetMethod("ItemCheck_UseMiningTools",
					// 		BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
					//
					// 	Player.tileTargetX = (int) mirroredCords.X;
					// 	Player.tileTargetY = (int) mirroredCords.Y;
					// 	player.controlUseItem = true;
					// 	player.releaseUseItem = false;
					// 	
					// 	UseMiningTools.Invoke(player, new[] {item});
					// }
				}
			};
		}

		public override void Unload() {
			AssetsLoader.UnloadTextures();
		}
	}
}
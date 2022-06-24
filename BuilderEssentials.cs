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

				Console.WriteLine("mine");
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

						//Not needed?
						// self.controlUseItem = true;
						// self.releaseUseItem = true;

						player.ItemCheck(player.whoAmI); //Would like to skip pre item check but oh well
					}
					
					if (item.hammer > 0) {
						Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);

						Player.tileTargetX = (int) mirroredCords.X;
						Player.tileTargetY = (int) mirroredCords.Y;
						
						int[] mirroredSlopes = new[] {0, 2, 1, 4, 3};
						int mirroredHammerIndex = mirroredSlopes[((int)tile.Slope + 1) % 5];
						SlopeType newSlope = (SlopeType) mirroredHammerIndex;
						bool isHalfBlock = !tile.IsHalfBlock && mirroredHammerIndex == 2;
						AutoHammer.ChangeSlope(!isHalfBlock ? newSlope : tile.Slope, isHalfBlock);
					}
					
					if (item.axe > 0) {
						Console.WriteLine("Axe");
						
						//Should this work on trees?
					}
				}
			};
		}

		public override void Unload() {
			AssetsLoader.UnloadTextures();
		}
	}
}
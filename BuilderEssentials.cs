using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BuilderEssentials.Assets;
using BuilderEssentials.Common;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using Newtonsoft.Json;
using Terraria;
using Terraria.DataStructures;
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
			//Runs whenever mining capable items are used
			On.Terraria.Player.ItemCheck_UseMiningTools_ActuallyUseMiningTool += (
				On.Terraria.Player.orig_ItemCheck_UseMiningTools_ActuallyUseMiningTool orig, 
				Terraria.Player self, Item item, out bool walls, int i, int i1) =>
			{
				orig.Invoke(self, item, out walls, i, i1);
			};

			//Runs every item.useTime or useAnimation (idk)
			On.Terraria.Player.ItemCheck_StartActualUse += (orig, self, item) =>
			{
				orig.Invoke(self, item);
			};

			//Runs on successful placements
			On.Terraria.Player.ApplyItemTime += (orig, self, item, multiplier, useItem) =>
			{
				orig.Invoke(self, item, multiplier, useItem);
				if (self.ItemUsesThisAnimation == 1) {

					Vector2 mirroredCords = MirrorPlacement.GetMirroredTileTargetCoordinate();
					Player.tileTargetX = (int) mirroredCords.X;
					Player.tileTargetY = (int) mirroredCords.Y;
					
					//If set to 0, makes it so ItemCheck_StartActualUse is called again
					self.itemAnimation = item.useAnimation;
					self.itemAnimationMax = item.useAnimation;
					self.itemTime = 0;
					//Not needed?
					// self.controlUseItem = true;
					// self.releaseUseItem = true;
					
					self.ItemCheck(self.whoAmI); //Would like to skip pre item check but oh well
				}
			};
		}

		public override void Unload() {
			AssetsLoader.UnloadTextures();
		}
	}
}
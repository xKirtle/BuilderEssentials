using System;
using System.Collections.Generic;
using System.Text;
using BuilderEssentials.Assets;
using BuilderEssentials.Common;
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
			// On.Terraria.Player.PlaceThing += (orig, self) =>
			// {
			// 	// Console.WriteLine("Place Thing");
			// 	orig.Invoke(self);
			// 	// Console.WriteLine("After Placing");
			// 	// MirrorPlacement.PlaceTile();
			// 	
			// 	// TileObject.objectPreview.CopyFrom(new TileObjectPreviewData());
			// };
		}

		public override void Unload() {
			AssetsLoader.UnloadTextures();
		}
	}
}
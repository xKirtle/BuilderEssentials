using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials
{
	public class BuilderEssentials : Mod
	{
		public static Dictionary<int, List<int>> TileToItems;
		public static Dictionary<int, List<int>> WallToItems;
		public static bool Initialized;

		//TODO: Optimize this whenever possible
		public override void PostSetupContent() {
			// AsyncCacheTiles();
		}
		
		private async void AsyncCacheTiles() {
			await new Task((() =>
			{
				TileToItems = new(TileLoader.TileCount);
				for (int i = 0; i < TileLoader.TileCount; i++) {
					List<int> tileItems = new();
					Item item = new();
				
					for (int j = 0; j < ItemLoader.ItemCount; j++) {
						item.SetDefaults(j);
						if (item.createTile == i)
							tileItems.Add(j);
					}
				
					TileToItems.Add(i, tileItems);
					// if (tileItems.Count > 1)
					// 	Console.WriteLine($"TileType {i} has {tileItems.Count}");
				}

				WallToItems = new(WallLoader.WallCount);

				for (int i = 0; i < WallLoader.WallCount; i++) {
					List<int> wallItems = new();
					Item item = new();

					for (int j = 0; j < ItemLoader.ItemCount; j++) {
						item.SetDefaults(j);
						if (item.createWall == i)
							wallItems.Add(j);
					}
				
					WallToItems.Add(i, wallItems);
					// if (wallItems.Count > 1)
					// 	Console.WriteLine($"WallType {i} has {wallItems.Count}");
				}
			}));
			
			Initialized = true;
		}
	}
}
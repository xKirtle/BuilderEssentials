using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

namespace BuilderEssentials;

public class BuilderEssentials : Mod
{
    internal ModKeybind FWIncrease;
    internal ModKeybind FWDecrease;
    internal ModKeybind UndoPlacement;
    internal Dictionary<int, List<int>> TileToItems;
    internal Dictionary<int, List<int>> WallToItems;

    public static BuilderEssentials GetInstance() => ModContent.GetInstance<BuilderEssentials>();
    
    public override void PostSetupContent() {
        ReadAndCacheLocally();

        FWIncrease = KeybindLoader.RegisterKeybind(this, "Increase Fill Wand selection size", "I");
        FWDecrease = KeybindLoader.RegisterKeybind(this, "Decrease Fill Wand selection size", "O");
        UndoPlacement = KeybindLoader.RegisterKeybind(this, "Undo last placement with a tool that supports it", "P");
    }

    public void ReadAndCacheLocally() {
        string tiles = Encoding.UTF8.GetString(GetFileBytes("CachedTiles.json"));
        TileToItems = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(tiles);

        string walls = Encoding.UTF8.GetString(GetFileBytes("CachedWalls.json"));
        WallToItems = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(walls);
        
        // Cache things async
        Task.Factory.StartNew(() => CacheModTiles(), TaskCreationOptions.LongRunning);
    }
    
    private async Task CacheModTiles() {
        Parallel.For(TileToItems.Count, TileLoader.TileCount, i => {
            // In case it was requested and cached before this step got to it
            if (TileToItems.ContainsKey(i))
                return; // same as continue in a parallel for loop
            
            Item item = new();
            List<int> tileItems = new();
            for (int j = 0; j < ItemLoader.ItemCount; j++) {
                item.SetDefaults(j);
                if (item.createTile == i)
                    tileItems.Add(j);
            }

            TileToItems.Add(i, tileItems);
        });
        
        Parallel.For(WallToItems.Count, WallLoader.WallCount, i => {
            // In case it was requested and cached before this step got to it
            if (TileToItems.ContainsKey(i))
                return; // same as continue in a parallel for loop
            
            Item item = new();
            List<int> wallItems = new();
            for (int j = 0; j < ItemLoader.ItemCount; j++) {
                item.SetDefaults(j);
                if (item.createWall == i)
                    wallItems.Add(j);
            }

            WallToItems.Add(i, wallItems);
        });
    }

    public override void Load() {
        MirrorPlacement.LoadDetours();
        AssetsLoader.LoadTextures();
    }

    public override void Unload() => AssetsLoader.UnloadTextures();
}
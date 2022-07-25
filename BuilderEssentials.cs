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

namespace BuilderEssentials;

public class BuilderEssentials : Mod
{
    internal static ModKeybind FWIncrease;
    internal static ModKeybind FWDecrease;
    internal static ModKeybind UndoPlacement;
    public static Dictionary<int, List<int>> TileToItems;
    public static Dictionary<int, List<int>> WallToItems;

#if TML_2022_06
    public override void AddRecipes() => ReadAndCacheLocally();
#endif

    public override void PostSetupContent() {
#if !TML_2022_06
			ReadAndCacheLocally();
#endif

        FWIncrease = KeybindLoader.RegisterKeybind(this, "Increase Fill Wand selection size", "I");
        FWDecrease = KeybindLoader.RegisterKeybind(this, "Decrease Fill Wand selection size", "O");
        UndoPlacement = KeybindLoader.RegisterKeybind(this, "Undo last placement with a tool that supports it", "P");
    }

    public void ReadAndCacheLocally() {
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
        MirrorPlacement.LoadDetours();
        AssetsLoader.LoadTextures();
    }

    public override void Unload() => AssetsLoader.UnloadTextures();
}
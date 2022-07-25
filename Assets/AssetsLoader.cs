using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuilderEssentials.Common;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace BuilderEssentials.Assets;

public static class AssetsLoader
{
    private const string AssetsPath = "BuilderEssentials/Assets/";
    private static Dictionary<string, Asset<Texture2D>> texturesDictionary = new Dictionary<string, Asset<Texture2D>>();
    internal static void LoadTextures() {
        List<string> paths = ModContent.GetInstance<BuilderEssentials>().GetFileNames()
            .FindAll(x => x.Contains("Assets/UI/"));
        paths.Sort(new NaturalComparer());

        paths.ForEach(path => {
            if (path.Contains(".ase")) return;

            path = path.Replace(".rawimg", "");
            string key = path.Replace("Assets/", "");
            path = path.Replace("Assets/", AssetsPath);
            texturesDictionary.Add(key, ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad));
        });
    }

    public static Asset<Texture2D> GetAssets(string key) => texturesDictionary[key];

    internal static void UnloadTextures() => texturesDictionary = null;
}
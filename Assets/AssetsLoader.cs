using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace BuilderEssentials.Assets;

public static class AssetsLoader
{
    private static bool isInitialized;
    private const string AssetsPath = "BuilderEssentials/Assets/";
    private static Dictionary<string, Asset<Texture2D>[]> texturesDictionary = new() {
        {AssetsID.AutoHammer, new Asset<Texture2D>[6]},
        {AssetsID.MultiWand, new Asset<Texture2D>[6]},
        {AssetsID.PaintBrushColors, new Asset<Texture2D>[33]},
        {AssetsID.PaintBrushTools, new Asset<Texture2D>[7]}
    };

    public static Asset<Texture2D>[] GetAssets(string key) => texturesDictionary[key];

    internal static void LoadTextures() {
        if (isInitialized) return;
        foreach (string key in texturesDictionary.Keys) {
            for (int i = 0; i < texturesDictionary[key].Length; i++) {
                var asset = ModContent.Request<Texture2D>(AssetsPath + key + i, AssetRequestMode.ImmediateLoad);
                texturesDictionary[key][i] = asset;
            }
        }

        isInitialized = true;
    }

    internal static void UnloadTextures() {
        //No need to null each array in the dictionary individually since nothing is referencing them?
        foreach (string key in texturesDictionary.Keys)
            texturesDictionary[key] = null;

        texturesDictionary = null;
        isInitialized = false;
    }
}
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace BuilderEssentials.Assets;

public static class AssetsLoader
{
    private const string AssetsPath = "BuilderEssentials/Assets/";
    private static Dictionary<string, Asset<Texture2D>[]> texturesDictionary = new() {
        {AssetsID.AutoHammer, new Asset<Texture2D>[6]},
        
    };

    public static Asset<Texture2D>[] GetAssets(string key) => texturesDictionary[key];

    internal static void AsyncLoadTextures() {
        foreach (string key in texturesDictionary.Keys) {
            for (int i = 0; i < texturesDictionary[key].Length; i++)
                texturesDictionary[key][i] = ModContent.Request<Texture2D>(AssetsPath + key + i);
        }
    }

    internal static void UnloadTextures() {
        //No need to null each array in the dictionary individually since nothing is referencing them?
        foreach (string key in texturesDictionary.Keys)
            texturesDictionary[key] = null;

        texturesDictionary = null;
    }
}
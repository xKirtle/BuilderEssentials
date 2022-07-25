using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BuilderEssentials.Common.Systems;

public class RecipeSystem : ModSystem
{
    internal static void CreateRecipeGroup(int[] items, string text) {
        RecipeGroup recipeGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + text, items);
        RecipeGroup.RegisterGroup("BuilderEssentials:" + text, recipeGroup);
    }

    public override void AddRecipeGroups() {
        //TileType indexing, returns list of items that place tiles implementing tiletype adjacency
        List<List<int>> tilesList = new List<int>[ItemLoader.ItemCount].Select(list => new List<int>()).ToList();

        foreach ((int itemType, Item item) in ContentSamples.ItemsByType) {
            //Vanilla -> cache these?
            if (item.createTile >= TileID.Dirt && item.createTile < TileID.Count)
                tilesList[item.createTile].Add(itemType);

            //Modded
            if (item.createTile >= TileID.Count) {
                foreach (int adjTileType in TileLoader.GetTile(item.createTile).AdjTiles)
                    tilesList[adjTileType].Add(itemType);
            }
        }

        List<int> woods = new List<int> {
            TileID.WoodBlock, TileID.RichMahogany, TileID.Ebonwood, TileID.Shadewood, TileID.Pearlwood,
            TileID.BorealWood, TileID.PalmWood, TileID.DynastyWood, TileID.SpookyWood
        };

        List<int> woodItems = new List<int>();
        woods.ForEach(woodType => woodItems.AddRange(tilesList[woodType]));
        CreateRecipeGroup(woodItems.ToArray(), "Wood");
        CreateRecipeGroup(tilesList[TileID.WorkBenches].ToArray(), "Workbench");
        CreateRecipeGroup(tilesList[TileID.Chairs].ToArray(), "Chair");
        CreateRecipeGroup(tilesList[TileID.Tables].Concat(tilesList[TileID.Tables2]).ToArray(), "Table");
        CreateRecipeGroup(tilesList[TileID.Sinks].ToArray(), "Sink");
        CreateRecipeGroup(tilesList[TileID.Furnaces].Concat(tilesList[TileID.Hellforge]).ToArray(), "Pre Hardmode Furnace/Forge");
        CreateRecipeGroup(tilesList[TileID.Anvils].ToArray(), "Pre Hardmode Anvil");
        CreateRecipeGroup(tilesList[TileID.MythrilAnvil].ToArray(), "Hardmode Anvil");
        CreateRecipeGroup(tilesList[TileID.CookingPots].ToArray(), "Cooking Pot");
        CreateRecipeGroup(tilesList[TileID.AdamantiteForge].ToArray(), "Hardmode Forge");
        CreateRecipeGroup(tilesList[TileID.Bookcases].ToArray(), "Bookcase");
        CreateRecipeGroup(tilesList[TileID.Campfire].ToArray(), "Campfire");
        CreateRecipeGroup(new int[] { ItemID.MagicMirror, ItemID.IceMirror }, "Magic Mirror");

        //Freeing resources
        tilesList = null;
    }
}
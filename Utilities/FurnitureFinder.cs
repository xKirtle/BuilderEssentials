using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Utilities
{
    public static partial class Tools
    {
        public static int FindFurniture(Tile tile, ref Item item)
        {
            int tilePlaceStyle = TileObjectData.GetTileStyle(tile);
            int originalItemType = item.type;
            for (int i = 0; i < ItemLoader.ItemCount; i++)
            {
                item.SetDefaults(i);
                if (item.createTile == tile.type && item.placeStyle == tilePlaceStyle)
                    return i;
            }

            //if it reaches here, didn't find any matches
            item.SetDefaults(originalItemType);
            return -1;
        }
    }
}
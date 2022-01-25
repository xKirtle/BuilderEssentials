using System;
using BuilderEssentials.Items;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace BuilderEssentials
{
    public class BEPlayer : ModPlayer
    {
        public bool ValidCursorPos => HelperMethods.ValidTileCoordinates(Player.tileTargetX, Player.tileTargetY);
        public Vector2 PointedCoord => Main.MouseWorld;
        public Vector2 PointedTileCoord => new Vector2(Player.tileTargetX, Player.tileTargetY);
        public Tile PointedTile => Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
        public bool InfinitePlayerRange { get; set; }
        public bool FastPlacement { get; set; }
        public bool InfinitePlacement { get; set; } //TODO: Needs to be implemented into vanilla stuff
        public bool PlacementAnywhere { get; set; } //TODO: Needs to be implemented into vanilla stuff
        public bool InfinitePickupRange { get; set; }

        public bool improvedRulerEquipped;
        public bool infinitePaintBucketEquipped;
        public bool buildingWrenchEquipped;
        public bool replaceTiles;
        
        public override void ResetEffects()
        {
            InfinitePlayerRange = FastPlacement = InfinitePlacement = 
            PlacementAnywhere = InfinitePickupRange = improvedRulerEquipped = 
            infinitePaintBucketEquipped = buildingWrenchEquipped = false;
        }

        public override void PostUpdateEquips()
        {
            if (InfinitePlayerRange)
            {
                int reach = Main.screenWidth / 16 / 2 + 5;
                HelperMethods.AddToPlayerRange(reach, reach, reach);
            }

            if (FastPlacement)
            {
                Player.wallSpeed += 10;
                Player.tileSpeed += 10;
            }

            if (InfinitePickupRange)
            {
                Player.defaultItemGrabRange = 1000000;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BuilderEssentials.IncreaseFillToolSize.JustPressed && FillWand.fillSelectionSize < 6)
                ++FillWand.fillSelectionSize;

            if (BuilderEssentials.DecreaseFillToolSize.JustPressed && FillWand.fillSelectionSize > 1)
                --FillWand.fillSelectionSize;

            if (BuilderEssentials.ReplaceTilesFillWand.JustPressed)
                replaceTiles = !replaceTiles;
        }
    }
}
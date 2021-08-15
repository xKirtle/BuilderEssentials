using System;
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
        public bool InfinitePlacement { get; set; }
        public bool PlacementAnywhere { get; set; }
        public bool InfinitePickupRange { get; set; }

        public bool improvedRulerEquipped;
        public bool infinitePaintBucketEquipped;
        
        public override void ResetEffects()
        {
            InfinitePlayerRange = FastPlacement = InfinitePlacement = 
                PlacementAnywhere = InfinitePickupRange = 
                    improvedRulerEquipped = infinitePaintBucketEquipped = false;
        }
        
        public override void PostUpdateEquips()
        {
            //TODO: Check tile/block Range values
            Player.tileRangeX = InfinitePlayerRange ? Main.screenWidth / 16 / 2 + 5 : 5;
            Player.tileRangeY = InfinitePlayerRange ? Main.screenHeight / 16 / 2 + 4 : 4;
            Player.blockRange = InfinitePlayerRange ? Main.screenWidth / 16 / 2 + 5 : 0;
            Player.wallSpeed = FastPlacement ? Player.wallSpeed + 10 : 1;
            Player.tileSpeed = FastPlacement ? Player.tileSpeed + 50 : 1;
            Player.defaultItemGrabRange = //I have no idea how much it should be so that should be enough??
                InfinitePickupRange ? (int)Math.Pow(Math.Max(Main.maxTilesX, Main.maxTilesY), 2) : 42; 
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //Update triggers
        }
    }
}
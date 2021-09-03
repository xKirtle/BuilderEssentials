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
            //TODO: Can't use a ternary here, it'll not allow other items to increase player range willingly
            
            // HelperMethods.SetPlayerRange(5, 4, 0);
            // if (InfinitePlayerRange)
            //     HelperMethods.SetPlayerRange(Main.screenWidth / 16 / 2 + 5, 
            //         Main.screenHeight / 16 / 2 + 4, Main.screenWidth / 16 / 2 + 5);
            // Player.wallSpeed = FastPlacement ? Player.wallSpeed + 10 : 1;
            // Player.tileSpeed = FastPlacement ? Player.tileSpeed + 50 : 1;
            // Player.defaultItemGrabRange = //I have no idea how much it should be so that should be enough??
            //     InfinitePickupRange ? (int) Math.Pow(Math.Max(Main.maxTilesX, Main.maxTilesY), 2) : 42;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BuilderEssentials.IncreaseFillToolSize.JustPressed && FillWand.fillSelectionSize < 6)
                ++FillWand.fillSelectionSize;

            if (BuilderEssentials.DecreaseFillToolSize.JustPressed && FillWand.fillSelectionSize > 1)
                --FillWand.fillSelectionSize;
        }
    }
}
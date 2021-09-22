using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BuilderEssentials.Items;
using BuilderEssentials.Utilities;

namespace BuilderEssentials
{
    public class BEPlayer : ModPlayer
    {
        public bool ValidCursorPos => HelperMethods.ValidTileCoordinates(Player.tileTargetX, Player.tileTargetY);
        public Vector2 PointedCoord => Main.MouseWorld;
        public Vector2 PointedTileCoord => new Vector2(Player.tileTargetX, Player.tileTargetY);
        public Tile PointedTile => Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
        public bool InfinitePlacementRange { get; set; }
        public bool InfinitePlayerRange { get; set; }
        public bool FastPlacement { get; set; }
        public bool InfinitePlacement { get; set; }
        public bool PlacementAnywhere { get; set; }
        public bool InfinitePickupRange { get; set; }

        public bool improvedRulerEquipped;
        public bool infinitePaintBucketEquipped;
        
        public override void ResetEffects()
        {
            InfinitePlacementRange = InfinitePlayerRange = FastPlacement
                = InfinitePlacement = PlacementAnywhere = InfinitePickupRange 
                    = improvedRulerEquipped = infinitePaintBucketEquipped = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BuilderEssentials.IncreaseFillToolSize.JustPressed && FillWand.fillSelectionSize < 6)
                ++FillWand.fillSelectionSize;

            if (BuilderEssentials.DecreaseFillToolSize.JustPressed && FillWand.fillSelectionSize > 1)
                --FillWand.fillSelectionSize;
        }

        public override void PostUpdateEquips()
        {
            if (InfinitePlayerRange)
            {
                Player.tileRangeX = Main.screenWidth / 16 / 2 + 5;
                Player.tileRangeY = Main.screenHeight / 16 / 2 + 4;
            }

            if (InfinitePlacementRange)
            {
                player.blockRange = Main.screenWidth / 16 / 2 + 5;
            }

            if (FastPlacement)
            {
                player.wallSpeed = player.wallSpeed + 10;
                player.tileSpeed = player.tileSpeed + 50;
            }

            if (InfinitePickupRange)
            {
                Player.defaultItemGrabRange = 1000000;
            }
        }

        public override void OnEnterWorld(Player player)
        {
            base.OnEnterWorld(player);
            //Update UI Wheel selected indexes and stuff
        }
    }
}
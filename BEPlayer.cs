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
        public bool InfinitePlacement { get; set; } //to be done
        public bool PlacementAnywhere { get; set; }
        public bool InfinitePickupRange { get; set; }

        public override void ResetEffects()
        {
            InfinitePlacementRange = InfinitePlayerRange = FastPlacement 
                = InfinitePlacement = PlacementAnywhere = InfinitePickupRange = false;
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
            Player.tileRangeX = InfinitePlayerRange ? Main.screenWidth / 16 / 2 + 5 : 5;
            Player.tileRangeY = InfinitePlayerRange ? Main.screenHeight / 16 / 2 + 4 : 4;
            player.blockRange = InfinitePlacementRange ? Main.screenWidth / 16 / 2 + 5 : 0;
            player.wallSpeed = FastPlacement ? player.wallSpeed + 10 : 1;
            player.tileSpeed = FastPlacement ? player.tileSpeed + 50 : 1;
            Player.defaultItemGrabRange = InfinitePickupRange ? 1000000 : 38; //I have no idea how much it should be so that should suffice??
        }

        public override void OnEnterWorld(Player player)
        {
            base.OnEnterWorld(player);
            //Update UI Wheel selected indexes and stuff
        }
    }
}
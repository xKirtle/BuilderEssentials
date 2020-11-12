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
        public bool InfiniteRange { get; set; }
        public bool InfinitePlacement { get; set; }
        public bool PlacementAnywhere { get; set; }

        public override void ResetEffects()
        {
            InfiniteRange = InfinitePlacement = PlacementAnywhere = false;
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
            Player.tileRangeX = InfiniteRange ? Main.screenWidth / 16 / 2 + 5 : 5;
            Player.tileRangeY = InfiniteRange ? Main.screenHeight / 16 / 2 + 4 : 4;
        }

        public override void OnEnterWorld(Player player)
        {
            base.OnEnterWorld(player);
            //Update UI Wheel selected indexes and stuff
        }
    }
}
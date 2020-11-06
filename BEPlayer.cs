using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BuilderEssentials.Items;

namespace BuilderEssentials
{
    public class BEPlayer : ModPlayer
    {
        public bool ValidCursorPos => Player.tileTargetX > 0 && Player.tileTargetX < Main.maxTilesX &&
                                      Player.tileTargetY > 0 && Player.tileTargetY < Main.maxTilesY;

        public Vector2 PointedCoord => Main.MouseWorld;
        public Vector2 PointedTileCoord => new Vector2(Player.tileTargetX, Player.tileTargetY);
        public Tile PointedTile => Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
        public bool InfiniteRange { get; set; }
        public bool InfinitePlacement { get; set; } = true;
        public bool PlacementAnywhere { get; set; }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (BuilderEssentials.IncreaseFillToolSize.JustPressed && FillWand.fillSelectionSize < 6)
                ++FillWand.fillSelectionSize;

            if (BuilderEssentials.DecreaseFillToolSize.JustPressed && FillWand.fillSelectionSize > 1)
                --FillWand.fillSelectionSize;
        }

        public override void OnEnterWorld(Player player)
        {
            base.OnEnterWorld(player);
            //Update UI Wheel selected indexes and stuff
        }
    }
}
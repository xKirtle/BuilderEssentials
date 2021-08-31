using System;
using System.Reflection;
using BuilderEssentials.UI.Elements.ShapesDrawer;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Utilities
{
    internal partial class HelperMethods
    {
        private static int mouseLeftTimer = 0;
        internal static void MirrorPlacement(On.Terraria.Player.orig_PlaceThing orig, Player self)
        {
            BEPlayer mp = Main.LocalPlayer.GetModPlayer<BEPlayer>();
            MirrorWandSelection sel = GameUIState.Instance.mirrorWandSelection;
            CoordsSelection cs = sel.cs;

            if (Main.mouseLeft && sel.IsMouseWithinSelection() && ++mouseLeftTimer == 1)
            {
                //Mirror coords
                Vector2 mirrorStart = cs.LMBStart;
                Vector2 mirrorEnd = cs.LMBEnd;

                //Check if coords intersect the mirror axis
                if (!IsWithinRange(Player.tileTargetY, mirrorStart.X, mirrorEnd.X, true) &&
                    !IsWithinRange(Player.tileTargetY, mirrorStart.Y, mirrorEnd.Y, true)) return;
                
                //Check if placement is on top of a player
                if (IsPlayerOnTopOfMouse()) return;

                if (!sel.horizontalMirror)
                {
                    Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
                    TileObjectData data = TileObjectData.GetTileData(tile);
                    int offsetCompensation = 0;
                    if (data != null)
                    {
                        var tileOrigin = TileObjectData.GetTileData(tile).Origin;
                        int tileSize = TileObjectData.GetTileData(tile).CoordinateFullWidth / 16;

                        //A bit scuffed but it works? Might break with mod compat
                        if (tileSize == 2 && (tileOrigin == new Point16(0, 0) || tileOrigin == new Point16(0, 1) ||
                                              tileOrigin == new Point16(0, 2)) || tileOrigin == new Point16(0, 4) || tileOrigin == new Point16(1, 2))
                            offsetCompensation -= 1;

                        if (tileSize == 2 && tileOrigin == new Point16(1, 1))
                            offsetCompensation += 1;

                        if (tileSize == 4 && (tileOrigin == new Point16(1, 1) || tileOrigin == new Point16(1, 3)))
                            offsetCompensation -= 1;

                        if (tileSize == 4 && tileOrigin == new Point16(1, 2))
                            offsetCompensation -= 2;

                        if (tile.type == TileID.Painting6X4)
                            offsetCompensation = -1;

                        if (tile.type == TileID.Statues || tile.type == TileID.AlphabetStatues ||
                            tile.type == TileID.BoulderStatue || tile.type == TileID.MushroomStatue ||
                            tile.type == TileID.WaterFountain)
                            offsetCompensation = 1;
                    }
                    
                    float minMirrorX = mirrorStart.X < mirrorEnd.X ? mirrorStart.X : mirrorEnd.X;
                    bool LeftOfTheMirror = Player.tileTargetX < minMirrorX;
                    float distanceToMirror = Math.Abs(Player.tileTargetX - mirrorStart.X) < Math.Abs(Player.tileTargetX - mirrorEnd.X) 
                        ? Math.Abs(Player.tileTargetX - mirrorStart.X) 
                        : Math.Abs(Player.tileTargetX - mirrorEnd.X);

                    int oldTargetX = Player.tileTargetX;
                    Player.tileTargetX += LeftOfTheMirror 
                        ? (int)(distanceToMirror * 2 + (sel.wideMirror ? 1 : 0) + offsetCompensation) 
                        : -(int)(distanceToMirror * 2 + (sel.wideMirror ? 1 : 0) + offsetCompensation);

                    int oldRangeX = Player.tileRangeX;
                    int oldRangeY = Player.tileRangeY;
                    Player.tileRangeX += 200;
                    Player.tileRangeY += 200;
                    
                    int oldDir = mp.Player.direction;
                    mp.Player.direction *= -1;
                    
                    
                    //mp.Player.PlaceThing();
                    //TODO: Requires me to try to place a tile twice, whyyyyyyyyyyy
                    orig.Invoke(self);

                    Player.tileTargetX = oldTargetX;
                    Player.tileRangeX = oldRangeX;
                    Player.tileRangeY = oldRangeY;
                    mp.Player.direction = oldDir;

                    Main.NewText("before plaer placething");
                }
            }

            if (Main.mouseLeftRelease)
                mouseLeftTimer = 0;
        }
    }
}
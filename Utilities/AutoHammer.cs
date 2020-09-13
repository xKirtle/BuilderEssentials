using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Utilities
{
    public static partial class Tools
    {
        public static void ChangeSlope(int slopeType)
        {
            int posX = Player.tileTargetX;
            int posY = Player.tileTargetY;
            Tile tile = Framing.GetTileSafely(posX, posY);
            Tile tileClone = (Tile)tile.Clone();

            if (tile.type >= 0 && tile.active())
            {
                switch (slopeType)
                {
                    case 0:
                        tile.halfBrick(false);
                        tile.slope(1);
                        break;
                    case 1:
                        tile.halfBrick(false);
                        tile.slope(2);
                        break;
                    case 2:
                        tile.halfBrick(false);
                        tile.slope(3);
                        break;
                    case 3:
                        tile.halfBrick(false);
                        tile.slope(4);
                        break;
                    case 4:
                        tile.halfBrick(true);
                        tile.slope(0);
                        break;
                    case 5:
                        tile.halfBrick(false);
                        tile.slope(0);
                        break;
                    default:
                        break;
                }

                Tile tileAfter = Framing.GetTileSafely(posX, posY);

                if (tileClone.slope() != tileAfter.slope() || tileClone.halfBrick() != tileAfter.halfBrick())
                    Main.PlaySound(SoundID.Dig);

                WorldGen.SquareTileFrame(posX, posY, false);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, posX, posY, 1);
            }
        }
    }
}
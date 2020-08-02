using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Utilities
{
    public static partial class Tools
    {
        public static void ChangeSlope(ref int oldPosX, ref int oldPosY, ref Tile previousClickedTile, int selectedIndex)
        {
            int posX = Player.tileTargetX;
            int posY = Player.tileTargetY;
            Tile tile = Main.tile[posX, posY];

            if (tile.type >= 0 && tile.active())
            {
                switch (selectedIndex)
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
                }

                WorldGen.SquareTileFrame(posX, posY, true);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, posX, posY, 1);

                if (previousClickedTile != null)
                {
                    if (!previousClickedTile.HasSameSlope(tile) || (oldPosX != posX || oldPosY != posY))
                        Main.PlaySound(SoundID.Dig);
                }
                else
                    Main.PlaySound(SoundID.Dig);

                previousClickedTile = tile;
                oldPosX = posX;
                oldPosY = posY;
            }
        }
    }

}
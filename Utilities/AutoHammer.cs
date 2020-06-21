using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Utilities
{

    public class AutoHammer
    {
        public static void ChangeSlope(ref int oldPosX, ref int oldPosY, ref Tile previousClickedTile)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            //TODO: DISABLE USE ON GAME INTERFACES WHEN USER CLICKS ON SETTINGS FOR EXAMPLE
            int posX = Player.tileTargetX;
            int posY = Player.tileTargetY;
            Tile tile = Main.tile[posX, posY];

            if (tile.type >= 0 && tile.active())
            {
                switch (modPlayer.autoHammerSelectedIndex)
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
                        tile.slope(0);
                        tile.halfBrick(true);
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
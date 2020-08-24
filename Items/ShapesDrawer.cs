using BuilderEssentials.UI;
using BuilderEssentials.UI.ShapesDrawing;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class ShapesDrawer : ModItem
    {
        BaseShape bs = BaseShape.Instance;
        public override string Texture => "BuilderEssentials/Textures/Items/ShapesDrawer";

        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to draw shapes");

        public static bool channeling;
        public static int selectedItemType = -1;
        int oldPosX;
        int oldPosY;
        public override void HoldItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();
                ShapesMenu.SDEquipped = true;
                channeling = Main.mouseRight;

                //Middle mouse
                if (Main.mouseMiddle && !player.mouseInterface &&
                        (modPlayer.pointedTilePos.X != oldPosX || modPlayer.pointedTilePos.Y != oldPosY))
                {
                    selectedItemType = Tools.PickItem(modPlayer.pointedTile, false);
                    oldPosX = (int)modPlayer.pointedTilePos.X;
                    oldPosY = (int)modPlayer.pointedTilePos.Y;
                }

                if (selectedItemType != -1)
                {
                    player.showItemIcon = true;
                    player.showItemIcon2 = selectedItemType;
                }
            }
        }
    }
}

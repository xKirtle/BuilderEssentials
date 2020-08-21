using BuilderEssentials.UI;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class ShapesDrawer : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/ShapesDrawer";

        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to draw shapes");

        public override void HoldItem(Player player) => ShapesMenu.SDEquipped = true;
    }
}

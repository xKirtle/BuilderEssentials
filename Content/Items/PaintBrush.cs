using BuilderEssentials.Common;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class PaintBrush : BasePaintBrush
{
    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.Paintbrush)
            .AddIngredient(ItemID.PaintRoller)
            .AddIngredient(ItemID.PaintScraper)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
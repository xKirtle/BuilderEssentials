using Terraria.ID;

namespace BuilderEssentials.Content.Items;

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
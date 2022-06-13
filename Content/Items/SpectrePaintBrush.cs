using Terraria.ID;

namespace BuilderEssentials.Content.Items;

public class SpectrePaintBrush : BasePaintBrush
{
    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.SpectrePaintbrush)
            .AddIngredient(ItemID.SpectrePaintRoller)
            .AddIngredient(ItemID.SpectrePaintScraper)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
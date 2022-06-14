using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
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
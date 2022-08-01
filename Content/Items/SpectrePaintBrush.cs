using BuilderEssentials.Common.Configs;
using BuilderEssentials.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class SpectrePaintBrush : BasePaintBrush
{
    public override int ItemRange => 16;
    protected override bool CloneNewInstances => true;

    public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<ServerConfig>().EnabledItems.SpectrePaintBrush;

    public override void SetDefaults() {
        base.SetDefaults();

        Item.useTime = Item.useAnimation = 7;
    }

    public override void HoldItem(Player player) {
        base.HoldItem(player);
        if (player.whoAmI != Main.myPlayer)
            return;

        Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
        PaintBrushPanel panel = ToggleableItemsUIState.GetUIPanel<PaintBrushPanel>();

        switch (panel.toolIndex) {
            case 0:
                if (tile.TileType >= 0 && tile.HasTile)
                    player.cursorItemIconID = ItemID.SpectrePaintbrush;
                break;
            case 1:
                if (tile.WallType > 0)
                    player.cursorItemIconID = ItemID.SpectrePaintRoller;
                break;
            case 2:
                if (tile.TileColor != 0 || tile.WallColor != 0)
                    player.cursorItemIconID = ItemID.SpectrePaintScraper;
                break;
        }
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.SpectrePaintbrush)
        .AddIngredient(ItemID.SpectrePaintRoller)
        .AddIngredient(ItemID.SpectrePaintScraper)
        .AddTile(TileID.MythrilAnvil)
        .Register();
}
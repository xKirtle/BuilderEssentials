using System;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Configs;
using BuilderEssentials.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class PaintBrush : BasePaintBrush
{
    public override int ItemRange => 10;
    protected override bool CloneNewInstances => true;

    public override bool IsLoadingEnabled(Mod mod)
        => ModContent.GetInstance<MainConfig>().EnabledItems.PaintBrush;
    
    public override void HoldItem(Player player) {
        base.HoldItem(player);
        if (player.whoAmI != Main.myPlayer) return;

        Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
        var panel = ToggleableItemsUIState.GetUIPanel<PaintBrushPanel>();
        
        switch (panel.toolIndex) {
            case 0:
                if (tile.TileType >= 0 && tile.HasTile)
                    player.cursorItemIconID = ItemID.Paintbrush;
                break;
            case 1:
                if (tile.WallType > 0)
                    player.cursorItemIconID = ItemID.PaintRoller;
                break;
            case 2:
                if (tile.TileColor != 0 || tile.WallColor != 0)
                    player.cursorItemIconID = ItemID.PaintScraper;
                break;
        }
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.Paintbrush)
            .AddIngredient(ItemID.PaintRoller)
            .AddIngredient(ItemID.PaintScraper)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
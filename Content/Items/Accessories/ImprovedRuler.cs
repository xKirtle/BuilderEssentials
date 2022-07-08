using BuilderEssentials.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items.Accessories;

public class ImprovedRuler : ModItem
{
    public override string Texture => "BuilderEssentials/Assets/Items/Accessories/" + GetType().Name;

    public override void SetStaticDefaults() {
        Tooltip.SetDefault("Only works when equipped and player's empty handed." +
                           "\nHold Left Click to draw a line." +
                           "\nHold Right Click to curve the line.");
    }

    public override void SetDefaults() {
        Item.accessory = true;
        Item.vanity = false;
        Item.width = Item.height = 42;
        Item.value = Item.sellPrice(silver: 10);
        Item.rare = ItemRarityID.Red;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (player.whoAmI != Main.myPlayer) return;

        var panel = ShapesUIState.GetUIPanel<ImprovedRulerPanel>();
        if (!panel.IsVisible)
            ShapesUIState.TogglePanelVisibility<ImprovedRulerPanel>();
    }
    
    public override void AddRecipes() {
        CreateRecipe()
        .AddIngredient(ItemID.Ruler)
        .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}
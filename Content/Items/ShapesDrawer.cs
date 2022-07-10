using System;
using BuilderEssentials.Common;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class ShapesDrawer : BuilderEssentialsItem
{
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("Used to draw shapes" +
           "\nOpen its menu by clicking the arrow on the left of your screen when equipped" +
           "\nLeft Click to make selection" +
           "\nMiddle Click to select working tile" +
           "\nRight Click to place blocks in the selection" +
           "\n[c/FFCC00:Press LShift to make circles/squares]" +
           "\n[c/FFCC00:Enables a selection menu on the left of the screen]");
    
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
    
        public override void SetDefaults() {
            Item.height = Item.width = 40;
            Item.useTime = Item.useAnimation = 10;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(silver: 4);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.noMelee = true;
        }
    
        public override Vector2? HoldoutOffset() => new Vector2(-2, -9);
    
        public override void HoldItem(Player player) {
            if (player.whoAmI != Main.myPlayer) return;
    
            var panel = ShapesUIState.GetUIPanel<ShapesDrawerPanel>();
    
            if (Main.mouseMiddle && Main.mouseMiddleRelease && !player.mouseInterface) {
                Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
    
                //Does not support multi tiles
                if (TileObjectData.GetTileData(tile) == null) {
                    var itemType = ItemPicker.PickItem(tile);
                    panel.SetSelectedItem(itemType);
                    Item.tileWand = itemType;
                }
            }
    
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = Type;
            if (!player.cursorItemIconEnabled) return;
            player.cursorItemIconID = panel.SelectedItem?.type ?? Type;
        }
        
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CopperPickaxe)
                .AddIngredient(ItemID.CopperAxe)
                .AddIngredient(ItemID.CopperHammer)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.TinPickaxe)
                .AddIngredient(ItemID.TinAxe)
                .AddIngredient(ItemID.TinHammer)
                .AddTile(TileID.Anvils)
                .Register();
        }
}
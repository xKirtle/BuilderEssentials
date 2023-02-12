using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Configs;
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
    public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<ServerConfig>().EnabledItems.ShapesDrawer;

    public override void SetStaticDefaults() {
        Tooltip.SetDefault("Used to draw shapes" +
            "\nRight Click to make selection" +
            "\nLeft Click to place blocks in the selection" +
            "\nMiddle Click to select working tile" +
            "\n[c/FFCC00:Press LShift to make circles/squares]" +
            "\n[c/FFCC00:Open its menu by clicking the squirrel builder under your opened inventory (must be being held)]");

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

    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        int insertIndex = tooltips.FindIndex(x => x.Text.Contains("Middle Click to select working tile"));
        var assignedKeys = BuilderEssentials.UndoPlacement?.GetAssignedKeys();
        string hotKey = assignedKeys == null || !assignedKeys.Any() ? "[NO HOTKEY ASSIGNED]" : assignedKeys[0];
        
        if (insertIndex == -1) {
            tooltips.Add(new TooltipLine(Mod, "BuilderEssentials:FillWand", $"Press {hotKey} to undo placements"));
        }
        else {
            tooltips.Insert(insertIndex + 1, new TooltipLine(Mod, "BuilderEssentials:FillWand", $"Press {hotKey} to undo placements"));
        }
    }

    public override Vector2? HoldoutOffset() => new Vector2(-2, -9);

    public override void HoldItem(Player player) {
        if (player.whoAmI != Main.myPlayer)
            return;

        ShapesDrawerPanel panel = ShapesUIState.GetUIPanel<ShapesDrawerPanel>();

        if (Main.mouseMiddle && Main.mouseMiddleRelease && !player.mouseInterface) {
            Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);

            //Does not support multi tiles
            if (TileObjectData.GetTileData(tile) == null) {
                int itemType = ItemPicker.PickItem(tile);
                panel.SetSelectedItem(itemType);
                Item.tileWand = itemType;
            }
        }

        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = Type;
        if (!player.cursorItemIconEnabled)
            return;
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
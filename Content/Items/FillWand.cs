using System;
using System.Collections.Generic;
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
public class FillWand : BuilderEssentialsItem
{
    public static int FillSelectionSize = 3;

    protected override bool CloneNewInstances => true;

    public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<MainConfig>().EnabledItems.FillWand;

    public override void SetStaticDefaults() {
        Tooltip.SetDefault("Fills or creates holes!" +
            "\nLeft Click to place" +
            "\nMiddle Click to select working tile" +
            "\n[c/FFCC00:Use hotkeys to increase/decrease selection size]" +
            "\n[c/FF0000:Does not support multi tiles]");

        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
        Item.height = Item.width = 46;
        Item.useTime = Item.useAnimation = 10;
        Item.useTurn = true;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.value = Item.sellPrice(gold: 1, silver: 40);
        Item.rare = ItemRarityID.Red;
        Item.autoReuse = true;
        Item.noMelee = true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        int insertIndex = tooltips.FindIndex(x => x.Text.Contains("Middle Click to select working tile"));

        if (insertIndex == -1) {
            tooltips.Add(new TooltipLine(Mod, "BuilderEssentials:FillWand",
                $"Press {BuilderEssentials.UndoPlacement?.GetAssignedKeys()[0]} to undo placements"));
        }
        else {
            tooltips.Insert(insertIndex + 1, new TooltipLine(Mod, "BuilderEssentials:FillWand",
                $"Press {BuilderEssentials.UndoPlacement?.GetAssignedKeys()[0]} to undo placements"));
        }
    }

    public override Vector2? HoldoutOffset() => new Vector2(-2, -7);

    public override void HoldItem(Player player) {
        if (player.whoAmI != Main.myPlayer)
            return;

        FillWandPanel panel = ShapesUIState.GetUIPanel<FillWandPanel>();

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

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.MoltenHamaxe)
        .AddIngredient(ItemID.DirtRod)
        .AddTile(TileID.Anvils)
        .Register();
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Common;

[Autoload(false)]
public abstract class BuilderEssentialsItem : ModItem
{
    public override string Texture => "BuilderEssentials/Assets/Items/" + GetType().Name;
    // public override string Texture => $"{GetType().Namespace.Replace(".", "/").Replace("Content", "Assets")}/{GetType().Name}";

    public virtual int ItemRange { get; protected set; } = 8;

    public override void HoldItem(Player player) {
        if (player.whoAmI != Main.myPlayer)
            return;

        if (ItemHasRange()) {
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = Type;
        }
    }

    //TODO: Player.IsTargetTileInItemRange
    public bool ItemHasRange(float itemRangeInTiles = default) {
        Vector2 screenPosition = Main.screenPosition.ToTileCoordinates().ToVector2();
        Vector2 playerCenterScreen = Main.LocalPlayer.Center.ToTileCoordinates().ToVector2() - screenPosition;
        Vector2 mouseCoords = Main.MouseScreen.ToTileCoordinates().ToVector2();

        itemRangeInTiles = itemRangeInTiles == default ? ItemRange : itemRangeInTiles;
        return Math.Abs(playerCenterScreen.X - mouseCoords.X) <= itemRangeInTiles &&
            Math.Abs(playerCenterScreen.Y - mouseCoords.Y) <= itemRangeInTiles - 2;
    }
}
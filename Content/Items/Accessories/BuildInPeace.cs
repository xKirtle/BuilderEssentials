using BuilderEssentials.Common.Configs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items.Accessories;

public class BuildInPeace : ModItem
{
    public override string Texture => "BuilderEssentials/Assets/Items/Accessories/" + GetType().Name;

    public override void SetStaticDefaults() {
        Tooltip.SetDefault("Disables all events in the game and enemy spawns. Also sets the clock to noon");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<ServerConfig>().EnabledAccessories.BuildInPeace;

    public override void SetDefaults() {
        Item.accessory = true;
        Item.vanity = false;
        Item.width = Item.height = 42;
        Item.value = Item.sellPrice(0, 50, 0, 0);
        Item.rare = ItemRarityID.Red;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        Main.bloodMoon = false;
        Main.StopRain();
        Main.stopMoonEvent();
        Main.StopSlimeRain(false);
        Main.eclipse = false;
        Main.invasionType = 0;
        Main.invasionDelay = 0;
        Main.invasionSize = 0;
        Main.dayTime = true;
        Main.time = 27000; //mid day
        Main.fastForwardTime = false;

        foreach (NPC npc in Main.npc) {
            //Don't want to remove town NPC's
            if (!npc.townNPC)
                npc.active = false;
        }
    }
}
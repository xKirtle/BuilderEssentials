using System.Collections.Generic;
using BuilderEssentials.Buffs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    internal class BuildInPeace : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/BuildInPeace";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Disables all events in the game and sets the clock to 12pm daytime");

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.vanity = false;
            Item.width = 42;
            Item.height = 42;
            Item.value = Item.sellPrice(0,50, 0, 0);
            Item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) 
            => player.AddBuff(ModContent.BuffType<BuildInPeaceBuff>(), 10);
    }

    internal class DropItemOnNpcLoot : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.MoonLordCore)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BuildInPeace>()));
        }
    }
}
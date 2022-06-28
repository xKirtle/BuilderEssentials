using BuilderEssentials.Content.Items.Accessories;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Common.GlobalItems;

public class GlobalNPCLoot : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
        if (npc.type == NPCID.MoonLordCore)
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BuildInPeace>()));
    }
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BuilderEssentials.Items.Accessories;
using Microsoft.Xna.Framework;

namespace BuilderEssentials.Buffs
{
    class BuildInPeaceBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Build Peacefully");
            Description.SetDefault("It's a perfect time to build!");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            Main.raining = false;
            Main.bloodMoon = false;
            Main.StopSlimeRain(false);
            Main.snowMoon = false;
            Main.pumpkinMoon = false;
            Main.eclipse = false;
            Main.invasionType = 0;
            Main.invasionDelay = 0;
            Main.invasionSize = 0;
            Main.dayTime = true;
            Main.time = 27000; //mid day
            Main.fastForwardTime = false;

            foreach (var npc in Main.npc)
            {
                //Don't want to remove town NPC's
                if (!npc.townNPC)
                    npc.active = false;
            }
        }
    }

    public class DropItemOnNpcLoot : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.MoonLordCore)
            {
                //Might only drop one in Multiplayer? Needs testing
                Item.NewItem(npc.Center, ModContent.ItemType<BuildInPeace>());
            }
        }
    }
}

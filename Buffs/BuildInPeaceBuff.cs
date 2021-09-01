using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Buffs
{
    internal class BuildInPeaceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Build Peacefully");
            Description.SetDefault("It's a perfect time to build!");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            CanBeCleared = false;
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
}
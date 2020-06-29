using Terraria.ID;
using System;
using Terraria;
using Terraria.ModLoader;
using BuilderEssentials.Utilities;
using static BuilderEssentials.BuilderPlayer;

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
            BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();

            //TODO: If invasion is summoned with an item, maybe make that item not be used?

            Main.raining = false;
            Main.bloodMoon = false;
            Main.StopSlimeRain(false);
            Main.snowMoon = false;
            Main.pumpkinMoon = false;
            Main.eclipse = false;
            Main.invasionType = 0;
            Main.invasionDelay = 0;
            Main.invasionSize = 0;
            Main.time = 27000; //mid day
            Main.fastForwardTime = false;

            foreach (var npc in Main.npc)
            {
                //Don't want to remove town NPC's
                if (!Tools.IsTownNpc(npc))
                    npc.active = false;
            }
        }
    }
}

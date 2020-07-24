using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BuilderEssentials.Items.Accessories;

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
            Main.dayTime = true;
            Main.time = 27000; //mid day
            Main.fastForwardTime = false;

            foreach (var npc in Main.npc)
            {
                //Don't want to remove town NPC's
                if (npc.townNPC)
                    npc.active = false;
            }
        }
    }

    public class BuildInPeaceSummonItem : GlobalItem
    {
        static bool summonedBossWhileBuffIsOn = false;
        public override bool UseItem(Item item, Player player)
        {
            if (player.HasBuff(mod.BuffType("BuildInPeaceBuff")))
            {
                if (item.type == ItemID.SuspiciousLookingEye || item.type == ItemID.BloodySpine || item.type == ItemID.WormFood ||
                    item.type == ItemID.MechanicalWorm || item.type == ItemID.MechanicalSkull || item.type == ItemID.MechanicalEye ||
                    item.type == ItemID.LihzahrdPowerCell || item.type == ItemID.SnowGlobe || item.type == ItemID.NaughtyPresent ||
                    item.type == ItemID.SlimeCrown || item.type == ItemID.Abeemination || item.type == ItemID.PumpkinMoonMedallion ||
                    item.type == ItemID.PirateMap || item.type == ItemID.SolarTablet || item.type == ItemID.CelestialSigil ||
                    item.type == ItemID.DD2ElderCrystal || item.type == ItemID.GoblinBattleStandard)
                {
                    if (!summonedBossWhileBuffIsOn)
                    {
                        Main.NewText("You can't summon bosses or invasions while you have the Build In Peace accessory equipped.", Color.Red);
                        summonedBossWhileBuffIsOn = true;
                    }
                    item.stack += 1;
                    return false;
                }
            }
            return base.UseItem(item, player);
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

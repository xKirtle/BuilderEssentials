using Terraria;
using Terraria.ModLoader;
using static BuilderEssentials.Utilities.Tools;

namespace BuilderEssentials.Buffs
{
    class InfinitePlacementBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Infinite Placement");
            Description.SetDefault("Where are all these materials coming from?");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();

            if (!modPlayer.creativeWheelSelectedIndex.Contains(CreativeWheelItem.InfinityUpgrade.ToInt()))
                modPlayer.creativeWheelSelectedIndex.Add(CreativeWheelItem.InfinityUpgrade.ToInt());
        }
    }
}

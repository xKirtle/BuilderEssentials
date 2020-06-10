using Terraria;
using Terraria.ModLoader;

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
            Main.buffAlpha[Type] = 0;
            canBeCleared = false;
        }
    }
}

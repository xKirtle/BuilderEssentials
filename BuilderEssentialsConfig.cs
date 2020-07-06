using BuilderEssentials.Utilities;
using Terraria;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BuilderEssentials
{
    public class BuilderEssentialsConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header("Building Mode Options (Require Mod Reload)")]
        [Label("Different Accessories while on Building Mode")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool accessories;
        [Label("Different Vanity Accessories while on Building Mode")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool vanityAccessories;
        [Label("Different Armor Set while on Building Mode")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool armor;
        [Label("Different Vanity Armor Set while on Building Mode")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool vanityArmor;
        [Label("Different Misc Equipements while on Building Mode")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool miscEquips;
        [Label("Different Dyes while on Building Mode")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool dyes;

        public override void OnLoaded()
        {
            BuilderEssentials.accessories = accessories;
            BuilderEssentials.vanityAccessories = vanityAccessories;
            BuilderEssentials.armor = armor;
            BuilderEssentials.vanityArmor = vanityArmor;
            BuilderEssentials.miscEquips = miscEquips;
            BuilderEssentials.dyes = dyes;
        }
    }
}

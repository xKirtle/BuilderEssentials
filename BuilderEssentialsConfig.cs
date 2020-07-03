using BuilderEssentials.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override void OnChanged()
        {
            Tools.accessories = accessories;
            Tools.vanityAccessories = vanityAccessories;
            Tools.armor = armor;
            Tools.vanityArmor = vanityArmor;
            Tools.miscEquips = miscEquips;
            Tools.dyes = dyes;
        }

        public override void OnLoaded()
        {
            Tools.accessories = accessories;
            Tools.vanityAccessories = vanityAccessories;
            Tools.armor = armor;
            Tools.vanityArmor = vanityArmor;
            Tools.miscEquips = miscEquips;
            Tools.dyes = dyes;
        }
    }
}

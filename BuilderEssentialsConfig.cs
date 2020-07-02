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
        [Header("Building Mode Options")]
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

        public override void OnChanged()
        {
            Tools.accessories = accessories;
            Tools.vanityAccessories = vanityAccessories;
            Tools.armor = armor;
            Tools.vanityArmor = vanityArmor;
            Tools.miscEquips = miscEquips;
        }

        public override void OnLoaded()
        {
            Tools.miscEquips = miscEquips;
            //Tools.LoadedConfig();
        }
    }
}

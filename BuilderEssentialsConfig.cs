using BuilderEssentials.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace BuilderEssentials
{
    public class BuilderEssentialsConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Pet changes with Building Mode")]
        public bool changePet;
        [Label("Light Pet changes with Building Mode")]
        public bool changeLightPet;
        [Label("Minecart changes with Building Mode")]
        public bool changeMinecart;
        [Label("Mount changes with Building Mode")]
        public bool changeMount;
        [Label("Hook changes with Building Mode")]
        public bool changeHook;

        private List<bool> miscEquipsList = new List<bool>(5);

        public override void OnChanged()
        {
            miscEquipsList.Clear();
            miscEquipsList.Add(changePet);
            miscEquipsList.Add(changeLightPet);
            miscEquipsList.Add(changeMinecart);
            miscEquipsList.Add(changeMount);
            miscEquipsList.Add(changeHook);

            Tools.miscEquipsList = miscEquipsList;
        }

        public override void OnLoaded()
        {
            miscEquipsList.Add(changePet);
            miscEquipsList.Add(changeLightPet);
            miscEquipsList.Add(changeMinecart);
            miscEquipsList.Add(changeMount);
            miscEquipsList.Add(changeHook);

            Tools.miscEquipsList = miscEquipsList;

            //Tools.LoadedConfig();
        }
    }
}

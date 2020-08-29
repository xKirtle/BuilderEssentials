using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BuilderEssentials
{
    public class BuilderEssentialsConfig : ModConfig
    {
        public static BuilderEssentialsConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;

        //---------------------------------------------------------
        [Header("Options")]
        [Label("Automatically replace held item if stack ends")]
        [DefaultValue(true)]
        public bool autoReplaceStack;

        public override void OnChanged() => BuilderEssentials.autoReplaceStack = autoReplaceStack;
        public override void OnLoaded() => BuilderEssentials.autoReplaceStack = autoReplaceStack;
    }
}

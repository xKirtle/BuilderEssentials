using BuilderEssentials.Items.Accessories;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Commands
{
    class EventCommand : ModCommand
    {
        public override string Command => "x";
        public override string Description => "Plays a random test event";
        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            //Main.LocalPlayer.builderAccStatus[1] = 1;

            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<FastWrench>())
            {
                FastWrench wrench = (FastWrench) ModContent.GetModItem(Main.LocalPlayer.HeldItem.type);
                //wrench.upgrades[int.Parse(args[0])] = true;
                // wrench.SetUpgrade((HelperMethods.WrenchUpgrade) int.Parse(args[0]),
                //     !wrench.GetUpgrade((HelperMethods.WrenchUpgrade) int.Parse(args[0])));

                //Main.NewText($"Value is {wrench.upgrades[int.Parse(args[0])]}");
            }
        }
    }
}
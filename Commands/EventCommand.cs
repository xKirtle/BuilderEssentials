using BuilderEssentials.UI;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Commands
{
    class EventCommand : ModCommand
    {
        public override string Command => "event";
        public override string Description => "Plays a random test event";
        public override CommandType Type => CommandType.Chat;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
        }
    }
}

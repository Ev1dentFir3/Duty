using Rocket.Unturned.Player;
using Rocket.API;
using System.Collections.Generic;

namespace EFG.Duty
{
    public class CommandDuty : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller == null) return;
            UnturnedPlayer player = (UnturnedPlayer)caller;
            Duty.Instance.duty(player);
        }

        public string Help
        {
            get { return "Gives admin powers to the player without the need of the console."; }
        }

        public string Name
        {
            get { return "duty"; }
        }

        public string Syntax
        {
            get { return ""; }
        }

        public bool AllowFromConsole
        {
            get { return false; }
        }

        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { "d" }; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "duty.duty" };
            }
        }
    }
}

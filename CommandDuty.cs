using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using Rocket.API;
using SDG;
using System;
using System.Collections.Generic;
using System.Linq;

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
            get { return "Let your staff do their job!"; }
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

        public List<string> Aliases {
            get { return new List<string>() { "d" }; }
        }
        public List<string> Permissions
        {
            get
            {
                return new List<string>();
            }
        }
    }
}

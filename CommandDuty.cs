using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFG
{
    public class CommandDuty : IRocketCommand
    {
        public void Execute(RocketPlayer caller, string[] command)
        {
            if (caller.IsAdmin) caller.Admin(false); else caller.Admin(true);
            if (caller.IsAdmin){
                caller.Admin(false);
                caller.Features.GodMode = false;
                caller.Features.VanishMode = false;
            }
            else{
                caller.Admin(true);
            }
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

        public bool RunFromConsole
        {
            get { return false; }
        }

        public List<string> Aliases {
            get { return new List<string>() { "d" }; }
        }
    }
}

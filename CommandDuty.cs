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
        public void Execute(RocketPlayer caller, string[] command)
        {
            Duty.Instance.duty(caller);
            
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

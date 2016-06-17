using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFG.Duty
{
    public class CommandDutycheck : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
             
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;
            UnturnedPlayer cplayer = UnturnedPlayer.FromName(command[0]);
            Duty.Instance.cduty(cplayer, player);

        }

        public string Help
        {
            get { return "It's the is that person on duty? Abuse Checker!"; }
        }

        public string Name
        {
            get { return "Duty Check"; }
        }

        public string Syntax
        {
            get { return "<checkname>"; }
        }

        public bool AllowFromConsole
        {
            get { return true; }
        }

        public AllowedCaller AllowedCaller
        {
            get { return Rocket.API.AllowedCaller.Player; }
        }
        public List<string> Aliases
        {
            get { return new List<string>() { "dc" }; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "duty.check" };
            }
        }
    }
}

using Rocket.Unturned.Player;
using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using Rocket.Core.Logging;

namespace EFG.Duty
{
    public class CommandDutycheck : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                if (caller is ConsolePlayer)
                {
                    Logger.LogWarning("No argument was specified. Please use \"dc <playername>\" to check on a player.");
                }
                else if (caller is UnturnedPlayer)
                {
                    UnturnedChat.Say(caller, "No argument was given. Please use \"/dc <playername>\" to check a player.");
                }
            }
            else if (command.Length > 0)
            {
                UnturnedPlayer cplayer = UnturnedPlayer.FromName(command[0]);
                Duty.Instance.cduty(cplayer, caller);
            }
        }

        public string Help
        {
            get { return "Checks if a player has admin powers or not."; }
        }

        public string Name
        {
            get { return "DutyCheck"; }
        }

        public string Syntax
        {
            get { return "<playername>"; }
        }

        public bool AllowFromConsole
        {
            get { return true; }
        }

        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
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

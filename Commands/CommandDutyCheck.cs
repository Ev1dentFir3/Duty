using System.Collections.Generic;
using System.Linq;
using EFG.Duty.Configuration;
using EFG.Duty.RocketExtended;
using JetBrains.Annotations;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Unturned.Player;

namespace EFG.Duty.Commands;

[UsedImplicitly]
public class CommandDutyCheck : RocketCommandWithTranslations
{
    public override AllowedCaller AllowedCaller => AllowedCaller.Both;

    public override string Help => "Checks if a player is in duty or not.";

    public override string Name => "DutyCheck";

    public override string Syntax => "<playername>";

    public override List<string> Aliases => new() { "dc" };

    public override List<string> Permissions => new() { Name, "duty.check" };

    private DutyConfiguration DutyConfiguration { get; }

    public CommandDutyCheck(DutyConfiguration configuration, TranslationList translations) : base(translations,
        new[]
        {
            "command_usage", "error_player_not_found", "check_on_duty_message", "check_on_duty_group_message"
        })
    {
        DutyConfiguration = configuration;
    }

    public override void Execute(IRocketPlayer caller, string[] command)
    {
        if (command.Length == 0)
        {
            SendMessage(caller, "command_usage", Name, Syntax);
            return;
        }

        var player = UnturnedPlayer.FromName(command[0]);
        if (player == null)
        {
            SendMessage(caller, "error_player_not_found", command[0]);
            return;
        }

        if (player.IsAdmin)
        {
            SendMessage(caller, "check_on_duty_message", DutyConfiguration.MessageColor, caller.DisplayName,
                player.DisplayName);
            return;
        }

        foreach (var targetGroup in DutyConfiguration.DutyPermissionGroups
                     .Select(group => R.Permissions.GetGroup(group.GroupId)).Where(targetGroup =>
                         targetGroup?.Members.Contains(player.CSteamID.ToString()) == true))
        {
            SendMessage(caller, "check_on_duty_group_message", DutyConfiguration.MessageColor, caller.DisplayName,
                player.DisplayName, targetGroup.DisplayName, targetGroup.Id);
            return;
        }

        SendMessage(caller, "check_off_duty_message");
    }
}
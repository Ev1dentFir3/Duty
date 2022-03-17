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
public class CommandDuty : RocketCommandWithTranslations
{
    public override AllowedCaller AllowedCaller => AllowedCaller.Player;

    public override string Name => "duty";

    public override string Syntax => "";

    public override string Help =>
        "Makes the user executing this command switch to a toggle-able permission group, or give/remove super admin.";

    public override List<string> Aliases => new() { "d" };

    private DutyConfiguration DutyConfiguration { get; }

    public CommandDuty(DutyConfiguration configuration, TranslationList translations) : base(translations,
        new[]
        {
            "off_duty_message", "on_duty_message", "off_duty_group_message", "on_duty_group_message",
            "no_super_or_group_permissions"
        })
    {
        DutyConfiguration = configuration;
    }

    public override void Execute(IRocketPlayer caller, string[] command)
    {
        if (caller is not UnturnedPlayer player) return;

        if (player.HasPermission(DutyConfiguration.SuperAdminPermission))
        {
            if (player.IsAdmin)
            {
                player.Admin(false);
                player.Features.GodMode = false;
                player.Features.VanishMode = false;
                if (DutyConfiguration.EnableServerAnnouncer)
                    SendMessage("off_duty_message", DutyConfiguration.MessageColor, player.DisplayName);
            }
            else
            {
                player.Admin(true);
                if (DutyConfiguration.EnableServerAnnouncer)
                    SendMessage("on_duty_message", DutyConfiguration.MessageColor, player.DisplayName);
            }

            return;
        }

        foreach (var targetGroup in from @group in DutyConfiguration.DutyPermissionGroups
                 where player.HasPermission(@group.PermissionRequired)
                 select R.Permissions.GetGroup(@group.GroupId)
                 into targetGroup
                 where targetGroup != null
                 select targetGroup)
        {
            if (targetGroup.Members.Contains(player.CSteamID.ToString()))
            {
                R.Permissions.RemovePlayerFromGroup(targetGroup.Id, player);
                player.Features.GodMode = false;
                player.Features.VanishMode = false;

                if (DutyConfiguration.EnableServerAnnouncer)
                    SendMessage("off_duty_group_message", DutyConfiguration.MessageColor, player.DisplayName,
                        targetGroup.DisplayName, targetGroup.Id);
            }
            else
            {
                R.Permissions.AddPlayerToGroup(targetGroup.Id, player);
                if (DutyConfiguration.EnableServerAnnouncer)
                    SendMessage("on_duty_group_message", DutyConfiguration.MessageColor, player.DisplayName,
                        targetGroup.DisplayName, targetGroup.Id);
            }

            return;
        }

        SendMessage(player, "no_super_or_group_permissions");
    }
}
using System.Linq;
using EFG.Duty.Commands;
using EFG.Duty.Configuration;
using EFG.Duty.Utilities;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace EFG.Duty;

public sealed class DutyPlugin : RocketPlugin<DutyConfiguration>
{
    public override TranslationList DefaultTranslations => new()
    {
        { "admin_login_message", "{0} has logged on and is now on duty." },
        { "group_duty_login_message", "{0} has logged on and is now on {1} [{2}] duty." },
        { "admin_logoff_message", "{0} has logged off and is now off duty." },
        { "group_duty_logoff_message", "{0} has logged off and is now off {1} [{2}] duty." },
        { "on_duty_message", "{0} is now on duty." }, { "off_duty_message", "{0} is now off duty." },
        { "on_duty_group_message", "{0} is now on {1} [{2}] duty." },
        { "off_duty_group_message", "{0} is now off {1} [{2}] duty." },
        {
            "no_super_or_group_permissions",
            "You are unable to go on duty as you lack Super Admin and any group duty permissions"
        },
        { "command_usage", "Wrong command usage, correct usage: /{0} {1}" },
        { "error_player_not_found", "Could not find a player with name {0}" },
        { "check_on_duty_message", "Player {0} is on duty." },
        { "check_on_duty_group_message", "Player {0} is on {1} [{2}] duty." }
    };

    protected override void Load()
    {
        Logging.Write(this, "Hooking onto event \"Player Connected\"...");
        U.Events.OnPlayerConnected += PlayerConnected;
        Logging.Write(this, "Hooking onto event \"Player Disconnected\"...");
        U.Events.OnPlayerDisconnected += PlayerDisconnected;

        Logging.Write(this, "Registering commands...");
        R.Commands.Register(new CommandDuty(Configuration.Instance, Translations.Instance));
        R.Commands.Register(new CommandDutyCheck(Configuration.Instance, Translations.Instance));

        Logging.Write(this, "Duty v2.0.0 by Pustalorc, originally by Ev1dentFir3 (EFG), has loaded successfully.");
    }

    protected override void Unload()
    {
        Logging.Write(this, "Unhooking from event \"Player Disconnected\"...");
        U.Events.OnPlayerDisconnected -= PlayerDisconnected;
        Logging.Write(this, "Unhooking from event \"Player Connected\"...");
        U.Events.OnPlayerConnected -= PlayerConnected;

        Logging.Write(this, "Duty v2.0.0 by Pustalorc, originally by Ev1dentFir3 (EFG), has unloaded successfully.");
    }

    public override string ToString()
    {
        return Name;
    }

    private void PlayerConnected(UnturnedPlayer player)
    {
        if (!Configuration.Instance.EnableServerAnnouncer)
            return;

        if (player.IsAdmin)
        {
            UnturnedChat.Say(Translate("admin_login_message", player.CharacterName),
                UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.red));

            return;
        }

        foreach (var targetGroup in Configuration.Instance.DutyPermissionGroups
                     .Select(group => R.Permissions.GetGroup(group.GroupId)).Where(targetGroup =>
                         targetGroup?.Members.Contains(player.CSteamID.ToString()) == true))
        {
            UnturnedChat.Say(
                Translate("group_duty_login_message", player.CharacterName, targetGroup.DisplayName,
                    targetGroup.Id), UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.red));

            return;
        }
    }

    private void PlayerDisconnected(UnturnedPlayer player)
    {
        if (player.IsAdmin)
        {
            if (Configuration.Instance.RemoveDutyOnLogout)
            {
                player.Admin(false);
                player.Features.GodMode = false;
                player.Features.VanishMode = false;
            }

            if (Configuration.Instance.EnableServerAnnouncer)
                UnturnedChat.Say(Translate("admin_logoff_message", player.CharacterName),
                    UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.red));
            return;
        }

        foreach (var targetGroup in Configuration.Instance.DutyPermissionGroups
                     .Select(group => R.Permissions.GetGroup(group.GroupId)).Where(targetGroup =>
                         targetGroup?.Members.Contains(player.CSteamID.ToString()) == true))
        {
            if (Configuration.Instance.RemoveDutyOnLogout)
            {
                player.Features.GodMode = false;
                player.Features.VanishMode = false;
                R.Permissions.RemovePlayerFromGroup(targetGroup.Id, player);
            }

            if (Configuration.Instance.EnableServerAnnouncer)
                UnturnedChat.Say(
                    Translate("group_duty_logoff_message", player.CharacterName, targetGroup.DisplayName,
                        targetGroup.Id),
                    UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.red));

            return;
        }
    }
}
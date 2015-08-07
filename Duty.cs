using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Chat;
using Rocket.Unturned;
using Rocket.Core.Plugins;
using Rocket.API.Collections;
using SDG;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFG.Duty
{
    public class Duty : RocketPlugin<DutyConfiguration>
    {

        public static Duty Instance;
        protected override void Load()
        {
            Duty.Instance = this;
            U.Events.OnPlayerConnected += PlayerConnected;
            U.Events.OnPlayerDisconnected += PlayerDisconnected;
        }
        public void duty(UnturnedPlayer caller)
        {
            if (caller.IsAdmin)
            {
                caller.Admin(false);
                caller.Features.GodMode = false;
                caller.Features.VanishMode = false;
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Duty.Instance.Translate("off_duty_message", caller.CharacterName), UnturnedChat.GetColorFromName(Duty.Instance.Configuration.Instance.MessageColor, Color.red));
            }
            else
            {
                caller.Admin(true);
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Duty.Instance.Translate("on_duty_message", caller.CharacterName), UnturnedChat.GetColorFromName(Duty.Instance.Configuration.Instance.MessageColor, Color.red));
                
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList {
                    {"admin_login_message", "{0} is now on duty"},
                    {"admin_logoff_message", "{0} is now off duty"},
                    {"on_duty_message", "{0} is now on duty"},
                    {"off_duty_message", "{0} is now off duty"},
                };
                    
            }
        }
        void PlayerConnected(UnturnedPlayer player)
        {
            if (player.IsAdmin)
            {
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Duty.Instance.Translate("admin_login_message", player.CharacterName), UnturnedChat.GetColorFromName(Duty.Instance.Configuration.Instance.MessageColor, Color.red));
            }
        }
        void PlayerDisconnected(UnturnedPlayer player)
        {
            if (player.IsAdmin)
            {
                if (Configuration.Instance.RemoveAdminOnLogout)
                {
                    player.Admin(false);
                    player.Features.GodMode = false;
                    player.Features.VanishMode = false;
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Duty.Instance.Translate("admin_logout_message", player.CharacterName), UnturnedChat.GetColorFromName(Duty.Instance.Configuration.Instance.MessageColor, Color.red));
                }
            }
        }
    }
}

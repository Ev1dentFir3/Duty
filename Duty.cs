using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned;
using SDG;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFG
{
    public class Duty : RocketPlugin<DutyConfiguration>
    {

        public static Duty Instance;
        protected override void Load()
        {
            Duty.Instance = this;
            RocketServerEvents.OnPlayerConnected += PlayerConnected;
            RocketServerEvents.OnPlayerDisconnected += PlayerDisconnected;
        }
        public void duty(RocketPlayer caller)
        {
            if (caller.IsAdmin)
            {
                caller.Admin(false);
                caller.Features.GodMode = false;
                caller.Features.VanishMode = false;
                if (Configuration.EnableServerAnnouncer) RocketChat.Say(Duty.Instance.Translate("off_duty_message", caller.CharacterName), RocketChat.GetColorFromName(Duty.Instance.Configuration.MessageColor, Color.red));
            }
            else
            {
                caller.Admin(true);
                if (Configuration.EnableServerAnnouncer) RocketChat.Say(Duty.Instance.Translate("on_duty_message", caller.CharacterName), RocketChat.GetColorFromName(Duty.Instance.Configuration.MessageColor, Color.red));
                
            }
        }

        public override Dictionary<string, string> DefaultTranslations
        {
            get
            {
                return new Dictionary<string, string> {
                    {"admin_login_message", "{0} is now on duty"},
                    {"admin_logoff_message", "{0} is now off duty"},
                    {"on_duty_message", "{0} is now on duty"},
                    {"off_duty_message", "{0} is now off duty"},
                };
                    
            }
        }
        void PlayerConnected(RocketPlayer player)
        {
            if (player.IsAdmin)
            {
                if (Configuration.EnableServerAnnouncer) RocketChat.Say(Duty.Instance.Translate("admin_login_message", player.CharacterName), RocketChat.GetColorFromName(Duty.Instance.Configuration.MessageColor, Color.red));
            }
        }
        void PlayerDisconnected(RocketPlayer player)
        {
            if (player.IsAdmin)
            {
                if (Configuration.RemoveAdminOnLogout)
                {
                    player.Admin(false);
                    player.Features.GodMode = false;
                    player.Features.VanishMode = false;
                    if (Configuration.EnableServerAnnouncer) RocketChat.Say(Duty.Instance.Translate("admin_logout_message", player.CharacterName), RocketChat.GetColorFromName(Duty.Instance.Configuration.MessageColor, Color.red));
                }
            }
        }
    }
}

using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using Rocket.Unturned;
using Rocket.Core.Plugins;
using Rocket.API.Collections;
using UnityEngine;
using Rocket.API;

namespace EFG.Duty
{
    public class Duty : RocketPlugin<DutyConfiguration>
    {
        public static Duty Instance;

        protected override void Load()
        {
            Instance = this;

            Rocket.Core.Logging.Logger.LogWarning("Loading event \"Player Connected\"...");
            U.Events.OnPlayerConnected += PlayerConnected;
            Rocket.Core.Logging.Logger.LogWarning("Loading event \"Player Disconnected\"...");
            U.Events.OnPlayerDisconnected += PlayerDisconnected;

            Rocket.Core.Logging.Logger.LogWarning("");
            Rocket.Core.Logging.Logger.LogWarning("Duty has been successfully loaded!");
        }
        
        protected override void Unload()
        {
            Instance = null;

            Rocket.Core.Logging.Logger.LogWarning("Unloading on player connect event...");
            U.Events.OnPlayerConnected -= PlayerConnected;
            Rocket.Core.Logging.Logger.LogWarning("Unloading on player disconnect event...");
            U.Events.OnPlayerConnected -= PlayerDisconnected;

            Rocket.Core.Logging.Logger.LogWarning("");
            Rocket.Core.Logging.Logger.LogWarning("Duty has been unloaded!");
        }

        public void duty(UnturnedPlayer caller)
        {
            if (caller.IsAdmin)
            {
                caller.Admin(false);
                caller.Features.GodMode = false;
                caller.Features.VanishMode = false;
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("off_duty_message", caller.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
            }
            else
            {
                caller.Admin(true);
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("on_duty_message", caller.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                
            }
        }

        public void cduty(UnturnedPlayer cplayer, IRocketPlayer caller)
        {
            if (cplayer != null)
            {
                if (cplayer.IsAdmin)
                {
                    Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Checking Duty");
                    if (Configuration.Instance.AllowDutyCheck)
                    {
                        Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Cplayer Admin Found.");
                        if (caller is ConsolePlayer)
                        {
                            UnturnedChat.Say(Instance.Translate("check_on_duty_message", cplayer, "Console"), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                        }
                        else if (caller is UnturnedPlayer)
                        {
                            UnturnedChat.Say(Instance.Translate("check_on_duty_message", cplayer, caller), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                        }
                    }
                    else if (Configuration.Instance.AllowDutyCheck == false)
                    {
                        Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Unable To Check Duty. Configuration Is Set To Be Disabled.");
                        if (caller is UnturnedPlayer)
                        {
                            UnturnedChat.Say(caller, "Unable To Check Duty. Configuration Is Set To Be Disabled.");
                        }
                    }
                }
                else if (cplayer.IsAdmin == false)
                {
                    if (Configuration.Instance.AllowDutyCheck)
                    {
                        Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Cplayer Admin Not Found");
                        if (caller is ConsolePlayer)
                        {
                            UnturnedChat.Say(Instance.Translate("check_off_duty_message", cplayer, "Console"), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                        }
                        else if (caller is UnturnedPlayer)
                        {
                            UnturnedChat.Say(Instance.Translate("check_off_duty_message", cplayer, caller), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                        }
                    }
                    else if (Configuration.Instance.AllowDutyCheck == false)
                    {
                        Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Unable To Check Duty. Configuration Is Set To Be Disabled.");
                        if (caller is UnturnedPlayer)
                        {
                            UnturnedChat.Say(caller, "Unable To Check Duty. Configuration Is Set To Be Disabled.");
                        }
                    }
                }
            }
            else if (cplayer == null)
            {
                Rocket.Core.Logging.Logger.LogWarning("Duty Debug: Player is not online or his name is invalid.");
                if (caller is UnturnedPlayer)
                {
                    UnturnedChat.Say(caller, "Player is not online or his name is invalid.");
                }
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList {
                    {"admin_login_message", "{0} has logged on and is now on duty."},
                    {"admin_logoff_message", "{0} has logged off and is now off duty."},
                    {"on_duty_message", "{0} is now on duty."},
                    {"off_duty_message", "{0} is now off duty."},
                    {"check_on_duty_message", "{0} has confirmed that {1} is on duty."},
                    {"check_off_duty_message", "{0} has confirmed that {1} is not on duty."},
                };
                    
            }
        }
        void PlayerConnected(UnturnedPlayer player)
        {
            if (player.IsAdmin)
            {
                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_login_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
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
                    if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_logout_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
                }

                if (Configuration.Instance.EnableServerAnnouncer) UnturnedChat.Say(Instance.Translate("admin_logout_message", player.CharacterName), UnturnedChat.GetColorFromName(Instance.Configuration.Instance.MessageColor, Color.red));
            }
        }
    }
}

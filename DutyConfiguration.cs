using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFG.Duty
{
    public class DutyConfiguration : IRocketPluginConfiguration
    {
        public bool EnableServerAnnouncer;
        public bool RemoveAdminOnLogout;
        public string MessageColor;

        public void LoadDefaults()
        {
            EnableServerAnnouncer = true;
            RemoveAdminOnLogout = true;
            MessageColor = "red";
        }
    }
}

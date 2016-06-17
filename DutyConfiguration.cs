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
        public bool AllowDutyCheck;
        public string MessageColor;

        public void LoadDefaults()
        {
            EnableServerAnnouncer = true;
            RemoveAdminOnLogout = true;
            AllowDutyCheck = true;
            MessageColor = "red";
        }
    }
}

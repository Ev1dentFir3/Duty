using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFG
{
    public class DutyConfiguration : IRocketPluginConfiguration
    {
        public bool EnableServerAnnouncer;
        public bool RemoveAdminOnLogout;
        public string MessageColor;

        public IRocketPluginConfiguration DefaultConfiguration
        {
            get
            {
                return new DutyConfiguration{
                    EnableServerAnnouncer = true,
                    RemoveAdminOnLogout = true,
                    MessageColor = "red"
                };
            }
        }
    }
}

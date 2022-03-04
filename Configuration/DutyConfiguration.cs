using System.Collections.Generic;
using JetBrains.Annotations;
using Rocket.API;

namespace EFG.Duty.Configuration;

[UsedImplicitly]
public class DutyConfiguration : IRocketPluginConfiguration
{
    public bool EnableServerAnnouncer { get; set; }
    public bool RemoveDutyOnLogout { get; set; }
    public string MessageColor { get; set; }
    public string SuperAdminPermission { get; set; }
    public List<DutyPermissionGroup> DutyPermissionGroups { get; set; }

    public DutyConfiguration()
    {
        EnableServerAnnouncer = true;
        RemoveDutyOnLogout = true;
        MessageColor = "red";
        SuperAdminPermission = "duty.superadmin";
        DutyPermissionGroups = new List<DutyPermissionGroup>();
    }

    public void LoadDefaults()
    {
        EnableServerAnnouncer = true;
        RemoveDutyOnLogout = true;
        MessageColor = "red";
        SuperAdminPermission = "duty.superadmin";
        DutyPermissionGroups = new List<DutyPermissionGroup>
        {
            new("Administrator", "duty.admin"),
            new("Moderator", "duty.mod"),
            new("Helper", "duty.helper")
        };
    }
}
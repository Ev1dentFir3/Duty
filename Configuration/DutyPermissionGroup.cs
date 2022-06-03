using System.Xml.Serialization;

namespace EFG.Duty.Configuration;

public sealed class DutyPermissionGroup
{
    [XmlAttribute] public string GroupId { get; set; }

    [XmlText] public string PermissionRequired { get; set; }

    public DutyPermissionGroup()
    {
        GroupId = "Administrator";
        PermissionRequired = "admin";
    }

    public DutyPermissionGroup(string groupId, string permissionRequired)
    {
        GroupId = groupId;
        PermissionRequired = permissionRequired;
    }
}
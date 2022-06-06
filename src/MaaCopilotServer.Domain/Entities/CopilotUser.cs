// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

/// <summary>
/// 用户
/// </summary>
public class CopilotUser : EditableEntity
{
    public CopilotUser(string email, string password, string userName, UserRole userRole)
    {
        Email = email;
        Password = password;
        UserName = userName;
        UserRole = userRole;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private CopilotUser() { }
#pragma warning restore CS8618

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; private set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 权限组
    /// </summary>
    public UserRole UserRole { get; set; }
}

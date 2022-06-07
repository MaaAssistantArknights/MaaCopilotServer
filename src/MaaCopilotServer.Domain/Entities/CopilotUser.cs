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
    public string Password { get; private set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; private set; }
    /// <summary>
    /// 权限组
    /// </summary>
    public UserRole UserRole { get; private set; }

    public void UpdatePassword(string newPassword)
    {
        Password = newPassword;
        UpdateAt = DateTimeOffset.UtcNow;
    }

    public void UpdateUserInfo(string? email = null, string? userName = null, UserRole? userRole = null)
    {
        if (string.IsNullOrEmpty(email) &&
            string.IsNullOrEmpty(userName) &&
            userRole is null)
        {
            return;
        }
        if (string.IsNullOrEmpty(email) is false)
            Email = email;
        if (string.IsNullOrEmpty(userName) is false)
            UserName = userName;
        if (userRole is not null)
            UserRole = userRole.Value;
        UpdateAt = DateTimeOffset.UtcNow;
    }
}

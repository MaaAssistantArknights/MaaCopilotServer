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
    /// <summary>
    /// The constructor of <see cref="CopilotUser"/>.
    /// </summary>
    /// <param name="email">The user email.</param>
    /// <param name="password">The password.</param>
    /// <param name="userName">The username.</param>
    /// <param name="userRole">The role of the user.</param>
    /// <param name="createBy">The creator of the user.</param>
    public CopilotUser(string email, string password, string userName, UserRole userRole, Guid? createBy)
    {
        Email = email;
        Password = password;
        UserName = userName;
        UserRole = userRole;
        if (createBy is null)
        {
            CreateBy = this.EntityId;
            UpdateBy = this.EntityId;
        }
        else
        {
            CreateBy = createBy.Value;
            UpdateBy = createBy.Value;
        }
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
    /// <summary>
    /// 用户激活
    /// </summary>
    public bool UserActivated { get; private set; } = false;
    /// <summary>
    /// 收藏夹
    /// </summary>
    public List<CopilotUserFavorite> UserFavorites { get; set; } = new();

    public void ActivateUser(Guid @operator)
    {
        UpdateAt = DateTimeOffset.UtcNow;
        UpdateBy = @operator;
        UserActivated = true;
    }

    /// <summary>
    /// Updates the password.
    /// </summary>
    /// <param name="operator">The user who does the operation.</param>
    /// <param name="newPassword">The new password.</param>
    public void UpdatePassword(Guid @operator, string newPassword)
    {
        Password = newPassword;
        UpdateAt = DateTimeOffset.UtcNow;
        UpdateBy = @operator;
    }

    /// <summary>
    /// Updates user info.
    /// </summary>
    /// <param name="operator">The user who does the operation.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="userName">The username.</param>
    /// <param name="userRole">The role of the user.</param>
    public void UpdateUserInfo(Guid @operator, string? email = null, string? userName = null, UserRole? userRole = null)
    {
        if (string.IsNullOrEmpty(email) &&
            string.IsNullOrEmpty(userName) &&
            userRole is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(email) is false)
        {
            Email = email;
            UserActivated = false;
        }
        if (string.IsNullOrEmpty(userName) is false)
            UserName = userName;
        if (userRole is not null)
            UserRole = userRole.Value;
        UpdateAt = DateTimeOffset.UtcNow;
        UpdateBy = @operator;
    }
}

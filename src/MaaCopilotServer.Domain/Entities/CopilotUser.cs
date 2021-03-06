// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     Maa Copilot user entity.
/// </summary>
[ExcludeFromCodeCoverage]
public class CopilotUser : EditableEntity
{
    /// <summary>
    ///     The constructor of <see cref="CopilotUser" />.
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
            CreateBy = EntityId;
            UpdateBy = EntityId;
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

    // WARNING:
    // YOU SHOULD NEVER EXPOSE SETTER TO PUBLIC SCOPE.
    // YOU SHOULD NEVER EXPOSE DEFAULT CONSTRUCTOR TO PUBLIC SCOPE.
    // YOU SHOULD ONLY USE A DOMAIN METHOD TO UPDATE PROPERTIES.
    // YOU SHOULD CALL DELETE METHOD BEFORE YOU ACTUALLY DELETE IT.

    /// <summary>
    ///     The email of the user.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    ///     The password of the user.
    /// </summary>
    public string Password { get; private set; }

    /// <summary>
    ///     The username of the user.
    /// </summary>
    public string UserName { get; private set; }

    /// <summary>
    ///     The role of the user.
    /// </summary>
    public UserRole UserRole { get; private set; }

    /// <summary>
    ///     Whether the user is active.
    /// </summary>
    public bool UserActivated { get; private set; }

    /// <summary>
    ///     Activate the user.
    /// </summary>
    /// <param name="operator">The operator.</param>
    public void ActivateUser(Guid @operator)
    {
        UpdateAt = DateTimeOffset.UtcNow;
        UpdateBy = @operator;
        UserActivated = true;
    }

    /// <summary>
    ///     Updates the password.
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
    ///     Updates user info.
    /// </summary>
    /// <param name="operator">The user who does the operation.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="userName">The username.</param>
    /// <param name="userRole">The role of the user.</param>
    /// <param name="force">Force change info.</param>
    public void UpdateUserInfo(Guid @operator, string? email = null, string? userName = null, UserRole? userRole = null, bool force = false)
    {
        // If everything is empty or null, nothing changes, just return.
        if (string.IsNullOrEmpty(email) &&
            string.IsNullOrEmpty(userName) &&
            userRole is null)
        {
            return;
        }

        // If email is not null or empty, set it.
        if (string.IsNullOrEmpty(email) is false)
        {
            Email = email;
            // If this is not a force change, change the activation status.
            if (force is false)
            {
                UserActivated = false;
            }
        }

        // If the username is not null or empty, set it.
        if (string.IsNullOrEmpty(userName) is false)
        {
            UserName = userName;
        }

        // If the role is not null, set it.
        if (userRole is not null)
        {
            UserRole = userRole.Value;
        }

        // Set update time and operator
        UpdateAt = DateTimeOffset.UtcNow;
        UpdateBy = @operator;
    }
}

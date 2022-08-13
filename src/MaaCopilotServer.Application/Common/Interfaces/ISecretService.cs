// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
///     The service for processing passwords and tokens.
/// </summary>
public interface ISecretService
{
    /// <summary>
    ///     Generates a JWT token.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>A JWT token string, and the expiration time.</returns>
    (string, DateTimeOffset) GenerateJwtToken(Guid id);

    /// <summary>
    ///     Generate a Refresh Token.
    /// </summary>
    /// <returns>A refresh token string, and the expiration time.</returns>
    (string, DateTimeOffset) GenerateRefreshToken();

    /// <summary>
    ///     Generates a token for validation.
    /// </summary>
    /// <param name="resourceId">The target resource id that the token will be used for.</param>
    /// <param name="validTimeSpan">The validation time of the token.</param>
    /// <returns>A token string, and the expiration time.</returns>
    (string, DateTimeOffset) GenerateToken(Guid resourceId, TimeSpan validTimeSpan);

    /// <summary>
    ///     Get user id from jwt access token.
    /// </summary>
    /// <param name="accessToken">JWT access token.</param>
    /// <returns></returns>
    Guid? GetUserIdFromAccessToken(string accessToken);

    /// <summary>
    ///     Hashes a password with the salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    string HashPassword(string password);

    /// <summary>
    ///     Verifies a password to check if it matches the hash.
    /// </summary>
    /// <param name="hash">The hash of the password.</param>
    /// <param name="password">The password.</param>
    /// <returns><c>true</c> if the password matches the hash, <c>false</c> otherwise.</returns>
    bool VerifyPassword(string hash, string password);
}

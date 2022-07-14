// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Interfaces;

namespace MaaCopilotServer.Application.Test.TestExtensions;

/// <summary>
/// Test extensions for <see cref="ICopilotOperationService"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class CopilotOperationServiceTestExtension
{
    /// <summary>
    /// Setups <see cref="ICopilotOperationService.DecodeId(string)"/> and <see cref="ICopilotOperationService.EncodeId(long)"/>.
    /// </summary>
    /// <param name="mock">The mock object.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mock"/> is <c>null</c>.</exception>
    public static void SetupDecodeAndEncodeId(this Mock<ICopilotOperationService> mock)
    {
        if (mock == null)
        {
            throw new ArgumentNullException(nameof(mock));
        }

        mock.Setup(x => x.EncodeId(It.IsAny<long>())).Returns<long>(EntityIdHelper.EncodeId);
        mock.Setup(x => x.DecodeId(It.IsAny<string>())).Returns<string>(EntityIdHelper.DecodeId);
    }
}

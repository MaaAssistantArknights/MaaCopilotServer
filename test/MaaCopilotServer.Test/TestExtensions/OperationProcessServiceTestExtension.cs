// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using Moq;

namespace MaaCopilotServer.Application.Test.TestExtensions;

/// <summary>
/// Test extension for <see cref="IOperationProcessService/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class OperationProcessServiceTestExtension
{
    /// <summary>
    /// Setups <see cref="IOperationProcessService.Validate(string?)"/>.
    /// </summary>
    /// <param name="mock">The mock object.</param>
    /// <param name="operation">The operation.</param>
    /// <param name="result">The validation result.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mock"/> is <c>null</c>.</exception>
    public static void SetupValidate(this Mock<IOperationProcessService> mock, string? operation, OperationValidationResult result)
    {
        if (mock == null)
        {
            throw new ArgumentNullException(nameof(mock));
        }

        mock.Setup(x => x.Validate(operation)).ReturnsAsync(result);
    }
}

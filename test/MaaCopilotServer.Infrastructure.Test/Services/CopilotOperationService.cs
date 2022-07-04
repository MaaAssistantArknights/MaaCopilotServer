// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Services;
using MaaCopilotServer.Resources;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Infrastructure.Test.Services;

/// <summary>
/// Tests <see cref="CopilotOperationService"/>.
/// </summary>
[TestClass]
public class CopilotIdServiceTest
{
    private readonly CopilotOperationService _copilotOperationService
        = new(Options.Create(new CopilotOperationOption
            {
                DislikeMultiplier = 2,
                ViewMultiplier = 1,
                LikeMultiplier = 10,
                InitialHotScore = 100,
                RequireDetails = default,
                RequireTitle = default
            }), new DomainString());

    /// <summary>
    /// Tests <see cref="CopilotOperationService.EncodeId(long)"/>.
    /// </summary>
    [TestMethod]
    public void TestEncodeId()
    {
        _copilotOperationService.EncodeId(42).Should().Be("10042");
    }

    /// <summary>
    /// Tests <see cref="CopilotOperationService.DecodeId(string)"/> with invalid ID.
    /// </summary>
    [TestMethod]
    public void TestDecodeId_Invalid()
    {
        _copilotOperationService.DecodeId("invalid").Should().BeNull();
    }

    /// <summary>
    /// Tests <see cref="CopilotOperationService.DecodeId(string)"/> with ID out of range.
    /// </summary>
    [TestMethod]
    public void TestDecodeId_OutOfRange()
    {
        _copilotOperationService.DecodeId("9999").Should().BeNull();
    }

    /// <summary>
    /// Tests <see cref="CopilotOperationService.DecodeId(string)"/>.
    /// </summary>
    [TestMethod]
    public void TestDecodeId()
    {
        _copilotOperationService.DecodeId("10042").Should().Be(42);
    }

    /// <summary>
    /// Tests <see cref="CopilotOperationService.CalculateHotScore(CopilotOperation)"/>.
    /// </summary>
    [TestMethod]
    public void TestCalculateHotScore_FromEntity()
    {
        var entity = new CopilotOperation(
            10001,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            new CopilotUser(string.Empty, string.Empty, string.Empty, UserRole.User, Guid.Empty),
            Guid.Empty,
            Array.Empty<string>(),
            Array.Empty<string>());
        entity.AddViewCount();
        entity.AddLike(Guid.Empty);
        entity.AddDislike(Guid.Empty);
        _copilotOperationService.CalculateHotScore(entity).Should().Be(100 + 2 + 1 + 10);
    }

    /// <summary>
    /// Tests <see cref="CopilotOperationService.CalculateHotScore(int, int, int)"/>.
    /// </summary>
    [TestMethod]
    public void TestCalculateHotScore_FromParam()
    {
        _copilotOperationService.CalculateHotScore(1, 1, 1).Should().Be(100 + 2 + 1 + 10);
    }
}

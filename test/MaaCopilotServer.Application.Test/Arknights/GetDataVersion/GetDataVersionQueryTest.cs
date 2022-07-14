// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Arknights.GetDataVersion;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.Arknights.GetDataVersion;

/// <summary>
/// Tests <see cref="GetDataVersionQueryHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class GetDataVersionQueryTest
{
    /// <summary>
    /// Tests <see cref="GetDataVersionQueryHandler.Handle(GetDataVersionQuery, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.PersistStorage.Add(new(SystemConstants.ARK_ASSET_VERSION_LEVEL, $"test_{SystemConstants.ARK_ASSET_VERSION_LEVEL}"));
            db.PersistStorage.Add(new(SystemConstants.ARK_ASSET_VERSION_CN, $"test_{SystemConstants.ARK_ASSET_VERSION_CN}"));
            db.PersistStorage.Add(new(SystemConstants.ARK_ASSET_VERSION_EN, $"test_{SystemConstants.ARK_ASSET_VERSION_EN}"));
            db.PersistStorage.Add(new(SystemConstants.ARK_ASSET_VERSION_JP, $"test_{SystemConstants.ARK_ASSET_VERSION_JP}"));
            db.PersistStorage.Add(new(SystemConstants.ARK_ASSET_VERSION_KO, $"test_{SystemConstants.ARK_ASSET_VERSION_KO}"));
        });

        var result = test.TestGetDataVersion(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var dto = (GetDataVersionQueryDto)result.Response.Data!;
        dto.Should().NotBeNull();
        dto.LevelVersion.Should().Be($"test_{SystemConstants.ARK_ASSET_VERSION_LEVEL}");
        dto.CnVersion.Should().Be($"test_{SystemConstants.ARK_ASSET_VERSION_CN}");
        dto.EnVersion.Should().Be($"test_{SystemConstants.ARK_ASSET_VERSION_EN}");
        dto.JpVersion.Should().Be($"test_{SystemConstants.ARK_ASSET_VERSION_JP}");
        dto.KoVersion.Should().Be($"test_{SystemConstants.ARK_ASSET_VERSION_KO}");
    }
}

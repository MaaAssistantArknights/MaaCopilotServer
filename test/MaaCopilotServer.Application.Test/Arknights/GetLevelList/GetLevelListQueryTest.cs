// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentEmail.Core;
using MaaCopilotServer.Application.Arknights.GetLevelList;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.Arknights.GetLevelList;

/// <summary>
/// Tests <see cref="GetLevelListQueryHandler"/>
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class GetLevelListQueryTest
{
    /// <summary>
    /// Tests <see cref="GetLevelListQueryHandler.Handle(GetLevelListQuery, CancellationToken)"/>.
    /// </summary>
    /// <param name="requestServer">The server value in the request.</param>
    /// <param name="server">The server suffix that should be attached in the test response.</param>
    [DataTestMethod]
    [DataRow("zh_cn", "cn")]
    [DataRow("cn", "cn")]
    [DataRow("zh_tw", "tw")]
    [DataRow("tw", "tw")]
    [DataRow("en_us", "en")]
    [DataRow("en", "en")]
    [DataRow("ja_jp", "jp")]
    [DataRow("ja", "jp")]
    [DataRow("ko_kr", "ko")]
    [DataRow("ko", "ko")]
    [DataRow("", "cn")]
    public void TestHandle(string requestServer, string server)
    {
        var test = new HandlerTest();
        test.DbContext.Setup(db => db.ArkLevelData.Add(new(new())));

        var result = test.TestGetLevelList(new()
        {
            Language = requestServer,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var dto = (IEnumerable<GetLevelListDto>)result.Response.Data!;

        dto.ForEach(i =>
        {
            i.Name.Should().Be($"{server.ToUpperInvariant()}");
            i.CatOne.Should().Be($"CatOne{server.ToUpperInvariant()}");
            i.CatTwo.Should().Be($"CatTwo{server.ToUpperInvariant()}");
            i.CatThree.Should().Be($"CatThree{server.ToUpperInvariant()}");
        });
    }
}

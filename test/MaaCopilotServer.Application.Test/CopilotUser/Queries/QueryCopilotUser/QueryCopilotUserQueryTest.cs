// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Queries.QueryCopilotUser;

/// <summary>
/// Tests <see cref="QueryCopilotUserQueryHandler"/>.
/// </summary>
[TestClass]
public class QueryCopilotUserQueryHandlerTest
{
    /// <summary>
    /// Initializes database.
    /// </summary>
    /// <returns>The <see cref="HandlerTest"/> instance.</returns>
    public static HandlerTest InitializeDatabase()
    {
        return new HandlerTest()
            .SetupDatabase(db =>
            {
                for (var i = 0; i < 10; i++)
                {
                    db.CopilotUsers.Add(
                        new Domain.Entities.CopilotUser(
                            string.Empty, string.Empty, $"user{i}", Domain.Enums.UserRole.User, null));
                }
            });
    }

    /// <summary>
    /// Tests <see cref="QueryCopilotUserQueryHandler.Handle(QueryCopilotUserQuery, CancellationToken)"/>
    /// with default request.
    /// </summary>
    [TestMethod]
    public void TestHandle_Default()
    {
        var result = InitializeDatabase()
            .TestQueryCopilotUser(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var dtos = (PaginationResult<QueryCopilotUserDto>)result.Response.Data!;
        dtos.Page.Should().Be(1);
        dtos.HasNext.Should().BeFalse();
        dtos.Total.Should().Be(10);
        dtos.Data.Should().HaveCount(10);
    }

    /// <summary>
    /// Tests <see cref="QueryCopilotUserQueryHandler.Handle(QueryCopilotUserQuery, CancellationToken)"/>
    /// with limit and page.
    /// </summary>
    [TestMethod]
    public void TestHandle_LimitAndPage()
    {
        var result = InitializeDatabase()
            .TestQueryCopilotUser(new()
            {
                Page = 2,
                Limit = 1,
            });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var dtos = (PaginationResult<QueryCopilotUserDto>)result.Response.Data!;
        dtos.Page.Should().Be(2);
        dtos.HasNext.Should().BeTrue();
        dtos.Total.Should().Be(10);
        dtos.Data.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests <see cref="QueryCopilotUserQueryHandler.Handle(QueryCopilotUserQuery, CancellationToken)"/>
    /// with username.
    /// </summary>
    [TestMethod]
    public void TestHandle_Username()
    {
        var result = InitializeDatabase()
            .TestQueryCopilotUser(new()
            {
                UserName = "user2",
            });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var dtos = (PaginationResult<QueryCopilotUserDto>)result.Response.Data!;
        dtos.Page.Should().Be(1);
        dtos.HasNext.Should().BeFalse();
        dtos.Total.Should().Be(1);
        dtos.Data.Should().HaveCount(1);
        dtos.Data[0].UserName.Should().Be("user2");
    }
}

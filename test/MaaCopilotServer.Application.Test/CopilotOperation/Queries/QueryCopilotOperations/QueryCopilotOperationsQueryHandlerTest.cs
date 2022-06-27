// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using MaaCopilotServer.Infrastructure.Services;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
/// Tests for <see cref="QueryCopilotOperationsQueryHandler"/>.
/// </summary>
[TestClass]
public class QueryCopilotOperationsQueryHandlerTest
{
    /// <summary>
    /// The index of entity that has the highest views.
    /// </summary>
    private static readonly int s_highestViewId = 9;

    /// <summary>
    /// The index of entity that has the highest rate.
    /// </summary>
    private static readonly int s_highestRateId = 8;

    /// <summary>
    ///     The API error message.
    /// </summary>
    private readonly Resources.ApiErrorMessage _apiErrorMessage = new();

    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private readonly ICopilotIdService _copilotIdService = new CopilotIdService();

    /// <summary>
    ///     The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext = new TestDbContext();

    /// <summary>
    /// Initializes database with initial test data.s
    /// </summary>
    /// <returns>A list of users.</returns>
    public List<Domain.Entities.CopilotUser> InitializeDatabase()
    {
        List<Domain.Entities.CopilotUser> users = new()
        {
            new Domain.Entities.CopilotUser(string.Empty, string.Empty, "user0", Domain.Enums.UserRole.User, null),
            new Domain.Entities.CopilotUser(string.Empty, string.Empty, "user1", Domain.Enums.UserRole.User, null),
        };
        _dbContext.CopilotUsers.AddRange(users);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        List<Domain.Entities.CopilotOperation> data = new();
        for (var i = 0; i < 5; i++)
        {
            data.Add(new Domain.Entities.CopilotOperation(i, $"content{i}", $"stage{i}", string.Empty, string.Empty, string.Empty, users[0], Guid.Empty, new List<string>(), new List<string>()));
        }
        for (var i = 5; i < 10; i++)
        {
            data.Add(new Domain.Entities.CopilotOperation(i, $"content{i}", $"stage{i}", string.Empty, string.Empty, string.Empty, users[1], Guid.Empty, new List<string>(), new List<string>()));
        }

        // Set operation[9] to have one view
        data[s_highestViewId].AddViewCount();

        // Set operation[8] to have one like
        data[s_highestRateId].AddLike(Guid.Empty);

        _dbContext.CopilotOperations.AddRange(data);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        return users;
    }

    /// <summary>
    /// Tests querying all operations.
    /// </summary>
    /// <param name="descending">Whether the result should be in descending order.</param>
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void TestHandle_All(bool descending)
    {
        var users = InitializeDatabase();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(users[0].EntityId);
        currentUserService.Setup(x => x.GetUser().Result).Returns(users[0]);
        var request = new QueryCopilotOperationsQuery()
        {
            Desc = descending ? "desc" : null,
        };

        var handler = new QueryCopilotOperationsQueryHandler(_dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(10);
        responseData.Data.Should().NotBeNull().And.HaveCount(10);
        var data = responseData.Data!;
        for (var i = 0; i < 10; i++)
        {
            data[i].Id.Should().Be(_copilotIdService.EncodeId(descending ? 9 - i : i));
        }
    }

    /// <summary>
    /// Tests querying operations of current user.
    /// </summary>
    [TestMethod]
    public void TestHandle_CurrentUser()
    {
        var users = InitializeDatabase();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(users[0].EntityId);
        currentUserService.Setup(x => x.GetUser().Result).Returns(users[0]);
        var request = new QueryCopilotOperationsQuery()
        {
            UploaderId = "me",
        };

        var handler = new QueryCopilotOperationsQueryHandler(_dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(5);
        responseData.Data.Should().NotBeNull().And.HaveCount(5);
        var data = responseData.Data!;
        for (var i = 0; i < 5; i++)
        {
            data[i].Id.Should().Be(_copilotIdService.EncodeId(i));
            data[i].StageName.Should().Be($"stage{i}");
        }
    }

    /// <summary>
    /// Tests querying with uploader ID.
    /// </summary>
    [TestMethod]
    public void TestHandle_WithUploaderId()
    {
        var users = InitializeDatabase();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(users[0].EntityId);
        currentUserService.Setup(x => x.GetUser().Result).Returns(users[0]);
        var request = new QueryCopilotOperationsQuery()
        {
            UploaderId = users[0].EntityId.ToString(),
        };

        var handler = new QueryCopilotOperationsQueryHandler(_dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(5);
        responseData.Data.Should().NotBeNull().And.HaveCount(5);
        var data = responseData.Data!;
        for (var i = 0; i < 5; i++)
        {
            data[i].Id.Should().Be(_copilotIdService.EncodeId(i));
            data[i].StageName.Should().Be($"stage{i}");
        }
    }

    /// <summary>
    /// Tests querying with invalid current user.
    /// </summary>
    [TestMethod]
    public void TestHandle_InvalidCurrentUser()
    {
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns((Guid?)null);
        currentUserService.Setup(x => x.GetUser().Result).Returns((Domain.Entities.CopilotUser?)null);
        var request = new QueryCopilotOperationsQuery()
        {
            UploaderId = "me",
        };

        var handler = new QueryCopilotOperationsQueryHandler(_dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests querying with stage name.
    /// </summary>
    [TestMethod]
    public void TestHandle_WithStageName()
    {
        var users = InitializeDatabase();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(users[0].EntityId);
        currentUserService.Setup(x => x.GetUser().Result).Returns(users[0]);
        var request = new QueryCopilotOperationsQuery()
        {
            StageName = "stage0",
        };

        var handler = new QueryCopilotOperationsQueryHandler(_dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(1);
        responseData.Data.Should().NotBeNull().And.HaveCount(1);
        var data = responseData.Data!;
        data[0].Id.Should().Be(_copilotIdService.EncodeId(0));
        data[0].StageName.Should().Be("stage0");
    }

    /// <summary>
    /// Tests querying with content.
    /// </summary>
    [TestMethod]
    public void TestHandle_WithContent()
    {
        var users = InitializeDatabase();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(users[0].EntityId);
        currentUserService.Setup(x => x.GetUser().Result).Returns(users[0]);
        var request = new QueryCopilotOperationsQuery()
        {
            Content = "content0",
        };

        var handler = new QueryCopilotOperationsQueryHandler(_dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(1);
        responseData.Data.Should().NotBeNull().And.HaveCount(1);
        var data = responseData.Data!;
        data[0].Id.Should().Be(_copilotIdService.EncodeId(0));
    }

    /// <summary>
    /// Tests querying with uploader's username.
    /// </summary>
    [TestMethod]
    public void TestHandle_WithUploader()
    {
        var users = InitializeDatabase();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(users[0].EntityId);
        currentUserService.Setup(x => x.GetUser().Result).Returns(users[0]);
        var request = new QueryCopilotOperationsQuery()
        {
            Uploader = users[0].UserName,
        };

        var handler = new QueryCopilotOperationsQueryHandler(_dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(5);
        responseData.Data.Should().NotBeNull().And.HaveCount(5);
        var data = responseData.Data!;
        for (var i = 0; i < 5; i++)
        {
            data[i].Id.Should().Be(_copilotIdService.EncodeId(i));
            data[i].Uploader.Should().Be(users[0].UserName);
        }
    }

    /// <summary>
    /// Tests querying with order.
    /// </summary>
    /// <param name="descending">Whether the result is in descending order.</param>
    /// <param name="orderBy">The field to order by, which can be <c>"views"</c> or <c>"rating"</c>.</param>
    [DataTestMethod]
    [DataRow("views", false)]
    [DataRow("views", true)]
    [DataRow("rating", false)]
    [DataRow("rating", true)]
    public void TestHandle_OrderBy(string orderBy, bool descending)
    {
        var users = InitializeDatabase();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(users[0].EntityId);
        currentUserService.Setup(x => x.GetUser().Result).Returns(users[0]);
        var request = new QueryCopilotOperationsQuery()
        {
            OrderBy = orderBy,
            Desc = descending ? "desc" : null,
        };

        var handler = new QueryCopilotOperationsQueryHandler(_dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(10);
        responseData.Data.Should().NotBeNull().And.HaveCount(10);
        var data = responseData.Data!;
        var highestId = orderBy == "views" ? s_highestViewId : s_highestRateId;
        if (descending)
        {
            data[0].Id.Should().Be(_copilotIdService.EncodeId(highestId));
        }
        else
        {
            data[9].Id.Should().Be(_copilotIdService.EncodeId(highestId));
        }
    }
}

// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.GameData.Entity;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
/// Tests <see cref="QueryCopilotOperationsQueryHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class QueryCopilotOperationsQueryHandlerTest
{
    /// <summary>
    /// The index of entity that has the highest views.
    /// </summary>
    private const int HighestViewId = 9;

    /// <summary>
    /// The index of entity that has the highest rate.
    /// </summary>
    private const int HighestRateId = 8;

    /// <summary>
    /// Initializes database with initial test data.
    /// </summary>
    /// <param name="test">The <see cref="HandlerTest"/> instance.</param>
    /// <returns>A list of users, and the <see cref="HandlerTest"/> instance.</returns>
    private static (List<Domain.Entities.CopilotUser>, HandlerTest) InitializeDatabase(HandlerTest test)
    {
        List<Domain.Entities.CopilotUser> users = new()
        {
            new Domain.Entities.CopilotUser(string.Empty, string.Empty, "user0", Domain.Enums.UserRole.User, null),
            new Domain.Entities.CopilotUser(string.Empty, string.Empty, "user1", Domain.Enums.UserRole.User, null),
        };

        List<Domain.Entities.CopilotOperation> data = new();
        for (var i = 0; i < 5; i++)
        {
            data.Add(new Domain.Entities.CopilotOperation(i, $"content{i}", string.Empty,
                string.Empty, string.Empty, users[0], Guid.Empty,
                new ArkLevelData(new ArkLevelEntityGlobal($"level{i}")),
                new List<string>(), new List<string>()));
        }
        for (var i = 5; i < 10; i++)
        {
            data.Add(new Domain.Entities.CopilotOperation(i, $"content{i}", string.Empty,
                string.Empty, string.Empty, users[1], Guid.Empty,
                new ArkLevelData(new ArkLevelEntityGlobal($"level{i}")),
                new List<string>(), new List<string>()));
        }

        List<CopilotOperationRating> rating = new();
        for (var i = 0; i < 10; i++)
        {
            rating.Add(new CopilotOperationRating(data[i].EntityId, data[i].Author.EntityId, Domain.Enums.OperationRatingType.Like));
            data[i].AddViewCount();
            data[i].AddLike(Guid.Empty);
        }

        // Set operation[9] to have another two views
        data[HighestViewId].AddViewCount();
        data[HighestRateId].AddViewCount();


        // Set operation[8] to have another two like
        data[HighestRateId].AddLike(Guid.Empty);
        data[HighestRateId].AddLike(Guid.Empty);

        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.AddRange(users);
            db.CopilotOperations.AddRange(data);
            db.CopilotOperationRatings.AddRange(rating);
        });

        return (users, test);
    }

    /// <summary>
    /// Tests querying all operations.
    /// </summary>
    /// <param name="descending">Whether the result should be in descending order.</param>
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void TestHandleAll(bool descending)
    {
        var (users, test) = InitializeDatabase(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var response = test.TestQueryCopilotOperations(new()
        {
            Desc = descending ? "desc" : null,
        }).Response;

        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(10);
        responseData.Data.Should().NotBeNull().And.HaveCount(10);
        var data = responseData.Data;
        for (var i = 0; i < 10; i++)
        {
            data[i].Id.Should().Be(EntityIdHelper.EncodeId(descending ? 9 - i : i));
        }
    }

    /// <summary>
    /// Tests querying all operations.
    /// </summary>
    /// <param name="descending">Whether the result should be in descending order.</param>
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void TestHandlePageAndLimit(bool descending)
    {
        var (users, test) = InitializeDatabase(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var response = test.TestQueryCopilotOperations(new()
        {
            Desc = descending ? "desc" : null,
            Limit = 2,
            Page = 2,
        }).Response;

        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeTrue();
        responseData.Page.Should().Be(2);
        responseData.Total.Should().Be(10);
        responseData.Data.Should().NotBeNull().And.HaveCount(2);
        var data = responseData.Data;
        data[0].Id.Should().Be(EntityIdHelper.EncodeId(descending ? 7 : 2));
        data[1].Id.Should().Be(EntityIdHelper.EncodeId(descending ? 6 : 3));
    }

    /// <summary>
    /// Tests querying operations of current user.
    /// </summary>
    [TestMethod]
    public void TestHandleCurrentUser()
    {
        var (users, test) = InitializeDatabase(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var response = test.TestQueryCopilotOperations(new()
        {
            UploaderId = "me",
        }).Response;

        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(5);
        responseData.Data.Should().NotBeNull().And.HaveCount(5);
        var data = responseData.Data;
        for (var i = 0; i < 5; i++)
        {
            data[i].Id.Should().Be(EntityIdHelper.EncodeId(i));
            data[i].Level.LevelId.Should().Be($"level{i}");
        }
    }

    /// <summary>
    /// Tests querying with uploader ID.
    /// </summary>
    [TestMethod]
    public void TestHandleWithUploaderId()
    {
        var (users, test) = InitializeDatabase(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var response = test.TestQueryCopilotOperations(new()
        {
            UploaderId = users[0].EntityId.ToString(),
        }).Response;

        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(5);
        responseData.Data.Should().NotBeNull().And.HaveCount(5);
        var data = responseData.Data;
        for (var i = 0; i < 5; i++)
        {
            data[i].Id.Should().Be(EntityIdHelper.EncodeId(i));
            data[i].Level.LevelId.Should().Be($"level{i}");
        }
    }

    /// <summary>
    /// Tests querying with invalid current user.
    /// </summary>
    [TestMethod]
    public void TestHandleInvalidCurrentUser()
    {
        var test = new HandlerTest();
        var response = test.TestQueryCopilotOperations(new QueryCopilotOperationsQuery
        {
            UploaderId = "me",
        }).Response;

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests querying with guest.
    /// </summary>
    /// <param name="descending">Whether the result should be in descending order.</param>
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void TestHandleNotLoggedIn(bool descending)
    {
        var (_, test) = InitializeDatabase(new());
        var response = test.TestQueryCopilotOperations(new()
        {
            Desc = descending ? "desc" : null,
        }).Response;

        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(10);
        responseData.Data.Should().NotBeNull().And.HaveCount(10);
        var data = responseData.Data!;
        for (var i = 0; i < 10; i++)
        {
            data[i].Id.Should().Be(EntityIdHelper.EncodeId(descending ? 9 - i : i));
            data[i].RatingType.Should().BeNull();
        }
    }

    /// <summary>
    /// Tests querying with level name.
    /// </summary>
    [TestMethod]
    [DataRow("", "CN")]
    [DataRow("qwerty", "CN")]
    [DataRow("chinese", "CN")]
    [DataRow("english", "EN")]
    [DataRow("japanese", "JP")]
    [DataRow("korean", "KO")]
    public void TestHandleWithLevelName(string language, string resultAppendix)
    {
        var (users, test) = InitializeDatabase(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var response = test.TestQueryCopilotOperations(new()
        {
            LevelName = "level0",
            Server = language
        }).Response;

        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(1);
        responseData.Data.Should().NotBeNull().And.HaveCount(1);
        var data = responseData.Data!;
        data[0].Id.Should().Be(EntityIdHelper.EncodeId(0));
        data[0].Level.LevelId.Should().Be("level0");
        data[0].Level.Name.Should().Be($"level0{resultAppendix}");
    }

    /// <summary>
    /// Tests querying with content.
    /// </summary>
    [TestMethod]
    public void TestHandleWithContent()
    {
        var (users, test) = InitializeDatabase(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var response = test.TestQueryCopilotOperations(new()
        {
            Content = "content0",
        }).Response;

        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(1);
        responseData.Data.Should().NotBeNull().And.HaveCount(1);
        var data = responseData.Data!;
        data[0].Id.Should().Be(EntityIdHelper.EncodeId(0));
    }

    /// <summary>
    /// Tests querying with uploader's username.
    /// </summary>
    [TestMethod]
    public void TestHandleWithUploader()
    {
        var (users, test) = InitializeDatabase(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var response = test.TestQueryCopilotOperations(new()
        {
            Uploader = users[0].UserName,
        }).Response;

        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(5);
        responseData.Data.Should().NotBeNull().And.HaveCount(5);
        var data = responseData.Data!;
        for (var i = 0; i < 5; i++)
        {
            data[i].Id.Should().Be(EntityIdHelper.EncodeId(i));
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
    public void TestHandleOrderBy(string orderBy, bool descending)
    {
        var (users, test) = InitializeDatabase(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var response = test.TestQueryCopilotOperations(new()
        {
            OrderBy = orderBy,
            Desc = descending ? "true" : null,
        }).Response;

        response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(10);
        responseData.Data.Should().NotBeNull().And.HaveCount(10);
        var data = responseData.Data!;
        var highestId = orderBy == "views" ? HighestViewId : HighestRateId;
        if (descending)
        {
            data[0].Id.Should().Be(EntityIdHelper.EncodeId(highestId));
        }
        else
        {
            data[9].Id.Should().Be(EntityIdHelper.EncodeId(highestId));
        }
    }
}

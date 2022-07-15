// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using Elastic.CommonSchema;
using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.GameData.Entity;
using MaaCopilotServer.Test.TestEntities;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
/// Tests <see cref="QueryCopilotOperationsQueryHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class QueryCopilotOperationsQueryTest
{
    /// <summary>
    /// The index of entity that has the highest ID.
    /// </summary>
    private const int HighestId = 9;

    /// <summary>
    /// The index of entity that has the highest views.
    /// </summary>
    private const int HighestViewId = 8;

    /// <summary>
    /// The index of entity that has the highest hot value.
    /// </summary>
    private const int HighestHotId = 7;

    /// <summary>
    /// Initializes test environment with test data.
    /// </summary>
    /// <param name="test">The <see cref="HandlerTest"/> instance.</param>
    /// <returns>A list of users, and the <see cref="HandlerTest"/> instance.</returns>
    private static (List<Domain.Entities.CopilotUser>, HandlerTest) Initialize(HandlerTest test)
    {
        List<Domain.Entities.CopilotUser> users = new()
        {
            new CopilotUserFactory { UserName = "user0" }.Build(),
            new CopilotUserFactory { UserName = "user1" }.Build(),
        };

        List<Domain.Entities.CopilotOperation> data = new();
        for (var i = 0; i < 5; i++)
        {
            data.Add(new CopilotOperationFactory { Id = i, Content = $"content{i}", Author = users[0], ArkLevel = new(new($"level{i}")) }.Build());
        }
        for (var i = 5; i < 10; i++)
        {
            data.Add(new CopilotOperationFactory { Id = i, Content = $"content{i}", Author = users[1], ArkLevel = new(new($"level{i}")) }.Build());
        }

        List<CopilotOperationRating> rating = new();
        for (var i = 0; i < 10; i++)
        {
            rating.Add(new CopilotOperationRating(data[i].EntityId, data[i].Author.EntityId, Domain.Enums.OperationRatingType.Like));
            data[i].AddViewCount();
            data[i].AddLike(Guid.Empty);
        }

        // Set operation[HighestViewId] to have another two views
        data[HighestViewId].AddViewCount();
        data[HighestViewId].AddViewCount();

        // Set operation[HighestHotId] to have highest hot value
        data[HighestHotId].UpdateHotScore(10000);

        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.AddRange(users);
            db.CopilotOperations.AddRange(data);
            db.CopilotOperationRatings.AddRange(rating);
        });

        test.CopilotOperationService.SetupDecodeAndEncodeId();

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
        var (users, test) = Initialize(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var result = test.TestQueryCopilotOperations(new()
        {
            Desc = descending ? "desc" : null,
        });

        result.Response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)result.Response.Data!;
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
        var (users, test) = Initialize(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var result = test.TestQueryCopilotOperations(new()
        {
            Desc = descending ? "desc" : null,
            Limit = 2,
            Page = 2,
        });

        result.Response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)result.Response.Data!;
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
        var (users, test) = Initialize(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var result = test.TestQueryCopilotOperations(new()
        {
            UploaderId = "me",
        });

        result.Response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)result.Response.Data!;
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
        var (users, test) = Initialize(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var result = test.TestQueryCopilotOperations(new()
        {
            UploaderId = users[0].EntityId.ToString(),
        });

        result.Response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)result.Response.Data!;
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
        var result = test.TestQueryCopilotOperations(new QueryCopilotOperationsQuery
        {
            UploaderId = "me",
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
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
        var (_, test) = Initialize(new());
        var result = test.TestQueryCopilotOperations(new()
        {
            Desc = descending ? "desc" : null,
        });

        result.Response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)result.Response.Data!;
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
    [DataRow("chinese", "CN")]
    [DataRow("english", "EN")]
    [DataRow("japanese", "JP")]
    [DataRow("korean", "KO")]
    [DataRow("cn", "CN")]
    [DataRow("en", "EN")]
    [DataRow("ja", "JP")]
    [DataRow("ko", "KO")]
    public void TestHandleWithLevelName(string language, string resultAppendix)
    {
        var (users, test) = Initialize(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var result = test.TestQueryCopilotOperations(new()
        {
            LevelName = "level0",
            LevelCatOne = $"level0CatOne{resultAppendix}",
            LevelCatTwo = $"level0CatTwo{resultAppendix}",
            LevelCatThree = $"level0CatThree{resultAppendix}",
            Server = language
        });

        result.Response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)result.Response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(1);
        responseData.Data.Should().NotBeNull().And.HaveCount(1);
        var data = responseData.Data!;
        data[0].Id.Should().Be(EntityIdHelper.EncodeId(0));
        data[0].Level.LevelId.Should().Be("level0");
        data[0].Level.Name.Should().Be($"level0{resultAppendix}");
        data[0].Level.CatOne.Should().Be($"level0CatOne{resultAppendix}");
        data[0].Level.CatTwo.Should().Be($"level0CatTwo{resultAppendix}");
        data[0].Level.CatThree.Should().Be($"level0CatThree{resultAppendix}");
    }

    [TestMethod]
    [DataRow("??")]
    [DataRow("jp")]
    [DataRow("french")]
    public void TestHandleWithUnknownLanguage(string language)
    {
        var (users, test) = Initialize(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        try
        {
            var _ = test.TestQueryCopilotOperations(new() { LevelName = "level0", Server = language });
        }
        catch (AggregateException e)
        {
            e.InnerException.Should().BeOfType<UnknownServerLanguageException>();
            return;
        }

        Assert.Fail();
    }

    /// <summary>
    /// Tests querying with content.
    /// </summary>
    [TestMethod]
    public void TestHandleWithContent()
    {
        var (users, test) = Initialize(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var result = test.TestQueryCopilotOperations(new()
        {
            Content = "content0",
        });

        result.Response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)result.Response.Data!;
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
        var (users, test) = Initialize(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var result = test.TestQueryCopilotOperations(new()
        {
            Uploader = users[0].UserName,
        });

        result.Response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)result.Response.Data!;
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
    [DataRow("hot", false)]
    [DataRow("hot", true)]
    [DataRow("", false)]
    [DataRow("", true)]
    public void TestHandleOrderBy(string orderBy, bool descending)
    {
        var (users, test) = Initialize(new());
        test.CurrentUserService.SetupGetUserIdentity(users[0].EntityId);
        test.CurrentUserService.SetupGetUser(users[0]);
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var result = test.TestQueryCopilotOperations(new()
        {
            OrderBy = orderBy,
            Desc = descending ? "true" : null,
        });

        result.Response.Data.Should().NotBeNull();
        var responseData = (PaginationResult<QueryCopilotOperationsQueryDto>)result.Response.Data!;
        responseData.HasNext.Should().BeFalse();
        responseData.Page.Should().Be(1);
        responseData.Total.Should().Be(10);
        responseData.Data.Should().NotBeNull().And.HaveCount(10);
        var data = responseData.Data!;
        var highestId = orderBy switch
        {
            "views" => HighestViewId,
            "hot" => HighestHotId,
            _ => HighestId,
        };
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

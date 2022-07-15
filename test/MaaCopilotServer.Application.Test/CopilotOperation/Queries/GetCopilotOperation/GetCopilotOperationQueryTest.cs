// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
/// Tests <see cref="GetCopilotOperationQueryHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class GetCopilotOperationQueryTest
{
    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler.Handle(GetCopilotOperationQuery, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new CopilotUserFactory().Build();
        var entities = new Domain.Entities.CopilotOperation[]
        {
            new CopilotOperationFactory { Id = 1, Author = user }.Build(),
            new CopilotOperationFactory { Id = 2, Author = user }.Build(),
        };

        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.Add(user);
            db.CopilotOperations.AddRange(entities);
        });
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var response = test.TestGetCopilotOperation(new()
        {
            Id = EntityIdHelper.EncodeId(entities[0].Id),
        }).Response;

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Data.Should().NotBeNull();
        var dto = (GetCopilotOperationQueryDto)response.Data!;
        dto.Id.Should().Be(EntityIdHelper.EncodeId(entities[0].Id));
        entities[0].ViewCounts.Should().Be(1);
    }

    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler.Handle(GetCopilotOperationQuery, CancellationToken)"/>
    /// with invalid ID.
    /// </summary>
    [TestMethod]
    public void TestHandleInvalidId()
    {
        var result = new HandlerTest().TestGetCopilotOperation(new()
        {
            Id = null,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler.Handle(GetCopilotOperationQuery, CancellationToken)"/>
    /// with ID that does not exist.
    /// </summary>
    [TestMethod]
    public void TestHandleEntityNotFound()
    {
        var result = new HandlerTest().TestGetCopilotOperation(new()
        {
            Id = EntityIdHelper.EncodeId(0),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}

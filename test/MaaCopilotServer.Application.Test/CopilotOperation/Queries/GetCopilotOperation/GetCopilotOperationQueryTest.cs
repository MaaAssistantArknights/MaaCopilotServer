// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
/// Tests for <see cref="GetCopilotOperationQueryHandler"/>.
/// </summary>
[TestClass]
public class GetCopilotOperationQueryTest
{
    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private readonly ICopilotIdService _copilotIdService = new CopilotIdService();

    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler.Handle(GetCopilotOperationQuery, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, Guid.Empty);
        var entities = new Domain.Entities.CopilotOperation[]
        {
            new Domain.Entities.CopilotOperation(1,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,user,Guid.Empty,new List<string>(),new List<string>()),
            new Domain.Entities.CopilotOperation(2,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,user,Guid.Empty,new List<string>(),new List<string>()),
        };
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db => db.CopilotOperations.AddRange(entities))
            .TestGetCopilotOperation(new()
            {
                Id = _copilotIdService.EncodeId(entities[0].Id),
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Data.Should().NotBeNull();
        var dto = (GetCopilotOperationQueryDto)response.Data!;
        dto.Id.Should().Be(_copilotIdService.EncodeId(entities[0].Id));
        entities[0].ViewCounts.Should().Be(1);
    }

    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler.Handle(GetCopilotOperationQuery, CancellationToken)"/>
    /// with invalid ID.
    /// </summary>
    [TestMethod]
    public void TestHandle_InvalidId()
    {
        var response = new HandlerTest()
            .TestGetCopilotOperation(new()
            {
                Id = null,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler.Handle(GetCopilotOperationQuery, CancellationToken)"/>
    /// with ID that does not exist.
    /// </summary>
    [TestMethod]
    public void TestHandle_EntityNotFound()
    {
        var response = new HandlerTest()
            .TestGetCopilotOperation(new()
            {
                Id = _copilotIdService.EncodeId(0),
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}

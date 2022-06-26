// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using MaaCopilotServer.Infrastructure.Services;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
/// Tests for <see cref="GetCopilotOperationQueryHandler"/>.
/// </summary>
[TestClass]
public class GetCopilotOperationQueryTest
{
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
    /// Tests <see cref="GetCopilotOperationQueryHandler.Handle(GetCopilotOperationQuery, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var entities = new Domain.Entities.CopilotOperation[]
        {
            new Domain.Entities.CopilotOperation(
                1,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Domain.Entities.CopilotUser(
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    Domain.Enums.UserRole.User,
                    Guid.Empty),
                Guid.Empty,
                new List<string>(),
                new List<string>()),
            new Domain.Entities.CopilotOperation(
                2,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Domain.Entities.CopilotUser(
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    Domain.Enums.UserRole.User,
                    Guid.Empty),
                Guid.Empty,
                new List<string>(),
                new List<string>())
        };
        _dbContext.CopilotOperations.AddRange(entities);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var handler = new GetCopilotOperationQueryHandler(_dbContext, _copilotIdService, _apiErrorMessage);
        var response = handler.Handle(new GetCopilotOperationQuery()
        {
            Id = _copilotIdService.EncodeId(entities[0].Id),
        }, new CancellationToken()).GetAwaiter().GetResult();

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
        var handler = new GetCopilotOperationQueryHandler(_dbContext, _copilotIdService, _apiErrorMessage);
        var response = handler.Handle(new GetCopilotOperationQuery()
        {
            Id = null,
        }, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler.Handle(GetCopilotOperationQuery, CancellationToken)"/>
    /// with ID that does not exist.
    /// </summary>
    [TestMethod]
    public void TestHandle_EntityNotFound()
    {
        var handler = new GetCopilotOperationQueryHandler(_dbContext, _copilotIdService, _apiErrorMessage);
        var response = handler.Handle(new GetCopilotOperationQuery()
        {
            Id = _copilotIdService.EncodeId(0),
        }, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}

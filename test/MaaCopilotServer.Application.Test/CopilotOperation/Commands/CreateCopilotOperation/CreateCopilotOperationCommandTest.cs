// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using System.Text.Json.Serialization;

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Infrastructure.Services;
using MaaCopilotServer.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
/// Tests for <see cref="CreateCopilotOperationCommandHandler"/>.
/// </summary>
[TestClass]
public class CreateCopilotOperationCommandTest
{
    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private ICopilotIdService _copilotIdService;

    /// <summary>
    ///     The service for current user.
    /// </summary>
    private ICurrentUserService _currentUserService;

    /// <summary>
    ///     The DB context.
    /// </summary>
    private IMaaCopilotDbContext _dbContext;

    /// <summary>
    ///     The service for user Identity.
    /// </summary>
    private IIdentityService _identityService;

    /// <summary>
    /// The validation error message.
    /// </summary>
    private ValidationErrorMessage _validationErrorMessage;

    /// <summary>
    /// Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _copilotIdService = new CopilotIdService();

        _currentUserService = Substitute.For<ICurrentUserService>();
        _currentUserService.GetUserIdentity().Returns(Guid.Empty);

        _dbContext = new TestDbContext();

        _identityService = Substitute.For<IIdentityService>();
        _identityService.GetUserAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(new Domain.Entities.CopilotUser(
                string.Empty,
                string.Empty,
                string.Empty,
                Domain.Enums.UserRole.User,
                Guid.Empty)));

        _validationErrorMessage = new ValidationErrorMessage();
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_Full()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Doc = new Doc { Title = "test_title", Details = "test_details" },
            Operators = new Operator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 }, new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        await TestHandle(testJsonContent);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without <c>doc</c> field.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_WithoutDoc()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new Operator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 }, new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        await TestHandle(testJsonContent);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without <c>doc</c> field (undefined).
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_WithoutDocUndefined()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new Operator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 }, new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        await TestHandle(testJsonContent, removeNullFields: true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without a <c>name</c> field in <c>operators</c>.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_MissingOperatorName()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new Operator[]
            {
                new() { Skill = 0 }, new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        await TestHandle(testJsonContent, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> with duplicate items in <c>operators</c> field.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_DuplicateOperators()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new Operator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 }, new() { Name = "test_oper_0_name", Skill = 0 }
            }
        };
        await TestHandle(testJsonContent);
    }

    private async Task TestHandle(CreateCopilotOperationContent testJsonContent, bool expectException = false,
        bool removeNullFields = false)
    {
        var testContent = JsonSerializer.Serialize(testJsonContent,
            new JsonSerializerOptions()
            {
                DefaultIgnoreCondition =
                    removeNullFields ? JsonIgnoreCondition.Never : JsonIgnoreCondition.WhenWritingNull
            });

        var handler = new CreateCopilotOperationCommandHandler(_dbContext, _identityService, _currentUserService,
            _copilotIdService, _validationErrorMessage);
        var action = async () =>
            await handler.Handle(new CreateCopilotOperationCommand { Content = testContent }, new CancellationToken());

        if (expectException)
        {
            var response = await action();
            response.StatusCode.Should().NotBe(StatusCodes.Status200OK);
            _dbContext.CopilotOperations.Any().Should().BeFalse();
        }
        else
        {
            var response = await action();
            var id = ((CreateCopilotOperationDto)response.Data).Id;
            _dbContext.CopilotOperations.Any().Should().BeTrue();
            var entity = _dbContext.CopilotOperations.FirstOrDefault();
            entity.Should().NotBeNull();
            entity.Id.Should().Be(_copilotIdService.DecodeId(id));
            entity.Content.Should().Be(testContent);
            entity.StageName.Should().Be(testJsonContent.StageName);
            entity.MinimumRequired.Should().Be(testJsonContent.MinimumRequired);
            entity.Title.Should().Be(testJsonContent.Doc?.Title ?? string.Empty);
            entity.Details.Should().Be(testJsonContent.Doc?.Details ?? string.Empty);

            var entityOperators = (from @operator in testJsonContent.Operators ?? Array.Empty<Operator>()
                                   select $"{@operator.Name}::{@operator.Skill?.ToString() ?? "1"}").Distinct().ToList();
            entity.Operators.Should().BeEquivalentTo(entityOperators);
        }
    }
}

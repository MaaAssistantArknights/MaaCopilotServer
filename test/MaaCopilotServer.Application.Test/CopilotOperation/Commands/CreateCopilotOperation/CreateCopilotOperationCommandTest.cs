// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using System.Text.Json.Serialization;

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
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
        _copilotIdService = Substitute.For<ICopilotIdService>();

        _currentUserService = Substitute.For<ICurrentUserService>();
        _currentUserService.GetUserIdentity().Returns(Guid.Empty);

        _dbContext = Substitute.For<IMaaCopilotDbContext>();
        _dbContext.CopilotOperations.Returns(Substitute.For<DbSet<Domain.Entities.CopilotOperation>>());
        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

        _identityService = Substitute.For<IIdentityService>();
        _identityService.GetUserAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(Substitute.For<Domain.Entities.CopilotUser>()));

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
        Domain.Entities.CopilotOperation? entity = null;
        _dbContext.CopilotOperations.When(x => x.Add(Arg.Any<Domain.Entities.CopilotOperation>())).Do(c =>
        {
            entity = c.Arg<Domain.Entities.CopilotOperation>();
        });
        _copilotIdService.EncodeId(Arg.Any<long>()).Returns("10000");

        var handler = new CreateCopilotOperationCommandHandler(_dbContext, _identityService, _currentUserService,
            _copilotIdService, _validationErrorMessage);
        var action = async () =>
            await handler.Handle(new CreateCopilotOperationCommand { Content = testContent }, new CancellationToken());

        if (expectException)
        {
            var response = await action();
            response.StatusCode.Should().NotBe(StatusCodes.Status200OK);
        }
        else
        {
            var response = await action();
            ((CreateCopilotOperationDto)response.Data).Id.Should().Be("10000");
            entity.Should().NotBeNull();
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

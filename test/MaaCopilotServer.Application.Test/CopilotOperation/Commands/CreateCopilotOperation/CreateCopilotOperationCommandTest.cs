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
    private readonly ICopilotIdService _copilotIdService = new CopilotIdService();

    /// <summary>
    ///     The service for current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService = Mock.Of<ICurrentUserService>(
        x => x.GetUserIdentity() == Guid.Empty);

    /// <summary>
    ///     The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext = new TestDbContext();

    /// <summary>
    ///     The service for user Identity.
    /// </summary>
    private readonly IIdentityService _identityService = Mock.Of<IIdentityService>(
        x => x.GetUserAsync(It.IsAny<Guid>()).Result ==
            new Domain.Entities.CopilotUser(
                string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, Guid.Empty));

    /// <summary>
    /// The validation error message.
    /// </summary>
    private readonly ValidationErrorMessage _validationErrorMessage = new();

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle_Full()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Doc = new Doc { Title = "test_title", Details = "test_details" },
            Operators = new Operator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 },
                new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        TestHandle(testJsonContent);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without <c>doc</c> field.
    /// </summary>
    [TestMethod]
    public void TestHandle_WithoutDoc()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new Operator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 },
                new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        TestHandle(testJsonContent);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without <c>doc</c> field (undefined).
    /// </summary>
    [TestMethod]
    public void TestHandle_WithoutDocUndefined()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new Operator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 },
                new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        TestHandle(testJsonContent, removeNullFields: true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without a <c>name</c> field in <c>operators</c>.
    /// </summary>
    [TestMethod]
    public void TestHandle_MissingOperatorName()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new Operator[]
            {
                new() { Skill = 0 },
                new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        TestHandle(testJsonContent, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> with duplicate items in <c>operators</c> field.
    /// </summary>
    [TestMethod]
    public void TestHandle_DuplicateOperators()
    {
        var testJsonContent = new CreateCopilotOperationContent
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new Operator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 },
                new() { Name = "test_oper_0_name", Skill = 0 }
            }
        };
        TestHandle(testJsonContent);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler"/>.
    /// </summary>
    /// <param name="testJsonContent">The test JSON content.</param>
    /// <param name="expectNon200Response"><c>true</c> if the result should not be 200, <c>false</c> otherwise.</param>
    /// <param name="removeNullFields">Whether null fields in JSON should be removed.</param>
    private void TestHandle(CreateCopilotOperationContent testJsonContent, bool expectNon200Response = false,
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

        if (expectNon200Response)
        {
            var response = action().GetAwaiter().GetResult();
            response.StatusCode.Should().NotBe(StatusCodes.Status200OK);
            _dbContext.CopilotOperations.Any().Should().BeFalse();
        }
        else
        {
            var response = action().GetAwaiter().GetResult();
            var id = ((CreateCopilotOperationDto)response.Data!).Id;
            _dbContext.CopilotOperations.Any().Should().BeTrue();
            var entity = _dbContext.CopilotOperations.FirstOrDefault();
            entity.Should().NotBeNull();
            entity!.Id.Should().Be(_copilotIdService.DecodeId(id));
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

// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using System.Text.Json.Serialization;

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
/// Tests <see cref="CreateCopilotOperationCommandHandler"/>.
/// </summary>
[TestClass]
public class CreateCopilotOperationCommandTest
{
    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private readonly ICopilotIdService _copilotIdService = new CopilotIdService();

    /// <summary>
    ///     The service for processing copilot server options.
    /// </summary>
    private readonly CopilotServerOption _optionsWithAllRequirement = new()
    {
        RequireDetailsInOperation = true,
        RequireTitleInOperation = true
    };

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle_Full()
    {
        var testJsonContent = new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Doc = new MaaCopilotOperationDoc { Title = "test_title", Details = "test_details" },
            Operators = new MaaCopilotOperationOperator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 },
                new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        TestHandle(testJsonContent);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle_FullRequired()
    {
        var testJsonContent = new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Doc = new MaaCopilotOperationDoc { Title = "test_title", Details = "test_details" },
            Operators = new MaaCopilotOperationOperator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 }, new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        TestHandle(testJsonContent, haveRequirement: true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without <c>doc</c> field.
    /// </summary>
    [TestMethod]
    public void TestHandle_WithoutDoc()
    {
        var testJsonContent = new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new MaaCopilotOperationOperator[]
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
        var testJsonContent = new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new MaaCopilotOperationOperator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 },
                new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        TestHandle(testJsonContent, removeNullFields: true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without <c>doc</c> field (undefined).
    /// </summary>
    [TestMethod]
    public void TestHandle_WithoutRequiredDocTitle()
    {
        var testJsonContent = new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new MaaCopilotOperationOperator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 }, new() { Name = "test_oper_1_name", Skill = 1 }
            },
            Doc = new MaaCopilotOperationDoc
            {
                Details = "details"
            }
        };
        TestHandle(testJsonContent, true, haveRequirement: true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without <c>doc</c> field (undefined).
    /// </summary>
    [TestMethod]
    public void TestHandle_WithoutRequiredDocDetails()
    {
        var testJsonContent = new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new MaaCopilotOperationOperator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 }, new() { Name = "test_oper_1_name", Skill = 1 }
            },
            Doc = new MaaCopilotOperationDoc { Title = "title" }
        };
        TestHandle(testJsonContent, true, haveRequirement: true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without <c>doc</c> field (undefined).
    /// </summary>
    [TestMethod]
    public void TestHandle_WithoutRequiredDoc()
    {
        var testJsonContent = new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new MaaCopilotOperationOperator[]
            {
                new() { Name = "test_oper_0_name", Skill = 0 }, new() { Name = "test_oper_1_name", Skill = 1 }
            }
        };
        TestHandle(testJsonContent, true, haveRequirement: true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/> without a <c>name</c> field in <c>operators</c>.
    /// </summary>
    [TestMethod]
    public void TestHandle_MissingOperatorName()
    {
        var testJsonContent = new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new MaaCopilotOperationOperator[]
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
        var testJsonContent = new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Operators = new MaaCopilotOperationOperator[]
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
    /// <param name="haveRequirement">Whether use options will all requirement.</param>
    private void TestHandle(MaaCopilotOperation testJsonContent, bool expectNon200Response = false,
        bool removeNullFields = false, bool haveRequirement = false)
    {
        var testContent = JsonSerializer.Serialize(testJsonContent,
            new JsonSerializerOptions()
            {
                DefaultIgnoreCondition =
                    removeNullFields ? JsonIgnoreCondition.Never : JsonIgnoreCondition.WhenWritingNull
            });

        var test = new HandlerTest()
            .SetupGetUser(new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, Guid.Empty));
        if (haveRequirement)
        {
            test = test.SetupCopilotServerOption(_optionsWithAllRequirement);
        }
        var result = test.TestCreateCopilotOperation(new()
        {
            Content = testContent,
        });

        if (expectNon200Response)
        {
            result.Response.StatusCode.Should().NotBe(StatusCodes.Status200OK);
            result.DbContext.CopilotOperations.Any().Should().BeFalse();
        }
        else
        {
            var id = ((CreateCopilotOperationDto)result.Response.Data!).Id;
            result.DbContext.CopilotOperations.Any().Should().BeTrue();
            var entity = result.DbContext.CopilotOperations.FirstOrDefault();
            entity.Should().NotBeNull();
            entity!.Id.Should().Be(_copilotIdService.DecodeId(id));
            entity.Content.Should().Be(testContent);
            entity.StageName.Should().Be(testJsonContent.StageName);
            entity.MinimumRequired.Should().Be(testJsonContent.MinimumRequired);
            entity.Title.Should().Be(testJsonContent.Doc?.Title ?? string.Empty);
            entity.Details.Should().Be(testJsonContent.Doc?.Details ?? string.Empty);

            var entityOperators = (from @operator in testJsonContent.Operators ?? Array.Empty<MaaCopilotOperationOperator>()
                                   select $"{@operator.Name}::{@operator.Skill?.ToString() ?? "1"}").Distinct().ToList();
            entity.Operators.Should().BeEquivalentTo(entityOperators);
        }
    }
}

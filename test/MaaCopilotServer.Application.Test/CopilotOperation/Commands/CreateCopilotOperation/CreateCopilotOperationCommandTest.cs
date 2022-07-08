// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.GameData.Entity;
using MaaCopilotServer.Infrastructure.Services;
using MaaCopilotServer.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Action = MaaCopilotServer.Application.Common.Operation.Model.Action;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
/// Tests <see cref="CreateCopilotOperationCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class CreateCopilotOperationCommandTest
{
    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private readonly ICopilotOperationService _copilotOperationService =
        new CopilotOperationService(Options.Create(new CopilotOperationOption()), new DomainString());

    private static Operation OperationFull => new()
    {
        StageName = "test_stage_name",
        MinimumRequired = "v4.0.0",
        Doc = new Doc
        {
            Title = "test_title",
            TitleColor = "test_title_color",
            Details = "test_details",
            DetailsColor = "test_details_color",
        },
        Operators = new Operator[]
        {
            new()
            {
                Name = "test_oper_0_name",
                Skill = 1
            },
            new()
            {
                Name = "test_oper_1_name",
                Skill = 2
            },
            new()
            {
                Name = "test_oper_2_name"
            }
        },
        Groups = new Group[]
        {
            new()
            {
                Name = "test_group_0_name",
                Operators = new Operator[]
                {
                    new()
                    {
                        Name = "test_g_oper_0_name",
                        Skill = 1
                    },
                    new()
                    {
                        Name = "test_g_oper_1_name",
                        Skill = 2
                    },
                    new()
                    {
                        Name = "test_g_oper_2_name"
                    }
                }
            }
        },
        Actions = new Action[]
        {
            new() { Name = "test_oper_0_name", Location = new []{ 1, 2 }, Direction = "Left" },
            new() { Type = "Skill", Name = "test_oper_0_name" },
            new() { Type = "Retreat", Name = "test_oper_0_name" },
            new() { Type = "Retreat", Location = new[] { 1, 2 } },
            new() { Type = "SkillUsage", SkillUsage = 1 },
            new() { Name = "test_oper_0_name", Location = new[] { 1, 2 }, Direction = "Left" },
            new() { Type = "技能", Name = "test_oper_0_name" },
            new() { Type = "撤退", Name = "test_oper_0_name" },
            new() { Type = "撤退", Location = new[] { 1, 2 } },
            new() { Type = "技能用法", SkillUsage = 1 }
        }
    };

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleFull()
    {
        TestHandle(OperationFull);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleWithoutAction()
    {
        var oper = OperationFull with { Actions = null };
        TestHandle(oper);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleWithoutGroup()
    {
        var oper = OperationFull with { Groups = null };
        TestHandle(oper);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleWithoutOpers()
    {
        var oper = OperationFull with { Operators = null };
        TestHandle(oper);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleWithoutDoc()
    {
        var oper = OperationFull with { Doc = null };
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleWithoutStageName()
    {
        var oper = OperationFull with { StageName = null };
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleWithoutMinimumRequired()
    {
        var oper = OperationFull with { MinimumRequired = null };
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleUnknownStageName()
    {
        var oper = OperationFull with { StageName = "unknown_stage_name" };
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleWithoutDocTitle()
    {
        var oper = OperationFull;
        oper.Doc!.Title = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleWithoutDocDetails()
    {
        var oper = OperationFull;
        oper.Doc!.Details = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleEmptyDocTitle()
    {
        var oper = OperationFull;
        oper.Doc!.Title = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleEmptyDocDetails()
    {
        var oper = OperationFull;
        oper.Doc!.Details = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleActionDeployWithoutName()
    {
        var oper = OperationFull;
        oper.Actions![0].Name = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleActionDeployWithoutLocation()
    {
        var oper = OperationFull;
        oper.Actions![0].Location = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleActionDeployWithoutDirection()
    {
        var oper = OperationFull;
        oper.Actions!.First(x => x.Type is null).Direction = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleActionSkillWithoutName()
    {
        var oper = OperationFull;
        oper.Actions!.First(x => x.Type == "Skill").Name = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleActionRetreatWithoutNameAndLocation()
    {
        var oper = OperationFull;
        oper.Actions!.First(x => x.Type == "Retreat").Name = null;
        oper.Actions!.First(x => x.Type == "Retreat").Location = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleActionSkillUsageWithoutSkillUsage()
    {
        var oper = OperationFull;
        oper.Actions!.First(x => x.Type == "SkillUsage").SkillUsage = null;
        TestHandle(oper, true);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler.Handle(CreateCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleActionUnknownType()
    {
        var oper = OperationFull with { Actions = new Action[] { new() { Type = "unknown_type" } } };
        TestHandle(oper, true);
    }


    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler"/>.
    /// </summary>
    /// <param name="testJsonContent">The test JSON content.</param>
    /// <param name="expectNon200Response"><c>true</c> if the result should not be 200, <c>false</c> otherwise.</param>
    private void TestHandle(Operation testJsonContent, bool expectNon200Response = false)
    {
        var testContent = JsonSerializer.Serialize(testJsonContent,
            new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

        var level = new ArkLevelData(new ArkLevelEntityGlobal("test_stage_name"));

        var test = new HandlerTest()
            .SetupDatabase(db => db.ArkLevelData.Add(level))
            .SetupGetUser(new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, UserRole.User, Guid.Empty));
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
            entity!.Id.Should().Be(_copilotOperationService.DecodeId(id));
            entity.Content.Should().Be(testContent);
            entity.ArkLevel.LevelId.Should().Be(testJsonContent.StageName);
            entity.MinimumRequired.Should().Be(testJsonContent.MinimumRequired);
            entity.Title.Should().Be(testJsonContent.Doc?.Title ?? string.Empty);
            entity.Details.Should().Be(testJsonContent.Doc?.Details ?? string.Empty);

            var entityOperators = (from @operator in testJsonContent.Operators ?? Array.Empty<Operator>()
                                   select $"{@operator.Name}::{@operator.Skill?.ToString(CultureInfo.InvariantCulture) ?? "1"}").Distinct().ToList();
            entity.Operators.Should().BeEquivalentTo(entityOperators);
        }
    }
}

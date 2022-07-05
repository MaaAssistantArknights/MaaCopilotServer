// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Models;

namespace MaaCopilotServer.Application.Test.Helpers;

/// <summary>
/// Tests <see cref="MaaCopilotOperationHelper"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class MaaCopilotOperationHelperTest
{
    /// <summary>
    /// The test operation data.
    /// </summary>
    /// <returns>The test data.</returns>
    private static MaaCopilotOperation GetTestData()
    {
        return new MaaCopilotOperation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Doc = new MaaCopilotOperationDoc { Title = "test_title", Details = "test_details" },
            Operators = new MaaCopilotOperationOperator[]
            {
                new() { Name = "test_oper_0_name" },
                new() { Name = "test_oper_1_name", Skill = 2 },
            },
            Groups = new MaaCopilotOperationGroup[]
            {
                new()
                {
                    Name = "test_group_0_name",
                    Operators = new MaaCopilotOperationOperator[]
                    {
                        new() { Name = "test_group_0_oper_0_name", Skill = 1 },
                        new() { Name = "test_group_0_oper_1_name", Skill = 2 },
                    }
                },
                new()
                {
                    Name = "test_group_1_name",
                    Operators = new MaaCopilotOperationOperator[]
                    {
                        new() { Name = "test_group_1_oper_0_name" },
                        new() { Name = "test_group_1_oper_1_name", Skill = 2 },
                    }
                },
                new()
                {
                    Name = "test_group_2_name",
                }
            }
        };
    }

    /// <summary>
    /// Tests <see cref="MaaCopilotOperationHelper.DeserializeMaaCopilotOperation(string)"/>.
    /// </summary>
    [TestMethod]
    public void TestDeserializeMaaCopilotOperation()
    {
        var data = GetTestData();

        MaaCopilotOperationHelper.DeserializeMaaCopilotOperation(JsonSerializer.Serialize(data)).Should().BeEquivalentTo(data);
    }

    /// <summary>
    /// Tests getters.
    /// </summary>
    [TestMethod]
    public void TestGetters()
    {
        var data = GetTestData();

        data.GetDocTitle().Should().Be("test_title");
        data.GetDocDetails().Should().Be("test_details");
        data.GetStageName().Should().Be("test_stage_name");
        data.GetMinimumRequired().Should().Be("0.0.1");
    }

    /// <summary>
    /// Tests getters with default values.
    /// </summary>
    [TestMethod]
    public void TestGettersDefaultValue()
    {
        var data = GetTestData();
        data.Doc = null;
        data.StageName = null;
        data.MinimumRequired = null;

        data.GetDocTitle().Should().Be("");
        data.GetDocDetails().Should().Be("");
        data.GetStageName().Should().Be("");
        data.GetMinimumRequired().Should().Be("");
    }

    /// <summary>
    /// Tests <see cref="MaaCopilotOperationHelper.SerializeGroup(MaaCopilotOperation)"/>.
    /// </summary>
    [TestMethod]
    public void TestSerializeGroup()
    {
        var data = GetTestData();

        var groups = data.SerializeGroup().ToArray();
        groups[0].Should().Be("test_group_0_name=>test_group_0_oper_0_name::1<>test_group_0_oper_1_name::2");
        groups[1].Should().Be("test_group_1_name=>test_group_1_oper_0_name::1<>test_group_1_oper_1_name::2");
        groups[2].Should().Be("test_group_2_name=>");
    }

    /// <summary>
    /// Tests <see cref="MaaCopilotOperationHelper.SerializeGroup(MaaCopilotOperation)"/>
    /// with empty groups.
    /// </summary>
    [TestMethod]
    public void TestSerializeGroupEmpty()
    {
        var data = GetTestData();
        data.Groups = null;

        data.SerializeGroup().Should().BeEmpty();
    }

    /// <summary>
    /// Tests <see cref="MaaCopilotOperationHelper.SerializeOperator(MaaCopilotOperation)"/>.
    /// </summary>
    [TestMethod]
    public void TestSerializezOperator()
    {
        var data = GetTestData();

        var opers = data.SerializeOperator().ToArray();
        opers[0].Should().Be("test_oper_0_name::1");
        opers[1].Should().Be("test_oper_1_name::2");
    }

    /// <summary>
    /// Tests <see cref="MaaCopilotOperationHelper.SerializeOperator(MaaCopilotOperation)"/>
    /// with empty operators.
    /// </summary>
    [TestMethod]
    public void TestSerializezOperatorEmpty()
    {
        var data = GetTestData();
        data.Operators = null;

        data.SerializeOperator().Should().BeEmpty();
    }

    /// <summary>
    /// Tests <see cref="MaaCopilotOperationHelper.DeserializeGroup(string[]?)"/>.
    /// </summary>
    [TestMethod]
    public void TestDeserializeGroup()
    {
        var data = GetTestData();

        var groups = data.SerializeGroup().ToArray().DeserializeGroup().ToList();
        groups.Should().BeEquivalentTo(new List<MaaCopilotOperationGroupStore>()
        {
            new("test_group_0_name", new() {"test_group_0_oper_0_name::1", "test_group_0_oper_1_name::2"}),
            new("test_group_1_name", new() {"test_group_1_oper_0_name::1", "test_group_1_oper_1_name::2"}),
            new("test_group_2_name", new() {""}),
        });
    }

    /// <summary>
    /// Tests <see cref="MaaCopilotOperationHelper.DeserializeGroup(string[]?)"/>
    /// with empty groups.
    /// </summary>
    [TestMethod]
    public void TestDeserializeGroupEmpty()
    {
        var data = GetTestData();
        data.Groups = null;

        var groups = ((string[]?)null).DeserializeGroup().ToList();
        groups.Should().BeEquivalentTo(new List<MaaCopilotOperationGroupStore>());
    }
}

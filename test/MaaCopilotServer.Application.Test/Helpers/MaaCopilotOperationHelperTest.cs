// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.Common.Operation;

namespace MaaCopilotServer.Application.Test.Helpers;

/// <summary>
/// Tests <see cref="OperationConvertor"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class MaaCopilotOperationHelperTest
{
    /// <summary>
    /// The test operation data.
    /// </summary>
    /// <returns>The test data.</returns>
    private static Operation GetTestData()
    {
        return new Operation
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
            Doc = new Doc { Title = "test_title", Details = "test_details" },
            Operators = new Operator[]
            {
                new() { Name = "test_oper_0_name" },
                new() { Name = "test_oper_1_name", Skill = 2 },
            },
            Groups = new Group[]
            {
                new()
                {
                    Name = "test_group_0_name",
                    Operators = new Operator[]
                    {
                        new() { Name = "test_group_0_oper_0_name", Skill = 1 },
                        new() { Name = "test_group_0_oper_1_name", Skill = 2 },
                    }
                },
                new()
                {
                    Name = "test_group_1_name",
                    Operators = new Operator[]
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
    /// Tests <see cref="OperationConvertor.SerializeGroup(Operation)"/>.
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
    /// Tests <see cref="OperationConvertor.SerializeGroup(Operation)"/>
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
    /// Tests <see cref="OperationConvertor.SerializeOperator(Operation)"/>.
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
    /// Tests <see cref="OperationConvertor.SerializeOperator(Operation)"/>
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
    /// Tests <see cref="OperationConvertor.DeserializeGroup(string[]?)"/>.
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
    /// Tests <see cref="OperationConvertor.DeserializeGroup(string[]?)"/>
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

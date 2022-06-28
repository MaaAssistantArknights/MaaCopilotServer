// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Controllers;
using MaaCopilotServer.Api.Test.TestHelpers;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

namespace MaaCopilotServer.Api.Test.Properties;

/// <summary>
///     Tests for <see cref="CopilotOperationController" />.
/// </summary>
[TestClass]
public class CopilotOperationControllerTest
{
    /// <summary>
    ///     Tests <see cref="CopilotOperationController.CreateCopilotOperation(CreateCopilotOperationCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestCreateCopilotOperation()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new CreateCopilotOperationCommand(),
            new CreateCopilotOperationDto(),
            mediator => new CopilotOperationController(mediator),
            (controller, request) => controller.CreateCopilotOperation(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotOperationController.DeleteCopilotOperation(DeleteCopilotOperationCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestDeleteCopilotOperation()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new DeleteCopilotOperationCommand(),
            null,
            mediator => new CopilotOperationController(mediator),
            (controller, request) => controller.DeleteCopilotOperation(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotOperationController.GetCopilotOperation(string)" />.
    /// </summary>
    [TestMethod]
    public void TestGetCopilotOperation()
    {
        ControllerTestHelper.TestControllerEndpoint(
            string.Empty,
            new GetCopilotOperationQueryDto(),
            mediator => new CopilotOperationController(mediator),
            (controller, request) => controller.GetCopilotOperation(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotOperationController.QueryCopilotOperation(QueryCopilotOperationsQuery)" />.
    /// </summary>
    [TestMethod]
    public void TestQueryCopilotOperation()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new QueryCopilotOperationsQuery(),
            new PaginationResult<QueryCopilotOperationsQueryDto>(),
            mediator => new CopilotOperationController(mediator),
            (controller, request) => controller.QueryCopilotOperation(request));
    }
}

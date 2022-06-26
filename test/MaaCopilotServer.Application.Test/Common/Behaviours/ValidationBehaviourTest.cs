// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;
using FluentValidation.Results;
using MaaCopilotServer.Application.Common.Behaviours;

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.Common.Behaviours;

/// <summary>
///     Tests for <see cref="ValidationBehaviour{TRequest,TResponse}" />.
/// </summary>
[TestClass]
public class ValidationBehaviourTest
{
    /// <summary>
    ///     The service of current testUser.
    /// </summary>
    private readonly ICurrentUserService _currentUserService = Mock.Of<ICurrentUserService>();

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="ValidationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with empty valiators.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmptyValidators()
    {
        var validators = new List<IValidator<IRequest<MaaApiResponse>>>();

        var behaviour =
            new ValidationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(validators, _currentUserService);
        var action = async () => await behaviour.Handle(Mock.Of<IRequest<MaaApiResponse>>(),
            new CancellationToken(),
            () => Task.FromResult(MaaApiResponseHelper.Ok()));

        action.Should().NotThrowAsync().Wait();
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="ValidationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with validators.
    /// </summary>
    /// <param name="validator1Passed">Indicates whether the first validator passes.</param>
    /// <param name="validator2Passed">Indicates whether the second validator passes.</param>
    /// <param name="expectException">The expected behaviour that whether the exception should be thrown.</param>
    [DataTestMethod]
    [DataRow(true, true, false)]
    [DataRow(true, false, true)]
    [DataRow(false, true, true)]
    [DataRow(false, false, true)]
    public void TestHandle_Validators(bool validator1Passed, bool validator2Passed, bool expectException)
    {
        var validator1 = new Mock<IValidator<IRequest<MaaApiResponse>>>();
        validator1.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()).Result)
            .Returns(validator1Passed ? new ValidationResult() : new ValidationResult(new List<ValidationFailure> { new() }));
        var validator2 = new Mock<IValidator<IRequest<MaaApiResponse>>>();
        validator2.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()).Result)
            .Returns(validator2Passed ? new ValidationResult() : new ValidationResult(new List<ValidationFailure> { new() }));
        var validators = new List<IValidator<IRequest<MaaApiResponse>>> { validator1.Object, validator2.Object };

        var behaviour =
            new ValidationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(validators, _currentUserService);
        var action = async () => await behaviour.Handle(Mock.Of<IRequest<MaaApiResponse>>(),
            new CancellationToken(),
            () => Task.FromResult(MaaApiResponseHelper.Ok()));

        if (expectException)
        {
            var response = action().GetAwaiter().GetResult();
            response.StatusCode.Should().NotBe(StatusCodes.Status200OK);
        }
        else
        {
            action.Should().NotThrowAsync().Wait();
        }
    }
}

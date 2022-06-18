// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;
using FluentValidation.Results;
using MaaCopilotServer.Application.Common.Behaviours;
using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.Common.Interfaces;
using MediatR;

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
    private ICurrentUserService _currentUserService;

    /// <summary>
    ///     The validators.
    /// </summary>
    private IEnumerable<IValidator<IRequest<string>>> _validators;

    /// <summary>
    ///     Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _currentUserService = Substitute.For<ICurrentUserService>();
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="ValidationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with empty valiators.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task TestHandle_EmptyValidators()
    {
        _validators = new List<IValidator<IRequest<string>>>();
        var behaviour = new ValidationBehaviour<IRequest<string>, string>(_validators, _currentUserService);
        var action = async () => await behaviour.Handle(Substitute.For<IRequest<string>>(),
            new CancellationToken(),
            () => Task.FromResult(string.Empty));
        await action.Should().NotThrowAsync();
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
    /// <returns>N/A</returns>
    [DataTestMethod]
    [DataRow(true, true, false)]
    [DataRow(true, false, true)]
    [DataRow(false, true, true)]
    [DataRow(false, false, true)]
    public async Task TestHandle_Validators(bool validator1Passed, bool validator2Passed, bool expectException)
    {
        var validator1 = new TestValidator(validator1Passed);
        var validator2 = new TestValidator(validator2Passed);
        _validators = new List<IValidator<IRequest<string>>> { validator1, validator2 };
        var behaviour = new ValidationBehaviour<IRequest<string>, string>(_validators, _currentUserService);
        var action = async () => await behaviour.Handle(Substitute.For<IRequest<string>>(),
            new CancellationToken(),
            () => Task.FromResult(string.Empty));
        if (expectException)
        {
            await action.Should().ThrowAsync<PipelineException>();
        }
        else
        {
            await action.Should().NotThrowAsync();
        }
    }
}

/// <summary>
///     The mock validator.
/// </summary>
internal class TestValidator : IValidator<IRequest<string>>
{
    /// <summary>
    ///     The flag that the validator should pass or not.
    /// </summary>
    private readonly bool _passed;

    /// <summary>
    ///     The constructor.
    /// </summary>
    /// <param name="passed">The flag that the validator should pass or not.</param>
    public TestValidator(bool passed)
    {
        _passed = passed;
    }

    /// <inheritdoc />
    public bool CanValidateInstancesOfType(Type type)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IValidatorDescriptor CreateDescriptor()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public ValidationResult Validate(IRequest<string> instance)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public ValidationResult Validate(IValidationContext context)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ValidationResult> ValidateAsync(IRequest<string> instance, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default)
    {
        var resultPassed = new ValidationResult();
        var resultFailed = new ValidationResult(new List<ValidationFailure> { new() });
        return Task.FromResult(_passed ? resultPassed : resultFailed);
    }
}

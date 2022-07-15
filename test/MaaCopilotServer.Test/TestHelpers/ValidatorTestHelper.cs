// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;

namespace MaaCopilotServer.Test.TestHelpers;

/// <summary>
/// The helper class of validator tests.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ValidatorTestHelper
{
    /// <summary>
    /// The validation error message.
    /// </summary>
    private static readonly Resources.ValidationErrorMessage s_validationErrorMessage = new();

    /// <summary>
    /// Tests validator.
    /// </summary>
    /// <typeparam name="TValidator">The type of the validator.</typeparam>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="request">The test request.</param>
    /// <param name="expected">The expected validation result.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the validator does not have the constructor with validation error message as the parameter.
    /// </exception>
    public static void Test<TValidator, TCommand>(TCommand request, bool expected)
        where TValidator : AbstractValidator<TCommand>
    {
        var validatorType = typeof(TValidator);
        TValidator validator;

        var ctor = validatorType.GetConstructor(new[] { typeof(Resources.ValidationErrorMessage) });
        if (ctor != null)
        {
            validator = (TValidator)ctor.Invoke(new object[] { s_validationErrorMessage });
        }
        else
        {
            ctor = validatorType.GetConstructor(Array.Empty<Type>());
            if (ctor != null)
            {
                validator = (TValidator)ctor.Invoke(Array.Empty<object?>());
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(TValidator));
            }
        }

        validator.Validate(request).IsValid.Should().Be(expected);
    }
}

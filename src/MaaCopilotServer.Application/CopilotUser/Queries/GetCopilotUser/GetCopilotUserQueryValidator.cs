// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

/// <summary>
/// The validator of getting user.
/// </summary>
public class GetCopilotUserQueryValidator : AbstractValidator<GetCopilotUserQuery>
{
    /// <summary>
    /// The constructor of <see cref="GetCopilotUserQueryValidator"/>.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    public GetCopilotUserQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.UserId)
            .NotNull().NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .When(x => x.UserId != "me")
            .WithMessage(errorMessage.UserIdIsInvalid);
        RuleFor(x => x.UserId)
            .NotNull().NotEmpty().Equal("me")
            .When(x => x.UserId == "me")
            .WithMessage(errorMessage.UserIdIsInvalid);
    }
}

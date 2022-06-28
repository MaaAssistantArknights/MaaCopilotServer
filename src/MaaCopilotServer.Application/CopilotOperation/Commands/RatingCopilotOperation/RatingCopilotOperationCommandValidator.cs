// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.RatingCopilotOperation;

public class RatingCopilotOperationCommandValidator : AbstractValidator<RatingCopilotOperationCommand>
{
    public RatingCopilotOperationCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(errorMessage.CopilotOperationIdIsEmpty);
        RuleFor(x => x.RatingType)
            .NotEmpty().IsEnumName(typeof(OperationRatingType))
            .WithMessage(errorMessage.OperationRatingTypeIsInvalid);
    }
}

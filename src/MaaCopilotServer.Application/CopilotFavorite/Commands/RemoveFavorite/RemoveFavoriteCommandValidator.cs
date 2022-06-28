// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotFavorite.Commands.RemoveFavorite;

public class RemoveFavoriteCommandValidator : AbstractValidator<RemoveFavoriteCommand>
{
    public RemoveFavoriteCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.FavoriteListId)
            .NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .WithMessage(errorMessage.FavoriteListIdIsInvalid);
        RuleFor(x => x.OperationId)
            .NotEmpty()
            .WithMessage(errorMessage.CopilotOperationIdIsEmpty);
    }
}

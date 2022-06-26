// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotFavorite.Commands.AddFavorite;

public class AddFavoriteCommandValidator : AbstractValidator<AddFavoriteCommand>
{
    public AddFavoriteCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.FavoriteListId)
            .NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .WithMessage(errorMessage.FavoriteListIdIsInvalid);
        RuleFor(x => x.OperationId)
            .NotEmpty()
            .WithMessage(errorMessage.CopilotOperationIdIsEmpty);
    }
}

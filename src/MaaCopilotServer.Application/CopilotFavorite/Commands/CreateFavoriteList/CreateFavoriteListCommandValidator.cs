// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotFavorite.Commands.CreateFavoriteList;

public class CreateFavoriteListCommandValidator : AbstractValidator<CreateFavoriteListCommand>
{
    public CreateFavoriteListCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Name)
            .NotEmpty().Length(1, 24)
            .WithMessage(errorMessage.FavoriteListNameIsInvalid);
    }
}

// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotFavorite.Queries.GetCopilotUserFavorites;

public class GetCopilotUserFavoritesQueryValidator : AbstractValidator<GetCopilotUserFavoritesQuery>
{
    public GetCopilotUserFavoritesQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.FavoriteListId)
            .NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .WithMessage(errorMessage.FavoriteListIdIsInvalid);
    }
}

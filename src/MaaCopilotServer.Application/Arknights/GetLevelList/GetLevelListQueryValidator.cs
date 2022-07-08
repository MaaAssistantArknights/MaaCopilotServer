// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Arknights.GetLevelList;

public class GetLevelListQueryValidator : AbstractValidator<GetLevelListQuery>
{
    private readonly string[] _languages = {"chinese", "english", "japanese", "korean"};

    public GetLevelListQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Server)
            .Must(x => _languages.Contains(x.ToLower()))
            .When(x => string.IsNullOrEmpty(x.Server) is false)
            .WithMessage(errorMessage.UnknownLanguageType);
    }
}

// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;

namespace MaaCopilotServer.Application.Arknights.GetOperatorList;

public class GetOperatorListQueryValidator : AbstractValidator<GetOperatorListQuery>
{
    public GetOperatorListQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Server)
            .Must(x => ArkServerLanguage.Parse(x) != ArkServerLanguage.Unknown)
            .When(x => string.IsNullOrEmpty(x.Server) is false)
            .WithMessage(errorMessage.UnknownLanguageType);
    }
}

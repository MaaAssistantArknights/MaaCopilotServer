// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

public class QueryCopilotOperationsQueryValidator : AbstractValidator<QueryCopilotOperationsQuery>
{
    public QueryCopilotOperationsQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(errorMessage.PageOutOfRange);
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1).LessThanOrEqualTo(100)
            .WithMessage(errorMessage.LimitOutOfRange);

        RuleFor(x => x.UploaderId)
            .NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .When(x => x.UploaderId != "me" && string.IsNullOrEmpty(x.UploaderId) is false)
            .WithMessage(errorMessage.UploaderIdIsInvalid);
        RuleFor(x => x.UploaderId)
            .NotEmpty().Equal("me")
            .When(x => x.UploaderId == "me")
            .WithMessage(errorMessage.UploaderIdIsInvalid);

        RuleFor(x => x.Language)
            .Must(x => ArkServerLanguage.Parse(x) != ArkServerLanguage.Unknown)
            .When(x => string.IsNullOrEmpty(x.Language) is false)
            .WithMessage(errorMessage.UnknownLanguageType);
    }
}

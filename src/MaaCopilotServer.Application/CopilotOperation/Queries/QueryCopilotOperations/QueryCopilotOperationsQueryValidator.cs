// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

public class QueryCopilotOperationsQueryValidator : AbstractValidator<QueryCopilotOperationsQuery>
{
    public QueryCopilotOperationsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Limit).GreaterThanOrEqualTo(1);
        RuleFor(x => x.StageName).NotNull();
        RuleFor(x => x.Content).NotNull();
        RuleFor(x => x.Uploader).NotNull();

        RuleFor(x => x.UploaderId)
            .NotNull().NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .When(x => x.UploaderId != "me" && string.IsNullOrEmpty(x.UploaderId) is false);
        RuleFor(x => x.UploaderId)
            .NotNull().NotEmpty().Equal("me")
            .When(x => x.UploaderId == "me");
    }
}

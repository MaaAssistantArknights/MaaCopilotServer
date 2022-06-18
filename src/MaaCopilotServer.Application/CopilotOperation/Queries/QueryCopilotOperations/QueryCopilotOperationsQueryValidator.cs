// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
///     The validator of querying multiple operations.
/// </summary>
public class QueryCopilotOperationsQueryValidator : AbstractValidator<QueryCopilotOperationsQuery>
{
    /// <summary>
    ///     The constructor of <see cref="QueryCopilotOperationsQueryValidator" />.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    public QueryCopilotOperationsQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(errorMessage.PageIsLessThenOne);
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1)
            .WithMessage(errorMessage.LimitIsLessThenOne);

        RuleFor(x => x.UploaderId)
            .NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .When(x => x.UploaderId != "me" && string.IsNullOrEmpty(x.UploaderId) is false)
            .WithMessage(errorMessage.UploaderIdIsInvalid);
        RuleFor(x => x.UploaderId)
            .NotEmpty().Equal("me")
            .When(x => x.UploaderId == "me")
            .WithMessage(errorMessage.UploaderIdIsInvalid);
    }
}

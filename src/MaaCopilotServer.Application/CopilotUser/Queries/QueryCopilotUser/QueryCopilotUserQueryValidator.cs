// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

/// <summary>
///     The validator of querying multiple users.
/// </summary>
public class QueryCopilotUserQueryValidator : AbstractValidator<QueryCopilotUserQuery>
{
    /// <summary>
    ///     The constructor of <see cref="QueryCopilotUserQueryValidator" />.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    public QueryCopilotUserQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(errorMessage.PageIsLessThenOne);
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1)
            .WithMessage(errorMessage.LimitIsLessThenOne);
    }
}

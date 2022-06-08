// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

public class CreateCopilotOperationCommandValidator : AbstractValidator<CreateCopilotOperationCommand>
{
    public CreateCopilotOperationCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotNull()
            .NotEmpty()
            .Must(BeValidatedContent);
    }

    private static bool BeValidatedContent(string? content)
    {
        try
        {
            var doc = JsonDocument.Parse(content!).RootElement;
            var stageName = doc.GetProperty("stage_name").GetString();
            var minimumRequired = doc.GetProperty("minimum_required").GetString();
            return !string.IsNullOrEmpty(stageName) && !string.IsNullOrEmpty(minimumRequired);
        }
        catch
        {
            return false;
        }
    }
}

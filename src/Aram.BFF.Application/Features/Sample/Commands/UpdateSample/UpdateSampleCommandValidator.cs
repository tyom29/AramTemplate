using FluentValidation;

namespace Aram.BFF.Application.Features.Sample.Commands.UpdateSample;

public class UpdateSampleCommandValidator : AbstractValidator<UpdateSampleCommand>
{
    public UpdateSampleCommandValidator()
    {
        RuleFor(x => x.SampleId)
            .NotEmpty()
            .WithMessage("SampleId ID is required.");

        // other rules
    }
}
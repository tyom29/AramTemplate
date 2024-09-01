using ErrorOr;
using MediatR;

namespace Aram.BFF.Application.Features.Sample.Commands.UpdateSample;

public record UpdateSampleCommand(
    Guid SampleId
    // other properties
) : IRequest<ErrorOr<Guid>>;
using Aram.BFF.Api.Base;
using Aram.BFF.Application.Features.Sample.Commands.UpdateSample;
using Aram.BFF.Contracts.Samples.Requests;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Aram.BFF.Api.Controllers;

[Route("[controller]")]
public class SampleController(ISender mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateSample(CreateSampleRequest request)
    {
        var command = new UpdateSampleCommand(request.SampleId);

        var createCityResult = await mediator.Send(command);

        return createCityResult.Match(
            sampleId => CreatedAtAction(
                null, // TODO: Add GET action name
                new { sampleId = sampleId },
                sampleId),
            Problem);
    }
}
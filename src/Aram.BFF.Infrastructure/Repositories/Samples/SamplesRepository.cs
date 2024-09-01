using Aram.BFF.Application.Features.Sample.Interfaces;
using Aram.BFF.Infrastructure.Common.Persistence;

namespace Aram.BFF.Infrastructure.Repositories.Samples;

public class SamplesRepository : ISampleRepository
{
    // move to primary constructor
    private readonly ApplicationDbContext _context;

    public SamplesRepository(ApplicationDbContext context)
    {
        _context = context;
    }
}
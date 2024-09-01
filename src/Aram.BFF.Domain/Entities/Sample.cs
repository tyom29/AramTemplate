using Aram.BFF.Domain.Common;
using Aram.BFF.Domain.Enums;

namespace Aram.BFF.Domain.Entities;

public class Sample : Entity
{
    public Sample(
        string sampleValue,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        SampleValue = SampleType.FromName(sampleValue);
    }

    private Sample()
    {
    }

    public SampleType SampleValue { get; private set; } = null!;

    // other properties

    public void Update(string sampleValue)
    {
        SampleValue = SampleType.FromName(sampleValue);
    }
}
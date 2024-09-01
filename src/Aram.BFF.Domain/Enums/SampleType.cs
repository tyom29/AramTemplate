using Ardalis.SmartEnum;

namespace Aram.BFF.Domain.Enums;

public class SampleType(string name, int value) : SmartEnum<SampleType>(name, value)
{
    public static readonly SampleType No = new(nameof(No), 0);
    public static readonly SampleType Yes = new(nameof(Yes), 1);
}
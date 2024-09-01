using Aram.BFF.Domain.Entities;
using Aram.BFF.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aram.BFF.Infrastructure.Repositories.Samples;

public class SampleConfigurations : IEntityTypeConfiguration<Sample>
{
    public void Configure(EntityTypeBuilder<Sample> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.SampleValue)
            .HasConversion(
                v => v.Value,
                v => SampleType.FromValue(v)
            );
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration;

public class ActorAwardConfiguration : IEntityTypeConfiguration<ActorAward>
{
    public void Configure(EntityTypeBuilder<ActorAward> builder)
    {
        builder.ToTable("ActorAwards", "dbo");
        builder.HasKey(x => new { x.ActorId, x.AwardId });

        builder.HasOne(aa => aa.Actor)
            .WithMany(a => a.ActorAwards)
            .HasForeignKey(aa => aa.ActorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(aa => aa.Award)
            .WithMany(a => a.ActorAwards)
            .HasForeignKey(aa => aa.AwardId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
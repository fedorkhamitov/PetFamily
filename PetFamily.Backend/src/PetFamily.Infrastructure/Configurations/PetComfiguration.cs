using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Share;

namespace PetFamily.Infrastructure.Configurations;

public class PetComfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGHT);
        builder.Property(p => p.IsSterilized)
            .IsRequired();
        builder.Property(p => p.IsVaccinated)
            .IsRequired();
        builder.Property(p => p.Color)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
        builder.OwnsOne(p => p.SpeciesInfo, si =>
        {
            si.Property(s => s.SpeciesId)
                .HasColumnName("species_id")
                .IsRequired();
            si.Property(s => s.BreedId)
                .HasColumnName("breed_id")
                .IsRequired();
        });
        builder.OwnsOne(p => p.Address, a => a.ToJson());
        builder.Property(p => p.Weight).IsRequired();
        builder.Property(p => p.Height).IsRequired();
        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
        builder.Property(p => p.BirthDate)
            .IsRequired();
        builder.OwnsOne(p => p.DonationDetails, d => d.ToJson());
        builder.Property(p => p.CreatedDate).IsRequired();
        builder.Property(p => p.HelpStatus).IsRequired();
        builder.Property<bool>("IsDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}
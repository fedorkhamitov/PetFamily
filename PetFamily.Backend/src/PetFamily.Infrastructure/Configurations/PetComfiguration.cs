using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        
        builder.OwnsOne(p => p.Address, a =>
        {
            a.ToJson();
            a.Property(adr => adr.ZipCode).HasJsonPropertyName("zipCode");
            a.Property(adr => adr.Country).HasJsonPropertyName("country");
            a.Property(adr => adr.City).HasJsonPropertyName("city");
            a.Property(adr => adr.StreetName).HasJsonPropertyName("streetName");
            a.Property(adr => adr.StreetNumber).HasJsonPropertyName("streetNumber");
            a.Property(adr => adr.Apartment).HasJsonPropertyName("apartment");
        });
        
        builder.Property(p => p.Weight).IsRequired();
        
        builder.Property(p => p.Height).IsRequired();
        
        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
        
        builder.Property(p => p.BirthDate)
            .IsRequired();
        
        builder.OwnsOne(p => p.DonationDetails, d =>
        {
            d.ToJson();
            d.Property(dd => dd.Name).HasJsonPropertyName("name");
            d.Property(dd => dd.Description).HasJsonPropertyName("description");
        });
        
        builder.Property(p => p.CreatedDate).IsRequired();
        
        builder.Property(p => p.HelpStatus).IsRequired();
        
        builder.Property<bool>("IsDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
        
        builder.Property(p => p.Files)
            .HasColumnType("jsonb")
            .HasConversion(list =>
                    JsonSerializer.Serialize(list, JsonSerializerOptions.Default),
                value => JsonSerializer.Deserialize<IReadOnlyList<PetFile>>(value, 
                    JsonSerializerOptions.Default)!,
                new ValueComparer<IReadOnlyList<PetFile>>(
                    (l1, l2) => l1!.SequenceEqual(l2!),
                    l => l.Aggregate(0, (current, value) => HashCode.Combine(current, value.GetHashCode())),
                    l => l.ToList()))
            .HasColumnName("files");
        
        builder.Property(p => p.Position)
            .HasConversion(p => p.Value, value => Position.Create(value).Value);
    }
}
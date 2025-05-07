using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Infrastructure;
using PetFamily.Domain.Models;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value)
                );
        builder.OwnsOne(v => v.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("first_name");
            name.Property(n => n.SecondName)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("second_name");
            name.Property(n => n.LastName)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("last_name");
        });
        builder.Property(v => v.Email)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGHT);
        builder.Property(v => v.YearsOfWorkExp)
            .IsRequired();
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(v => v.PhoneNumber)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
        builder.OwnsOne(v => v.Donation, d =>
        {
            d.ToJson();
            d.Property(dnt => dnt.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
            d.Property(dnt => dnt.Description)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
        });
        builder.OwnsOne(v => v.SocialNetworkList, vbuilder =>
        {
            vbuilder.ToJson();
            vbuilder.OwnsMany(snw => snw.SnList, snwbuilder =>
            {
                snwbuilder.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
                snwbuilder.Property(s => s.Url)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
            });
        });
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}
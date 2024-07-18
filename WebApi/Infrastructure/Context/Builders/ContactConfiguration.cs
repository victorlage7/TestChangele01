using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Infrastructure.Context.Builders;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contact");

        builder.HasKey(x => x.ContactId);

        builder.Property(x => x.ContactId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasColumnType("VARCHAR").HasMaxLength(250).IsRequired();

        builder.OwnsMany(p => p.PhoneNumbers, phoneNumberBuilder =>
        {
            phoneNumberBuilder.Property<Guid>("ContactId");
            phoneNumberBuilder.WithOwner().HasForeignKey("ContactId");
            phoneNumberBuilder.HasKey("ContactId", "Type", "CountryCode", "AreaCode", "Number");
            phoneNumberBuilder.ToTable("ContactPhoneNumbers");

            phoneNumberBuilder.Property(x => x.Type)
                .IsRequired();

            phoneNumberBuilder.Property(x => x.CountryCode)
                .HasMaxLength(2)
                .IsRequired();

            phoneNumberBuilder.Property(x => x.AreaCode)
                .HasColumnType("VARCHAR")
                .HasMaxLength(4)
                .IsRequired();

            phoneNumberBuilder.Property(x => x.Number)
                .HasColumnType("VARCHAR")
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.OwnsMany(c => c.EmailAddresses, emailBuilder =>
        {
            emailBuilder.Property<Guid>("ContactId");
            emailBuilder.WithOwner().HasForeignKey("ContactId");
            emailBuilder.HasKey("ContactId", "Type", "Address");
            emailBuilder.ToTable("ContactEmails");


            emailBuilder.Property(x => x.Type)
                .IsRequired();

            emailBuilder.Property(x => x.Address)
                .HasMaxLength(255)
                .IsRequired();
        });
    }
}
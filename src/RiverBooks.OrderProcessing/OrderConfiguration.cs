using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.OrderProcessing;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        //for now we differ from the course and accept database gnerated id's

        builder.ComplexProperty(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street1)
              .HasMaxLength(Constants.STREET_MAXLENGTH);
            address.Property(a => a.Street2)
              .HasMaxLength(Constants.STREET_MAXLENGTH);
            address.Property(a => a.City)
              .HasMaxLength(Constants.CITY_MAXLENGTH);
            address.Property(a => a.State)
              .HasMaxLength(Constants.STATE_MAXLENGTH);
            address.Property(a => a.PostalCode)
              .HasMaxLength(Constants.POSTALCODE_MAXLENGTH);
            address.Property(a => a.Country)
              .HasMaxLength(Constants.COUNTRY_MAXLENGTH);
        });

        builder.ComplexProperty(o => o.BillingAddress, address =>
        {
            address.Property(a => a.Street1)
              .HasMaxLength(Constants.STREET_MAXLENGTH);
            address.Property(a => a.Street2)
              .HasMaxLength(Constants.STREET_MAXLENGTH);
            address.Property(a => a.City)
              .HasMaxLength(Constants.CITY_MAXLENGTH);
            address.Property(a => a.State)
              .HasMaxLength(Constants.STATE_MAXLENGTH);
            address.Property(a => a.PostalCode)
              .HasMaxLength(Constants.POSTALCODE_MAXLENGTH);
            address.Property(a => a.Country)
              .HasMaxLength(Constants.COUNTRY_MAXLENGTH);
        });
    }
}

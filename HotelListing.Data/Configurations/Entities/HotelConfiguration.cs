using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(h => h.Id);
            builder.HasData(
               new Hotel
               {
                   Id = 1,
                   Name = "Sandals Resort and spa",
                   Address = "Negril",
                   CountryId = 1,
                   Rating = 4.5
               },
               new Hotel
               {
                   Id = 2,
                   Name = "Grand palladium",
                   Address = "Nessua",
                   CountryId = 2,
                   Rating = 4
               },
               new Hotel
               {
                   Id = 3,
                   Name = "Comfort Suites",
                   Address = "George Town",
                   CountryId = 3,
                   Rating = 4.3
               }
           );
        }
    }
}
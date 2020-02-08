using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using newNet.Models;

namespace newNet.Data
{
    public class VehicleConfig : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            // builder.HasOne(v => v.VehicleOwnerId)
            // .WithMany(u => u.Vehicles)
            // .HasForeignKey(v => v.UserId);
        }
    }
}
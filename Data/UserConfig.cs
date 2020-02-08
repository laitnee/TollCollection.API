using Microsoft.EntityFrameworkCore;
using newNet.Models;

namespace newNet.Data {
    public class UserConfig : IEntityTypeConfiguration<User> {
        public void Configure (Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder) {
            builder.HasMany (u => u.Vehicles)
                .WithOne (v => v.VehicleOwner)
                .HasForeignKey (v => v.VehicleOwnerId)
                .OnDelete (DeleteBehavior.Cascade);

        }
    }
}
using Microsoft.EntityFrameworkCore;
using newNet.Models;

namespace newNet.Data
{
    public class PlazaConfig : IEntityTypeConfiguration<Plaza>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Plaza> builder)
        {
            
        }
    }
}
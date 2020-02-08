using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using newNet.Models;

namespace newNet.Data
{
    public class ChargeLogConfig : IEntityTypeConfiguration<ChargeLog>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ChargeLog> builder)
        {
            
        }
    }
}
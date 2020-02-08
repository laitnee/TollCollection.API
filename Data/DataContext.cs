using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using newNet.Models;

namespace newNet.Data
{
    public class DataContext : DbContext
    {
        private IConfiguration _configuration;
        public DataContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Plaza> Plazas { get; set; }
        public DbSet<ChargeLog> ChargeLogs { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         var connectionString = _configuration.GetConnectionString("TollCollectionDb");
        //         optionsBuilder.UseMySql(connectionString);
        //     }
        // }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new PlazaConfig());
            builder.ApplyConfiguration(new VehicleConfig());
            builder.ApplyConfiguration(new ChargeLogConfig());
        }


    }
}
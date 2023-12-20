using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;

namespace WeatherApp.Api;

public class WeatherContext : DbContext
{
    public DbSet<Country> Countries { get; set; }

    public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CountryConfiguration());
    }
}

public class Country()
{
    public string Name { get; set; }
    public NpgsqlPoint Location { get; set; }
}

public class CountryConfiguration() : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("country");
        builder.HasKey(e => e.Name);
    }
}
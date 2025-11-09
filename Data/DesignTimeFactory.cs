using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Azorian.Data;

public sealed class DesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Find and load configuration just like Program.cs
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .AddUserSecrets(typeof(DesignTimeFactory).Assembly, optional: true)
            .AddEnvironmentVariables()
            .Build();

        var cs = config.GetConnectionString("Default")
                 ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(cs)
            .Options;

        return new AppDbContext(options);
    }
}
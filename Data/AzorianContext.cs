using Microsoft.EntityFrameworkCore;

namespace Azorian.Data;

/// <summary>
/// Entity Framework database context for the Azorian application.
/// </summary>
public class AzorianContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AzorianContext"/> class.
    /// </summary>
    /// <param name="options">The options to configure the context.</param>
    public AzorianContext(DbContextOptions<AzorianContext> options) : base(options)
    {
    }

    // Add DbSet<T> properties for your entities here in the future.
}

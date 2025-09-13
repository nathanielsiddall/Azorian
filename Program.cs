using Azorian.Data;
using Azorian.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

/// <summary>
/// Entry point for the Azorian web application.
/// Configures services and middleware including controllers, Swagger, OpenAPI, and DocFX static docs.
/// </summary>
public class Program
{
    /// <summary>
    /// Builds and runs the web application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add OpenAPI/Swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        });

        // Add MVC controllers
        builder.Services.AddControllers();

        // Configure authentication and authorization
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<TokenService>();

        // Configure EF Core with PostgreSQL
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<AzorianContext>(options =>
            options.UseNpgsql(connectionString));

        // Add OpenAPI (minimal APIs + controller discovery)
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Ensure the database and schema exist
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AzorianContext>();
            db.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            // Map OpenAPI definition endpoint (v1 by default)
            app.MapOpenApi();

            // Swagger middleware for interactive docs
            app.UseSwagger();
            app.UseSwaggerUI();

            // Make /docs auto-serve index.html from the DocFX _site folder
            var docsPath = Path.Combine(builder.Environment.ContentRootPath, "_site");

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = new PhysicalFileProvider(docsPath),
                RequestPath = "/docs"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(docsPath),
                RequestPath = "/docs"
            });
        }
        else
        {
            // In production, force HTTPS redirection
            app.UseHttpsRedirection();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        // Map attribute-routed controllers
        app.MapControllers();

        app.Run();
    }
}

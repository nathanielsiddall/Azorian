using Azorian.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        // Add application services
        builder.Services.AddSingleton<IUserService, InMemoryUserService>();

        // Configure JWT authentication
        var jwtKey = builder.Configuration["Jwt:Key"] ?? "ChangeThisSecretKey";
        var key = Encoding.ASCII.GetBytes(jwtKey);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        builder.Services.AddAuthorization();

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

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
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

        app.UsePathBase("/1");
        app.UseAuthentication();
        app.UseAuthorization();

        // Map attribute-routed controllers
        app.MapControllers();

        app.Run();
    }
}

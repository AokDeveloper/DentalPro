using DentalApp.Application;
using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using DentalApp.Infrastructure;
using DentalApp.Persistence;
using DentalApp.Persistence.Contexts;
using DentalApp.WebApi.Endpoints.Patients;
using DentalApp.WebApi.Infrastructure;
using DentalApp.WebApi.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("DentalApp Baþlatýlýyor...");

    var builder = WebApplication.CreateBuilder(args);

    // 2. Serilog Entegrasyonu
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // --- MEVCUT SERVÝSLER ---
    builder.Services.AddApplicationServices();
    builder.Services.AddPersistenceServices(builder.Configuration);
    builder.Services.AddInfrastructureServices();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

    // ---IDENTITY AYARLARI ---
    // Veritabaný ile kullanýcý tablolarýný eþleþtiriyoruz
    builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    // --- JWT DOÐRULAMA AYARLARI ---
    // Gelen Token'ýn geçerli olup olmadýðýný kontrol eden mekanizma
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
        };
    });

    builder.Services.AddAuthorization();

    // 3. FASTENDPOINTS AYARLARI
    builder.Services.AddFastEndpoints(options =>
    {
        options.Assemblies = new[] { typeof(Create).Assembly };
    });

    builder.Services.SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "DentalApp API";
            s.Version = "v1";

            // --- Swagger'a Kilit Ýkonu Koyma ---
            s.AddAuth("Bearer", new()
            {
                Type = NSwag.OpenApiSecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Token'ý buraya girin (Bearer yazmanýza gerek yok, sadece token)"
            });
        };
        o.AutoTagPathSegmentIndex = 0;
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", b => b.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
    });

    builder.Services.AddExceptionHandler<DentalApp.WebApi.Infrastructure.GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    // --- MIDDLEWARE ---

    app.UseExceptionHandler();
    app.UseCors("AllowAll");
    app.UseSerilogRequestLogging();

    // KÝMLÝK VE YETKÝ KONTROLÜ ---
    // Sýralama ÇOK ÖNEMLÝDÝR. UseFastEndpoints'ten ÖNCE olmalý.
    app.UseAuthentication(); // 1. Kimsin? (Token kontrolü)
    app.UseAuthorization();  // 2. Yetkin var mý? (Admin/Doktor kontrolü)

    
    app.UseFastEndpoints();

    app.UseSwaggerGen();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Uygulama beklenmedik bir þekilde sonlandý!");
}
finally
{
    Log.CloseAndFlush();
}
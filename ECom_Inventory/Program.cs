using ECom_Inventory.Data;
using ECom_Inventory.Interfaces;
using ECom_Inventory.Model;
using ECom_Inventory.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console() // Always log to console
        .WriteTo.Conditional(
            evt => builder.Configuration.GetValue<bool>("Logging:EnableFileLogging"),
            wt => wt.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Conditionally log to file
        )
        .CreateLogger();

    builder.Host.UseSerilog(); // Use Serilog for logging

    builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

    // Add services to the container
    builder.Services.AddRazorPages();
    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Add Authentication with JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:SecretKey"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = builder.Configuration["AuthSettings:Issuer"],
                ValidAudience = builder.Configuration["AuthSettings:Audience"],
                RoleClaimType = "role"
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Check if JWT exists in the AuthToken cookie
                    var token = context.Request.Cookies["AuthToken"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                },
                OnForbidden = context =>
                {
                     context.Response.Redirect("/Auth/Denied"); // Redirect unauthorized users
                     return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    if (!context.Response.HasStarted)
                    {
                        context.HandleResponse(); // Prevent default 401 response
                        context.Response.Redirect("/Auth/Denied"); // Redirect for 401 Unauthorized
                    }
                    return Task.CompletedTask;
                }
            };
        });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy =>
            policy.RequireClaim(ClaimTypes.Role, "Admin"));
    });
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.AccessDeniedPath = "/Auth/Denied";
    });



    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IAuditLogService, AuditLogService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IBrandService, BrandService>();
    builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
    builder.Services.AddScoped<ISupplierService, SupplierService>();

    var app = builder.Build();

    // Error handling
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Middleware setup
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    if (builder.Configuration.GetValue<bool>("Logging:EnableHttpRequestLogging"))
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            options.GetLevel = (ctx, elapsed, ex) => Serilog.Events.LogEventLevel.Information;
        });
    }

    app.MapRazorPages();
    app.MapControllers();
    app.Use(async (context, next) =>
    {
        await next();

        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            context.Response.Redirect("/Auth/NotFound");
        }
    });
    Log.Information("Application is starting up...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed.");
}
finally
{
    Log.CloseAndFlush();
}
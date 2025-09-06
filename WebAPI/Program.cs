using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Extensions;
using WebAPI.Models.Entities;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Validate email configuration before adding services
var emailSettings = builder.Configuration.GetSection("EmailSettings");
if (string.IsNullOrEmpty(emailSettings["DefaultFromEmail"]) || 
    string.IsNullOrEmpty(emailSettings["SMTPSetting:Host"]))
{
    throw new InvalidOperationException("Email configuration is incomplete. Please check EmailSettings in appsettings.json");
}

builder.Services.AddFluentEmail(builder.Configuration);
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200", 
                "https://localhost:4200",
                "http://localhost:3000",  // React default port
                "https://localhost:3000",
                "http://127.0.0.1:4200",
                "https://127.0.0.1:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Required for token-based authentication
    });
    
    // Add a named policy for production
    options.AddPolicy("Production", policy =>
    {
        policy
            .WithOrigins("https://yourdomain.com") // Replace with your production domain
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "LiveSchool API", Version = "v1" });
});
builder.Services.AddAuthorization();

builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

// Configure Cookie Authentication after AddIdentity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "LiveSchoolAuth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use SameAsRequest in production
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Path = "/";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    
    // Configure events to return 401 instead of redirecting
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
    
    // Configure events to add custom claims to the authentication cookie
    options.Events.OnSigningIn = async context =>
    {
        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
        var user = await userManager.GetUserAsync(context.Principal);
        
        if (user != null)
        {
            // Add custom claims to the authentication cookie
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim("FirstName", user.FirstName),
                new System.Security.Claims.Claim("LastName", user.LastName),
                new System.Security.Claims.Claim("Phone", user.Phone ?? string.Empty),
                new System.Security.Claims.Claim("Email", user.Email ?? string.Empty)
            };
            
            // Add role claims
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role));
            }
            
            var identity = context.Principal.Identity as System.Security.Claims.ClaimsIdentity;
            identity?.AddClaims(claims);
        }
    };
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddDbContext<ApplicationDBContext>(options 
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

/*app.UseHttpsRedirection();*/

// CORS must be before Authentication and Authorization
if (app.Environment.IsDevelopment())
{
    app.UseCors(); // Uses default policy
}
else
{
    app.UseCors("Production"); // Uses production policy
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();

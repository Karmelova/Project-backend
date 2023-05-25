using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace WebAPI.Configuration
{
    public static class Configure
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services
                .AddDbContext<ApplicationDbContext>()
                .AddIdentity<UserEntity, UserRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
                opt.AddPolicy("Email", policy =>
                {
                    policy.RequireClaim("email");
                });
            });
            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    if (jwtSettings.Secret != null)
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                            ClockSkew = TimeSpan.FromSeconds(60)
                        };
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenException))
                            {
                                context.Response.Headers.Add("Token-expired", "true");
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("401 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("403 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }

        public static async void AddUsers(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<UserEntity>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<UserRole>>();

                if (!roleManager.RoleExistsAsync("ADMIN").Result)
                {
                    var adminRole = new UserRole { Name = "ADMIN" };
                    roleManager.CreateAsync(adminRole).Wait();
                }
                if (!roleManager.RoleExistsAsync("USER").Result)
                {
                    var adminRole = new UserRole { Name = "USER" };
                    roleManager.CreateAsync(adminRole).Wait();
                }

                var find = await userManager.FindByEmailAsync("administrator@admin.pl");
                var findUser = await userManager.FindByEmailAsync("user@test.com");
                if (find == null)
                {
                    UserEntity user = new UserEntity() { Email = "administrator@admin.pl", UserName = "admin" };

                    var saved = await userManager?.CreateAsync(user, "!Administrator123");
                    await userManager.AddToRoleAsync(user, "ADMIN");
                }
                if (findUser == null)
                {
                    UserEntity user = new UserEntity() { Email = "user@test.com", UserName = "user" };

                    var saved = await userManager?.CreateAsync(user, "!User123");
                    await userManager.AddToRoleAsync(user, "USER");
                }
            }
        }
    }
}
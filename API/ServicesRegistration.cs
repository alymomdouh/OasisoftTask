using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OasisoftTask.Applications.IServices;
using OasisoftTask.Applications.Services;
using OasisoftTask.Common;
using OasisoftTask.Core.DomainModels;
using OasisoftTask.Infrastructure;
using OasisoftTask.Infrastructure.IRepositories;
using OasisoftTask.Infrastructure.Repositories;
using OasisoftTask.Infrastructure.UnitOfWorks;
using System.Reflection;
using System.Text;

namespace OasisoftTask.API
{
    public static class ServicesRegistration
    {

        public static void Register_SingletonServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        public static void Register_Repository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped<IToDoRepository, ToDoRepository>();
        }
        public static void AddMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
        public static void Register_Service(this IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddScoped<IToDoService, ToDoService>();
            services.AddScoped<IServiceAccount, ServiceAccount>();
            services.AddScoped<ILiveToDoService, LiveToDoService>();
            services.AddScoped<IApiHelperService, ApiHelperService>();

        }
        public static void Register_Identity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.AllowedForNewUsers = false;
                // Configure password requirements
                options.Password.RequireDigit = false; // No digits required
                options.Password.RequireLowercase = false; // No lowercase letters required
                options.Password.RequireNonAlphanumeric = false; // No non-alphanumeric characters required
                options.Password.RequireUppercase = false; // No uppercase letters required
                options.Password.RequiredLength = 6; // Minimum password length
                options.Password.RequiredUniqueChars = 0; // Minimum unique characters required in the password 
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
        public static void Register_Cors(this IServiceCollection services, string _corsPolicy)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicy, op => op.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });
        }


        public static void Register_SwaggerAndSecurity(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

        }
        public static void Register_DbContext(this IServiceCollection services, ConfigurationManager Configuration)
        {
            // var _connectionString = Configuration.GetConnectionString("DefaultConnection");
            var _connectionString = Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_connectionString
                     , b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                    );
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
        public static void Register_JWTConfig(this IServiceCollection services, ConfigurationManager Configuration)
        {
            services.Configure<JwtSetupData>(Configuration.GetSection("JwtSetupData"));

            var jwtSetupData = new JwtSetupData();
            Configuration.Bind(nameof(jwtSetupData), jwtSetupData);
            services.AddSingleton(jwtSetupData);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSetupData.SecretKey)),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSetupData.Issuer,
                    ClockSkew = TimeSpan.Zero,
                };
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        return Task.CompletedTask;
                    }
                };
            });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });
        }
        public static async Task<IApplicationBuilder> MainSeed(this IApplicationBuilder app)
        {
            try
            {
                var scopedFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
                using var scope = scopedFactory!.CreateScope();
                var applicationRole = scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
                var applicationUser = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                await DefaultSeeding.SeedSuperAdminRole(applicationRole!);
                await DefaultSeeding.SeedSuperAdminUserAsync(applicationUser!);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return app;
        }
    }
}

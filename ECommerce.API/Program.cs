using ECommerce.API.CustomMiddelwares;
using ECommerce.API.Extentions;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModuleEntities;
using ECommerce.Persistence.Data.Contetxts;
using ECommerce.Persistence.DataSeed;
using ECommerce.Persistence.IdentityData.Contexts;
using ECommerce.Persistence.IdentityData.DataSeed;
using ECommerce.Persistence.Repository;
using ECommerce.Persistence.UnitOfWork;
using ECommerce.Services;
using ECommerce.Services.Abstraction;
using ECommerce.Services.Abstraction.HelperServiceInterfaces;
using ECommerce.Services.MappingProfiles.ProductMappingProfiles;
using ECommerce.Services.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using StackExchange.Redis;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region DI Registration
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddKeyedScoped<IDataInitializer , DataInitializer>("default");
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("identity");
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(x => x.LicenseKey = "" ,typeof(ProductProfile).Assembly);
            builder.Services.AddScoped<IProductServices, ProductServices>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketServices, BasketServices>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value!.Errors.Count > 0)
                    .ToDictionary(x => x.Key, x => x.Value!.Errors.Select(x => x.ErrorMessage).ToArray());
                    var problem = new ProblemDetails()
                    {
                        Title = "Validation Errors",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "One Or More Model State InValid",
                        Extensions = { { "Errors", errors } }
                    };
                    return new BadRequestObjectResult(problem);
                };
            });

            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!))
                };
            });

            builder.Services.AddScoped<IIdentityAddressRepository, IdentityAddressRepository>();
            #endregion

            var app = builder.Build();

            #region Seed Data
            await app.MigrateDataBaseAsync();
            await app.MigrateIdentityDataBaseAsync();
            await app.SeedDataAsync();
            await app.SeedIDentityDataAsync();
            #endregion


            #region Piplines [MiddelWares]
            //app.   [Show All Built In Middle Ware]
            //app.Use(async (context,next) => ) [Use Method => Used To Writte Custome Logic Not Custom Middle ware ]
            //app.UseMiddleware<>(); => [To Call Custom Middle Ware ]
            app.UseMiddleware<ExceptionHandlerMiddelware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #endregion

            app.Run();

        }
    }
}

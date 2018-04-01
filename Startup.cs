using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Stazo.API.Data;
using Stazo.API.Dtos;
using Stazo.API.Models;

namespace Stazo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            
            services.AddMvc().AddJsonOptions(opt => {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            TypeAdapterConfig<LocationForCreationDto, Location>
                .NewConfig()
                .Map(dest => dest.Photo,
                    src => new Photo() {
                        Url = src.PhotoUrl
                })
                .Map(dest => dest.Market,
                    src => (Market)Enum.Parse(typeof(Market), src.MarketType));

            TypeAdapterConfig<Location, LocationForDisplayDto>
                .NewConfig()
                .Map(dest => dest.PhotoUrl,
                    src => src.Photo.Url)
                .Map(dest => dest.MarketType,
                    src => Enum.GetName(typeof(Market), src.Market));

            TypeAdapterConfig<ProductForCreationDto, Product>
                .NewConfig()
                // .Map(dest => dest.Brand,
                //     src => src.Brand)
                .Ignore(dest => dest.Brand)
                // .Ignore(src => src.Brand)
                .Map(dest => dest.Photo,
                    src => new Photo() {
                        Url = src.PhotoUrl
                    })
                .Map(dest => dest.MarketType,
                    src => (Market)Enum.Parse(typeof(Market), src.Market));

            TypeAdapterConfig<Product, ProductForDisplayDto>
                .NewConfig()
                // .Ignore(dest => dest.Locations)
                // .Ignore(src => dest.Locations)
                // .Ignore(src => src.LocationProducts)
                .Map(dest => dest.PhotoUrl,
                    src => src.Photo.Url)
                .Map(dest => dest.CategoryName,
                    src => src.Category.Name)
                .Map(dest => dest.CategoryName,
                    src => src.Category.Name)
                .Map(dest => dest.BrandName,
                    src => src.Brand.Name)
                // .Map(dest => dest.Locations,
                //     src => TypeAdapter.Adapt<List<Location>, List<LocationForDisplayDto>>(src.LocationProducts.Select(x => x.Location).ToList()))
                .Map(dest => dest.Market,
                    src => Enum.GetName(typeof(Market), src.MarketType));

            TypeAdapterConfig<Product, SimpleProductForDisplayDto>
                .NewConfig()
                .Map(dest => dest.PhotoUrl,
                    src => src.Photo.Url)
                .Map(dest => dest.CategoryName,
                    src => src.Category.Name)
                .Map(dest => dest.CategoryName,
                    src => src.Category.Name)
                .Map(dest => dest.BrandName,
                    src => src.Brand.Name)
                .Map(dest => dest.Market,
                    src => Enum.GetName(typeof(Market), src.MarketType));

            TypeAdapterConfig<ReviewForCreationDto, Review>
                .NewConfig()
                .Map(dest => dest.StarScore,
                    src => src.Star)
                .Map(dest => dest.Photos,
                    src => src.PhotoUrls.Select(x => new Photo() { Url = x }).ToList());

            TypeAdapterConfig<User, UserForDisplayDto>
                .NewConfig()
                .Map(dest => dest.Reviews,
                    src => TypeAdapter.Adapt<IEnumerable<Review>, IEnumerable<ReviewForDisplayDto>>(src.Reviews));

            TypeAdapterConfig<User, UserForDisplayInReviewDto>
                .NewConfig();

            TypeAdapterConfig<Review, ReviewForDisplayDto>
                .NewConfig()
                .Map(dest => dest.Location,
                    src => src.Location.Adapt<LocationForDisplayDto>());
                // .Map(dest => dest.User,
                //     src => src.User.Adapt<UserForDisplayDto>())
                // .Map(dest => dest.Product,
                //     src => src.Product.Adapt<SimpleProductForDisplayDto>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => 
            x.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
            .AllowCredentials());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

using System;
using System.Text;
using FluentValidation.AspNetCore;
using Lovys.Core.Calendar.Data;
using Lovys.Core.Calendar.Entities;
using Lovys.Core.Calendar.Repositories;
using Lovys.Core.Calendar.Services;
using Lovys.Core.Calendar.Util.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Lovys.WebApp
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
            services.AddHealthChecks();
            
            services.AddDbContext<APIContext>(opt =>
                opt.UseInMemoryDatabase("lovys"));
            
            services.AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Lovys.WebApp", Version = "v1"}); 
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {jwtSecurityScheme, Array.Empty<string>()}
                });
            });
            
            
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            services.AddScoped<IUserRepository, UserRepository>();            
            services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();            
            services.AddScoped<IAvailabilityService, AvailabilityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lovys.WebApp v1"));
            }
            using (var scope = app.ApplicationServices.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<APIContext>();

                AddTestData(context);
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        private static void AddTestData(APIContext context)
        {
            var testUser1 = new User
            {
                Id = "abc123",
                Name = "string",
                Username = "string",
                Password = "string",
                Role = UserRole.Interviewer,
            };
 
            context.Users.Add(testUser1);
 
            var testAvailability = new Availability()
            {
                Id = "def234",
                User = testUser1,
                EndTime = "9pm",
                StartTime = "8pm",
                DayOfWeek = DayOfWeek.Friday
            };
 
            context.Availabilities.Add(testAvailability);
 
            context.SaveChanges();
        }
        
    }
}
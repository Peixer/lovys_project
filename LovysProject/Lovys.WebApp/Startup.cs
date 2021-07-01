using System;
using System.Text;
using FluentDateTime;
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
            // if (env.IsDevelopment())
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
            AddDianaInterviewer(context);
            AddMaryInterviewer(context);
            AddJohnCandidate(context);
 
            context.SaveChanges();
        }

        private static void AddDianaInterviewer(APIContext context)
        {
            var dianaUserInterviewer = new User
            {
                Name = "Diana",
                Username = "diana",
                Password = "123456",
                Role = UserRole.Interviewer,
            };  
 
            context.Users.Add(dianaUserInterviewer);
            context.Availabilities.Add(new Availability()
            {
                User = dianaUserInterviewer,
                StartTime = "12pm",
                EndTime = "6pm",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Monday
            });
            context.Availabilities.Add(new Availability()
            {
                User = dianaUserInterviewer,
                StartTime = "12pm",
                EndTime = "6pm",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Wednesday
            });
            context.Availabilities.Add(new Availability()
            {
                User = dianaUserInterviewer,
                StartTime = "9am",
                EndTime = "12pm",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Tuesday
            });
            context.Availabilities.Add(new Availability()
            {
                User = dianaUserInterviewer,
                StartTime = "9am",
                EndTime = "12pm",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Thursday
            });
        }

        private static void AddMaryInterviewer(APIContext context)
        {
            var maryUserInterviewer = new User
            {
                Name = "Mary",
                Username = "mary",
                Password = "123456",
                Role = UserRole.Interviewer,
            };

            context.Users.Add(maryUserInterviewer);
            
            context.Availabilities.Add(new Availability()
            {
                User = maryUserInterviewer,
                StartTime = "9am",
                EndTime = "4pm",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Monday
            });
            context.Availabilities.Add(new Availability()
            {
                User = maryUserInterviewer,
                StartTime = "9am",
                EndTime = "4pm",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Tuesday
            });
            context.Availabilities.Add(new Availability()
            {
                User = maryUserInterviewer,
                StartTime = "9am",
                EndTime = "4pm",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Wednesday
            });
            context.Availabilities.Add(new Availability()
            {
                User = maryUserInterviewer,
                StartTime = "9am",
                EndTime = "4pm",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Thursday
            });
            context.Availabilities.Add(new Availability()
            {
                User = maryUserInterviewer,
                StartTime = "9am",
                EndTime = "4pm",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Friday
            });
        }
        
        private static void AddJohnCandidate(APIContext context)
        {
            var johnUserCandidate = new User
            {
                Name = "John",
                Username = "john",
                Password = "123456",
                Role = UserRole.Candidate,
            };  
 
            context.Users.Add(johnUserCandidate);
            context.Availabilities.Add(new Availability()
            {
                User = johnUserCandidate,
                StartTime = "9am",
                EndTime = "10am",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Monday
            });
            context.Availabilities.Add(new Availability()
            {
                User = johnUserCandidate,
                StartTime = "9am",
                EndTime = "10am",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Tuesday
            });
            context.Availabilities.Add(new Availability()
            {
                User = johnUserCandidate,
                StartTime = "9am",
                EndTime = "10am",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Wednesday
            });
            context.Availabilities.Add(new Availability()
            {
                User = johnUserCandidate,
                StartTime = "9am",
                EndTime = "10am",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Thursday
            });
            context.Availabilities.Add(new Availability()
            {
                User = johnUserCandidate,
                StartTime = "9am",
                EndTime = "10am",
                StartDate = DateTime.Now.WeekAfter().BeginningOfWeek(),
                EndDate = DateTime.Now.WeekAfter().EndOfWeek(),
                DayOfWeek = DayOfWeek.Friday
            });
        }
    }
}
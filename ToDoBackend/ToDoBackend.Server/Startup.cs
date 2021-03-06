using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDoBackend.Auth.Identity;
using ToDoBackend.Auth.Interfaces;
using ToDoBackend.Auth.Services;
using ToDoBackend.BLL;
using ToDoBackend.BLL.Interfaces;
using ToDoBackend.BLL.Services;
using ToDoBackend.DAL.Database;
using ToDoBackend.DAL.Interfaces;
using ToDoBackend.Server.Filters;

namespace ToDoBackend.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddScoped<ICaseService, CaseService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddControllers(options => 
                options.Filters.Add(new ExceptionFilter()));
            
            services.AddCors();
            
            services.AddDbContext<AuthContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("Authentication")));
            services.AddDbContext<ToDoContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("ToDo")));
            
            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    
                    options.User.RequireUniqueEmail = true;
                    
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<AuthContext>();
            
            services.AddScoped<RoleSeeder>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                        ValidateIssuerSigningKey = true
                    };
                });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Market", Version = "v1" });            
            });
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            RoleSeeder seeder)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader());
 
            app.UseRouting();
 
            app.UseAuthentication();
            app.UseAuthorization();
            
            seeder.SeedRolesAsync();
            seeder.SeedAdminAsync();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json","Market API"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}




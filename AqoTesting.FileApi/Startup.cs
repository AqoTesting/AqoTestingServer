using AqoTesting.Shared.Attributes;
using AqoTesting.Shared.Infrastructure;
using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AqoTesting.FileApi
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
            services.AddControllers();

            services.Configure<AuthOptionsConfig>(Configuration.GetSection("AuthOptions"));
            services.Configure<RedisConnectionConfig>(Configuration.GetSection("RedisConnection"));

            services.AddSingleton<ICacheRepository, CacheRepository>();
            services.AddSingleton<ITokenRepository, TokenRepository>();

            services.AddCors();

            var authOptions = Configuration.GetSection("AuthOptions").Get<AuthOptionsConfig>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ��������, ����� �� �������������� �������� ��� ��������� ������
                        ValidateIssuer = true,
                        // ������, �������������� ��������
                        ValidIssuer = authOptions.Issuer,

                        // ����� �� �������������� ����������� ������
                        ValidateAudience = true,
                        // ��������� ����������� ������
                        ValidAudience = authOptions.Audience,
                        // ����� �� �������������� ����� �������������
                        ValidateLifetime = true,

                        // ��������� ����� ������������
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        // ��������� ����� ������������
                        ValidateIssuerSigningKey = true,
                    };
                    options.EventsType = typeof(DefaultJwtBearerEvents);
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelAttribute));

                options.EnableEndpointRouting = false;
            });

            //AutoMapperConfig.Initialize();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICacheRepository cacheRepository)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();

            cacheRepository.Connect();

            app.UseMvc();
        }
    }
}

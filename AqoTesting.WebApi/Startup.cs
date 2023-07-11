using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AqoTesting.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AqoTesting.Shared.Infrastructure;
using AqoTesting.Core.Repositories;
using AqoTesting.WebApi.Infrastructure;
using MongoDB.Bson.Serialization;
using System;
using MongoDB.Bson.Serialization.Serializers;
using AqoTesting.WebApi.Attributes.CommonAPI;
using AqoTesting.Domain.Controllers;

namespace AqoTestingServer
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
            services.Configure<InternalApiConfig>(Configuration.GetSection("InternalApi"));

            services.AddScoped<IWorkContext, WorkContext>();
            services.AddScoped<DefaultJwtBearerEvents>();

            // Load services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IAttemptService, AttemptService>();
            services.AddScoped<IValidationService, ValidationService>();

            services.AddSingleton<ITokenGeneratorService, TokenGeneratorService>();

            // Load repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IAttemptRepository, AttemptRepository>();

            services.AddSingleton<ITokenRepository, TokenRepository>();

            services.AddCors();

            var mongoConnection = Configuration.GetSection("MongoConnection").Get<MongoConnectionConfig>();
            MongoController.ConnectToDB(
                mongoConnection.Username,
                mongoConnection.Password,
                mongoConnection.Host,
                mongoConnection.Port,
                mongoConnection.DefaultAuthDb,
                mongoConnection.Options );

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
                        IssuerSigningKey = authOptions.SymmetricSecurityKey,
                        // ��������� ����� ������������
                        ValidateIssuerSigningKey = true,
                    };
                    options.EventsType = typeof(DefaultJwtBearerEvents);
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CommonAPI_ValidateModelAttribute));

                options.EnableEndpointRouting = false;
            });

            AutoMapperConfig.Initialize();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();

            app.UseMvc();
        }
    }
}

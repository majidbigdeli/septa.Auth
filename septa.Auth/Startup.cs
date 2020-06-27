using AutoMapper;
using DNTCommon.Web.Core;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using septa.Auth.Domain;
using septa.Auth.Domain.Contexts;
using septa.Auth.Domain.Entities;
using septa.Auth.Domain.Hellper;
using septa.Auth.Domain.Mapper;
using septa.Auth.Domain.Services;
using septa.Auth.Domain.Settings;
using septa.Auth.Hellper;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace septa.Auth
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
            //services.AddControllers();
            services.Configure<SiteSettings>(options => Configuration.Bind(options));

            var siteSettings = services.GetSiteSettings();

            services.AddCustomIdentityServices(siteSettings);

            services.AddRequiredEfInternalServices(siteSettings);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap["UserId"] = JwtClaimTypes.Subject;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap["UserName"] = JwtClaimTypes.Name;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap["Role"] = JwtClaimTypes.Role;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap["Email"] = JwtClaimTypes.Email;

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "Bearer";
                o.DefaultScheme = "Bearer";
                o.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer("Bearer", options =>
            {
                options.Authority = siteSettings.Authority; //Configuration.GetSection("Authority").Value;
                options.RequireHttpsMetadata = false;
                options.Audience = "api.sample";
            });

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<User>()
                .AddProfileService<AspNetIdentityProfileService>()
                .AddResourceOwnerValidator<SeptaResourceOwnerPasswordValidator>()
                .AddClientStore<ClientStore>()
                .AddResourceStore<ResourceStore>()
                .AddCorsPolicyService<AbpCorsPolicyService>();
            // .AddExtensionGrantValidator<AuthenticationGrant>();

            services.Replace(ServiceDescriptor.Transient<IClaimsService, SeptaClaimsService>());

            services.AddDbContextPool<ApplicationDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.SetDbContextOptions(siteSettings);

                // It's added to access services from the dbcontext, remove it if you are using the
                // normal `AddDbContext` and normal constructor dependency injection.
                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });

            services.AddMvc(options =>
            {
                options.UseYeKeModelBinder();
                options.AllowEmptyInputInBodyModelBinding = true;

                // options.Filters.Add(new NoBrowserCacheAttribute());
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.IgnoreNullValues = true;
            }).AddControllersAsServices();


            services.AddSwaggerGen(  options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "PerformanceEvaluation API", Version = "v1" });
        options.DocInclusionPredicate((docName, description) => true);
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Description = "Swagger Api",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.OAuth2,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Flows = new Microsoft.OpenApi.Models.OpenApiOAuthFlows() { Password = new Microsoft.OpenApi.Models.OpenApiOAuthFlow() { TokenUrl = new Uri($"{siteSettings.Authority}/connect/token") } }
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
                        {
                         new OpenApiSecurityScheme
                            {
                             Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                             },
                             new string[] {}
                        }
        });
    });


            services.AddAutoMapper(
                typeof(ApiResourceMapperProfile),
                typeof(ClientMapperProfile),
                typeof(IdentityResourceMapperProfile),
                typeof(PersistedGrantMapperProfile),
                typeof(ScopeMapperProfile));


            services.AddDNTCommonWeb();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.ApplicationServices.InitializeDb();

         //   InitializeDatabase(app);

            app.UseCors("default");
            app.UseIdentityServer();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PerformanceEvaluation API");
                options.OAuthClientId("PerformanceEvaluation_App");
                options.OAuthClientSecret("1q2w3e*");

            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

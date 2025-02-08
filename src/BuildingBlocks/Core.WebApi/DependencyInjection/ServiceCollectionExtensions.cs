using Amazon.CognitoIdentityProvider;
using Core.Domain.Notificacoes;
using Core.WebApi.Configurations;
using Core.WebApi.GlobalErrorMiddleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.WebApi.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddApiDefautConfig(this IServiceCollection services, JwtBearerConfigureOptions jwtBearerConfigureOptions)
        {
            if (jwtBearerConfigureOptions == null)
            {
                throw new ArgumentNullException(nameof(jwtBearerConfigureOptions), "JWT Bearer Configuration Options cannot be null.");
            }

            services.AddScoped<INotificador, Notificador>();

            services.AddControllers().AddJsonOptions(delegate (JsonOptions options)
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            });

            services.AddSwaggerConfig();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = jwtBearerConfigureOptions.Authority;
                    opt.MetadataAddress = jwtBearerConfigureOptions.MetadataAddress;
                    opt.IncludeErrorDetails = true;
                    opt.RequireHttpsMetadata = false;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = "cognito:groups"
                    };

                    opt.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            // Extrair o "sub" (ID do usuário)
                            var userIdClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier) ??
                                              context.Principal?.FindFirst("sub");

                            if (userIdClaim is not null)
                            {
                                var identity = context.Principal.Identity as ClaimsIdentity;
                                identity?.AddClaim(new Claim("UserId", userIdClaim.Value));
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorizationBuilder()
                .AddPolicy("UsuarioRole", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("usuarios");
                });

            services.AddAWSService<IAmazonCognitoIdentityProvider>();

            services.AddScoped<IUserContextService, UserContextService>();

            services.AddHttpContextAccessor();
        }

        public static void UseApiDefautConfig(this IApplicationBuilder app)
        {
            app.UseApplicationErrorMiddleware();

            app.UseSwaggerConfig();

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
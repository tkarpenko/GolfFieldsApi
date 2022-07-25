using System.Reflection;
using Golf.Fields.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();



#region Connect swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.0", new OpenApiInfo() { Title = "Web API", Version = "v1.0" });
    // c.IncludeXmlComments(@"Golf.Fields.Api.xml");
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    // This call remove version from parameter, without it we will have version as parameter 
    // for all endpoints in swagger UI
    c.OperationFilter<RemoveVersionFromParameter>();

    // This make replacement of v{version:apiVersion} to real version of corresponding swagger doc.
    c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

    // This on used to exclude endpoint mapped to not specified in swagger version.
    // In this particular example we exclude 'GET /api/v2/Values/otherget/three' endpoint,
    // because it was mapped to v3 with attribute: MapToApiVersion("3")
    c.DocInclusionPredicate((version, desc) =>
    {
        if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

        if (methodInfo.DeclaringType != null)
        {
            var versions = methodInfo.DeclaringType
            .GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(attr => attr.Versions);


            var maps = methodInfo
                .GetCustomAttributes(true)
                .OfType<MapToApiVersionAttribute>()
                .SelectMany(attr => attr.Versions)
                .ToArray();

            return versions.Any(v => $"v{v}" == version)
                && (!maps.Any() || maps.Any(v => $"v{v}" == version));
        }

        return false;
    });
});

#endregion



builder.Services.AddApiVersioning(c =>
{
    c.RouteConstraintName = "ApiVersion";
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Audience = "api1";
    //options.Authority = "http://localhost:5030";
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "Golf",
        ValidAudience = "Golf",
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("test")) //SecurityServices.JWT_KEY))
    };
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "V1 Docs");

    });
}

app.UseApiVersioning();

app.UseRouting();

app.UseAuthentication(); // UseAuthentication before UseAuthorization

app.UseAuthorization();

// Configure the HTTP request pipeline.

//app.UseAuthorization();

//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();


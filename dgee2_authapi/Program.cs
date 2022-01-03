using dgee2_authapi;
using dgee2_authapi.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme."
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "bearer"
						}
					},
					Array.Empty<string>()
				}
			});
});

builder.Services.AddCors(o =>
{
	o.AddDefaultPolicy(
		builder => builder.WithOrigins("http://localhost:8080").AllowAnyHeader());
});

builder.Services.Configure<JwtConfig>(builder.Configuration.GetRequiredSection(JwtConfig.ConfigName));

builder.Services.AddTransient<JwtHelpers>();

builder.Services.AddTransient<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("primary-token", o => { })
.AddJwtBearer("refresh-token", o => { });

builder.Services.AddAuthorization(options =>
{
	options.DefaultPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.AddAuthenticationSchemes("primary-token")
		.Build();

	options.AddPolicy("refresh-token", new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.AddAuthenticationSchemes("refresh-token")
		.Build());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(o =>
	{
		o.SwaggerEndpoint("v1/swagger.yaml", "dgee2_authapi V1");
		o.DisplayRequestDuration();
		o.EnableTryItOutByDefault();
	});
}

app.UseHttpsRedirection();
app.UseAuthentication().UseAuthorization();

app.UseCors();

app.MapPost("/auth/generate-token", (JwtHelpers jwt) =>
{
	return jwt.GenerateTokens(2543, "dan@dan-gee.co.uk", "geed");
});

app.MapPost("/auth/refresh-token", (JwtHelpers jwt, ClaimsPrincipal user) =>
 {
	 return jwt.GenerateTokens(2543, "dan@dan-gee.co.uk", "geed");
 }).RequireAuthorization("refresh-token");

app.Run();

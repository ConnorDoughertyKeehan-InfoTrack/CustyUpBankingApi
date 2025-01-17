using CustyUpBankingApi.Services;
using CustyUpBankingApi.Contexts;
using Microsoft.EntityFrameworkCore;
using OpenAI.Extensions;
using CustyUpBankingApi.Profiles;
using CustyUpBankingApi.Repo;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using CustyUpBankingApi.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using CustyUpBankingApi.Services.Interfaces;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin() // Allow all origins
               .AllowAnyMethod() // Allow all HTTP methods (GET, POST, etc.)
               .AllowAnyHeader(); // Allow all headers
    });
});

//configure api keys custom middleware
builder.Services.AddAuthentication("ApiKeyScheme")
	   .AddScheme<AuthenticationSchemeOptions, AuthMiddleware>("ApiKeyScheme", null);

builder.Services.AddControllers(config =>
{
	// Enforce authentication by default
	var policy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();
	config.Filters.Add(new AuthorizeFilter(policy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Add swagger gen with apikey
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

	c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please insert API key into the field",
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "ApiKey"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "ApiKey"
				},
				Scheme = "ApiKey",
				Name = "Authorization",
				In = ParameterLocation.Header
			},
			new string[] { }
		}
	});
});

// Register your DbContext
builder.Services.AddDbContext<MegaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MegaDb")));

//Dependency Injection
builder.Services.AddTransient<IAiAdviceService, AiAdviceService>();
builder.Services.AddTransient<IUpBankingService, UpBankingService>();
builder.Services.AddTransient<IBudgetService, BudgetService>();
builder.Services.AddTransient<IAiService, AiService>();
builder.Services.AddTransient<IMegaDbRepo, MegaDbRepo>();
builder.Services.AddOpenAIService(settings => { settings.ApiKey = builder.Configuration["OpenAIServiceOptions:ApiKey"]; });
builder.Services.AddAutoMapper(typeof(MegaMappingProfile));

var app = builder.Build();

//User Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Enable CORS globally
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

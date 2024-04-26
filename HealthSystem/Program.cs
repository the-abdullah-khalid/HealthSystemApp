using HealthSystemApp;
using HealthSystemApp.CustomActionFilters.Authorization;
using HealthSystemApp.CustomMiddlewares.Authorization;
using HealthSystemApp.Data;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using HealthSystemApp.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//add auth to swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});



builder.Services.AddDbContext<HealthSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HealthSystemConnectionString"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.MigrationsHistoryTable(
            tableName: HistoryRepository.DefaultTableName,
            schema: "healthDB");
    }));

builder.Services.AddDbContext<HealthSystemAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HealthSystemConnectionString"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.MigrationsHistoryTable(
            tableName: HistoryRepository.DefaultTableName,
        schema: "authDB");
    }));

//injecting the inherited IAuthorizationHandler
builder.Services.AddTransient<IAuthorizationHandler, HierarchicalAccessControl>();
builder.Services.AddHttpContextAccessor();


//dependency injection for repository HealthSystem
builder.Services.AddScoped<IHealthSystem, HealthSystemRepository>();

//dependency injection for repository HealthRegion
builder.Services.AddScoped<IHealthRegion, HealthRegionRepository>();

//dependency injection for repository Organization
builder.Services.AddScoped<IOrganization, OrganizationRepository>();

//dependency injection for repository token
builder.Services.AddScoped<IToken, TokenRepository>();

//dependency injection for AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("HealthSystem")
    .AddEntityFrameworkStores<HealthSystemAuthDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

//setting JWT 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });
//builder.Services.AddAuthorization();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdministratorOnly", policy =>
           policy.AddRequirements(new RoleRequirement(new List<string> { "Administrator" })).Build());
    options.AddPolicy("AdministratorOrHealthSystemAdmin", policy =>
           policy.AddRequirements(new RoleRequirement(new List<string> { "Administrator", "HealthSystemAdmin" })).Build());
    options.AddPolicy("AdministratorOrHealthSystemAdminOrHealthRegionAdmin", policy =>
           policy.AddRequirements(new RoleRequirement(new List<string> { "Administrator", "HealthSystemAdmin", "RegionAdmin" })).Build());
    options.AddPolicy("AdministratorOrHealthSystemAdminOrHealthRegionAdminOrOrganizationAdmin", policy =>
       policy.AddRequirements(new RoleRequirement(new List<string> { "Administrator", "HealthSystemAdmin", "RegionAdmin", "OrganizationAdmin" })).Build());

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

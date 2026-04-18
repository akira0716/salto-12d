using LoanManagement.Application.Interfaces;
using LoanManagement.Application.Services;
using LoanManagement.Domain.Repositories;
using LoanManagement.Infrastructure.Authentication;
using LoanManagement.Infrastructure.Persistence;
using LoanManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers add
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// CORS Setting
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Swagger JWT Config
builder.Services.AddSwaggerGen();

// DbContext configuration mapping to SQLite
builder.Services.AddDbContext<LoanDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

var jwtOptionsSection = builder.Configuration.GetSection(JwtOptions.SectionName);
var jwtOptions = jwtOptionsSection.Get<JwtOptions>() ?? new JwtOptions();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
        };
    });

// Dependency Injection

// Repositories (Infrastructure)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEquipmentCategoryRepository, EquipmentCategoryRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<ILoanRequestRepository, LoanRequestRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Domain Services
builder.Services.AddScoped<LoanManagement.Domain.Services.LoanPolicyService>();

// AppServices (Application)
builder.Services.AddScoped<IAuthAppService, AuthAppService>();
builder.Services.AddScoped<IEquipmentCategoryAppService, EquipmentCategoryAppService>();
builder.Services.AddScoped<IEquipmentAppService, EquipmentAppService>();
builder.Services.AddScoped<ILoanRequestAppService, LoanRequestAppService>();
builder.Services.AddScoped<ILoanAppService, LoanAppService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowViteApp");

// Use Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

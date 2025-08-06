using AuthenticationManager;
using MessageBrokerManager.Extensions;
using Microsoft.EntityFrameworkCore;
using RequestFeatureShared.SortHelper;
using UserAuthenticationService.Data;
using UserAuthenticationService.MappingConfiguration;
using UserAuthenticationService.Models;
using UserAuthenticationService.Repositories;
using UserAuthenticationService.Services;
using UserAuthenticationService.Utils;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddJwtAuthenticationConfigurations();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<AuthenticationTokenProvider>();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.AddSingleton<ISortHelper<User>, SortHelper<User>>();
builder.Services.AddMassTransitConfigurations(builder.Configuration);
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("UserDb"))
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

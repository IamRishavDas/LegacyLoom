using AuthenticationManager;
using LegacyLoomApplicationGateway.Extensions;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ocelot configuration
builder.Services.AddJwtAuthenticationConfigurations();
builder.Configuration.AddJsonFilesForOcelotConfig();
builder.Services.AddOcelotConfig(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);

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

app.UseCors(builder.Configuration["Cors:Policy"] ?? throw new ArgumentNullException("Cors:Policy not found!"));  

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

app.Run();

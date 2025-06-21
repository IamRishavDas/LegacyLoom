using AuthenticationManager;
using RequestFeatureShared.SortHelper;
using TimelineService.Extensions;
using TimelineService.MappingConfiguration;
using TimelineService.Models;
using TimelineService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddJwtAuthenticationConfigurations();
builder.Services.AddAutoMapper(typeof(ApplicationMappingConfig));
builder.Services.LoadMongoDbSettings(builder.Configuration);
builder.Services.CreateMongoClientInstance(builder.Configuration);
builder.Services.AddSingleton<ISortHelper<Timeline>, SortHelper<Timeline>>();
builder.Services.AddScoped<ITimelineService, TimelineService.Services.TimelineService>();

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

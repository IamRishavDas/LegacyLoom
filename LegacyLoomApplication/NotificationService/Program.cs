using AuthenticationManager;
using MongoDB.Driver;
using NotificationService.ConsumerRegistrationExtension;
using NotificationService.EmailTemplates;
using NotificationService.MappingConfiguration;
using NotificationService.Models;
using NotificationService.Services;
using RequestFeatureShared.SortHelper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddJwtAuthenticationConfigurations();
builder.Services.AddSingleton<Templates>();
builder.Services.AddAutoMapper(typeof(NotificationMapper));
builder.Services.AddScoped<INotificationSender, NotificationSender>();
builder.Services.AddScoped<INotificationService, NotificationService.Services.NotificationService>();
builder.Services.AddSingleton<ISortHelper<Notification>, SortHelper<Notification>>();
builder.Services.AddMassTransitRegistrationForConsumer(builder.Configuration);
builder.Services.Configure<NotificationDbSettings>(
        builder.Configuration.GetSection("NotificationDbSettings")
    );
builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var connectionString =
        builder
            .Configuration
            .GetSection("NotificationDbSettings:ConnectionString")?
            .Value;

    return new MongoClient(connectionString);
});
builder.Services.AddGrpc();

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

app.MapGrpcService<NotificationSender>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

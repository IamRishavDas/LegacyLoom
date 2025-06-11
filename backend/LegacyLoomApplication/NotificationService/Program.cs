using AuthenticationManager;
using NotificationService.ConsumerRegistrationExtension;
using NotificationService.EmailTemplates;
using NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddJwtAuthenticationConfigurations();
builder.Services.AddSingleton<Templates>();
builder.Services.AddScoped<INotificationSender, NotificationSender>();
builder.Services.AddMassTransitRegistrationForConsumer();

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

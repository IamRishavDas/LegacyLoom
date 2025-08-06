using AuthenticationManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using RequestFeatureShared.SortHelper;
using TimelineService.Extensions;
using TimelineService.MappingConfiguration;
using TimelineService.Models;
using TimelineService.MongoRepository;
using TimelineService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddJwtAuthenticationConfigurations();
builder.Services.AddAutoMapper(typeof(ApplicationMappingConfig));
builder.Services.LoadMongoDbSettings(builder.Configuration);
builder.Services.CreateMongoClientInstance(builder.Configuration);
builder.Services.AddSingleton<AppMongoRepository>();
builder.Services.AddCloudinaryServiceAsSingleton(builder.Configuration);
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddSingleton<ISortHelper<Timeline>, SortHelper<Timeline>>();
builder.Services.AddScoped<ITimelineService, TimelineService.Services.TimelineService>();
builder.Services.AddScoped<IStoryService, StoryService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(setup =>
//{
//    var jwtSecurityScheme = new OpenApiSecurityScheme
//    {
//        BearerFormat = "JWT",
//        Name = "JWT Authentication",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = JwtBearerDefaults.AuthenticationScheme,
//        Description = "Put only your jwt bearer token",

//        Reference = new OpenApiReference()
//        {
//            Id = JwtBearerDefaults.AuthenticationScheme,
//            Type = ReferenceType.SecurityScheme
//        }
//    };

//    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
//    setup.AddSecurityRequirement(new OpenApiSecurityRequirement()
//    {
//        { jwtSecurityScheme, Array.Empty<string>() }
//    });
//});

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

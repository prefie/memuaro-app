using System.Text;
using Rememory.Auth;
using Rememory.Auth.Settings;
using Rememory.Persistance;
using Rememory.Persistance.Client;
using Rememory.Persistance.Repositories.CategoryRepository;
using Rememory.Persistance.Repositories.GlobalQuestionRepository;
using Rememory.Persistance.Repositories.QuestionRepository;
using Rememory.Persistance.Repositories.UserRepository;
using Rememory.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Rememory.Bot;
using Rememory.Bot.Settings;
using Rememory.Email;
using Rememory.Email.Settings;
using Rememory.Persistance.Repositories.NotificationSettingsRepository;
using Rememory.Persistance.Repositories.UserSettingsRepository;
using Rememory.WebApi.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection("DatabaseConfig"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleSettings"));
builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddSingleton<IDatabaseClient, DatabaseClient>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IQuestionRepository, QuestionRepository>();
builder.Services.AddSingleton<IGlobalQuestionRepository, GlobalQuestionRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<INotificationSettingsRepository, NotificationSettingsRepository>();
builder.Services.AddSingleton<IUserSettingsRepository, UserSettingsRepository>();
builder.Services.AddSingleton<IBot, TelegramBot>();
builder.Services.AddSingleton<IEmailClient, EmailClient>();
builder.Services.AddSingleton<AuthProvider>();
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SecurityKey"))),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddHostedService<NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(); // Временно используем всегда

app.UseMiddleware<CustomExceptionHandlerMiddleware>();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
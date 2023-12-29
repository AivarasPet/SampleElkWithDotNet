using InquiryAPI.BaseDbContexts;
using InquiryAPI.Configuration;
using InquiryAPI.Repositories;
using InquiryAPI.Services.InquiriesService;
using InquiryAPI.Services.UserService;
using InquiryAPI.Services.UserTokenService;
using InquiryStatusUpdater.SignalRHubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, SignalRUserIdProvider>();

builder.Services.AddCors(options => options.AddDefaultPolicy(b =>
{
    b.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

builder.Services.Configure<AppConfiguration>(builder.Configuration);
AppConfiguration config = builder.Configuration.Get<AppConfiguration>();
builder.Services.AddSingleton<AppConfiguration>(config);

var key = Encoding.ASCII.GetBytes(config.JwtSecretKey);

//copy the same auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (config.SignalRConfiguration.SignalRConnectionPath != null)
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments(config.SignalRConfiguration.SignalRConnectionPath))
                {
                    context.Token = accessToken;
                }
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddDbContext<InquiryDbContext>(options =>
    options.UseNpgsql(config.ConnectionString));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IInquiriesRepository, InquiriesRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IInquiriesService, InquiriesService>();
builder.Services.AddScoped<IUserTokenService, UserTokenService>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // Configure SignalR endpoints
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.Run();

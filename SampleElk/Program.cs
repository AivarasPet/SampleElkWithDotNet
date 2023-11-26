using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

configureLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void configureLogging()
{
    string? enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    IConfigurationBuilder configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        .AddJsonFile($"appsettings.{enviroment}.json", optional: false);

    ElasticsearchSinkOptions elasicsearchOptions = ConfigureElasticSink(configuration.Build(), enviroment);
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(elasicsearchOptions)
        .Enrich.WithProperty("Environment", enviroment)
        .ReadFrom.Configuration(configuration.Build())
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot config, string environment)
{
    Uri uri = new Uri(config["ElasticConfiguration:Uri"]);
    return new ElasticsearchSinkOptions(uri)
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.Replace(".", "-")}-{environment.ToLower()}",
        NumberOfReplicas = 1,
        NumberOfShards = 2
    };
}
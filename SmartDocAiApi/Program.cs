using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartDocAiApi.Models;
using SmartDocAiApi.Repositories;
using SmartDocAiApi.Repositories.impl;
using SmartDocAiApi.Services;
using SmartDocAiApi.Services.impl;
using SmartDocAiApi.Swagger;

DotNetEnv.Env.Load();
// DotNetEnv.Env.Load(".env.production.local");

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";


builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "SmartDoc Api", Version = "v1" });

    // Enable support for form-data requests (for IFormFile)
    options.OperationFilter<FileUploadOperationFilter>();
});

// Configuration
builder.Services.Configure<AzureBlobSettings>(
    builder.Configuration.GetSection("AzureBlobSettings"));

// Register BlobServiceClient as singleton
builder.Services.AddSingleton(sp =>
{

    // Retrieve the configured AzureBlobSettings
    var blobSettings = sp.GetRequiredService<IOptions<AzureBlobSettings>>().Value;

    // Log diagnostic info (visible in Azure Log Stream)
    Console.WriteLine($"[DIAGNOSTIC] Blob ConnectionString: {(string.IsNullOrWhiteSpace(blobSettings.ConnectionString) ? "<empty>" : "set")}");
    Console.WriteLine($"[DIAGNOSTIC] Blob ContainerName: {blobSettings.ContainerName}");

    if (string.IsNullOrWhiteSpace(blobSettings.ConnectionString))
    {
        throw new InvalidOperationException("Azure Blob Storage ConnectionString is null or empty. Check Azure environment variables or appsettings.");
    }

    return new BlobServiceClient(blobSettings.ConnectionString);
});

// PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// DI
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "SmartDoc API is running");

app.Run();
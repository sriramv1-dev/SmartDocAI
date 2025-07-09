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

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";


builder.Configuration
    // .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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

builder.Services.AddSingleton(sp =>
{
    // var blobSettings = sp.GetRequiredService<IOptions<AzureBlobSettings>>().Value;
    // return new BlobServiceClient(blobSettings.ConnectionString);

    // Retrieve the configured AzureBlobSettings
    var blobSettings = sp.GetRequiredService<IOptions<AzureBlobSettings>>().Value;

    // --- DIAGNOSTIC START: Check AzureBlobSettings object after binding ---
    Console.WriteLine($"--- DIAGNOSTIC: From IOptions<AzureBlobSettings> - ConnectionString: '{blobSettings.ConnectionString}' ---");
    Console.WriteLine($"--- DIAGNOSTIC: From IOptions<AzureBlobSettings> - ContainerName: '{blobSettings.ContainerName}' ---");
    // --- DIAGNOSTIC END ---

    // This is the line that's failing if blobSettings.ConnectionString is null
    // Add a null check here to provide a more specific error if needed
    if (string.IsNullOrEmpty(blobSettings.ConnectionString))
    {
        throw new InvalidOperationException("Azure Blob Storage ConnectionString is null or empty after configuration binding. Check environment variables or appsettings.json.");
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
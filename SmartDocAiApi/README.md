# SmartDocAI Backend

This project is a .NET Core Web API for handling document uploads, extracting text (OCR), storing metadata in PostgreSQL, and enabling future AI-based Q&A capabilities. It is designed for both local development and production deployment on Azure.

---

## 🔧 Tech Stack

- ASP.NET Core Web API
- PostgreSQL (Supabase locally / Azure Flexible Server in prod)
- Azure Blob Storage
- Swagger for API testing
- EF Core for data access
- Tesseract OCR (coming soon)

## 🚀 Features

- Upload PDF/image files
- Save to Azure Blob Storage
- Save metadata (file name, blob URL, timestamp) in PostgreSQL
- Extract text via OCR (coming soon)
- Full environment separation (local/dev/prod)

# SmartDocAI - Backend Setup Guide

This guide helps you fully set up the SmartDocAI .NET API project from scratch, for both development and production environments.

---

## 📁 Project Structure

```
SmartDocAI/
├── Controllers/
├── Models/
├── Repositories/
├── Services/
├── Swagger/
├── appsettings.json
├── appsettings.Development.json
├── .env
└── Program.cs
```

---

## ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- PostgreSQL (local or cloud - e.g., Azure Database for PostgreSQL)
- Azure Blob Storage (with private access)

---

## ⚙️ 1. Clone the Repository

```bash
git clone https://github.com/yourusername/SmartDocAI.git
cd SmartDocAI/SmartDocAiApi
```

---

## 🗂️ 2. Environment Variables Setup

Create a `.env` file in the `SmartDocAiApi/` folder:

```
AzureBlobSettings__ConnectionString=DefaultEndpointsProtocol=https;AccountName=smartdocstoragesriram;AccountKey=your-key;EndpointSuffix=core.windows.net
AzureBlobSettings__ContainerName=smartdoc-files-dev
ConnectionStrings__PostgresConnection=Host=your-host;Database=your-db;Username=your-username;Password=your-password;SslMode=Require;Trust Server Certificate=true;
ASPNETCORE_ENVIRONMENT=Development
```

> ⚠️ Replace values with your real credentials.

---

## 🧠 3. Configuration Loading

In `Program.cs`, configuration is loaded in this order:

```csharp
DotNetEnv.Env.Load();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
```

---

## 🗄️ 4. PostgreSQL Setup

### Option A: Local PostgreSQL

- Install [Postgres locally](https://www.postgresql.org/download/)
- Create a database: `smartdoc`

Update your `.env`:

```
ConnectionStrings__PostgresConnection=Host=localhost;Port=5432;Database=smartdoc;Username=postgres;Password=yourpassword
```

### Option B: Azure PostgreSQL Flexible Server (Prod)

- Go to [Azure Portal](https://portal.azure.com)
- Create PostgreSQL Flexible Server (free tier available)
- Enable SSL & access from your IP
- Use connection string in `.env` (see above)

> ℹ️ Use **Service-managed key** and **PostgreSQL authentication only**.

---

## ☁️ 5. Azure Blob Storage Setup

- Create a **Storage Account** in Azure.
- Add a **container**: `smartdoc-files-dev`
- Make sure **public access is disabled** (default).

> ❗ In code, ensure container creation does not request public access:

```csharp
_containerClient.CreateIfNotExists(PublicAccessType.None);
```

---

## 🚀 6. Run the Project

```bash
dotnet build
dotnet run
```

Open Swagger UI:

```
http://localhost:5000/swagger
```

Use the file upload endpoint:

```http
POST /api/upload
FormData: file = (your PDF/Doc file)
```

---

## 🧪 7. Debugging Tips

- Log values in `Program.cs` to validate environment bindings.
- If `BlobServiceClient` throws a format error, inspect the `.env` and env vars.
- If Swagger throws on file upload, ensure `FileUploadOperationFilter` is applied.

---

## 🛠️ 8. Additional Notes

- Avoid hardcoding secrets; use `.env` and `DotNetEnv`.
- Don't commit `.env` to version control.
- To switch environments, change `ASPNETCORE_ENVIRONMENT` to `Production` or `Development`.

---

## 📦 Dependencies Used

- `Swashbuckle.AspNetCore`
- `Azure.Storage.Blobs`
- `Microsoft.EntityFrameworkCore`
- `DotNetEnv`

---

## ✅ You’re all set!

This README serves as the single source of truth to set up, configure, and deploy your SmartDocAI backend for dev and prod.

Let me know if you'd like Docker support or CI/CD pipeline setup next!

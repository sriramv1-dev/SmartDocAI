using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using SmartDocAiApi.Models;

namespace SmartDocAiApi.Services.impl
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IOptions<AzureBlobSettings> options)
        {
            var settings = options.Value;
            Console.WriteLine("Using Blob ConnectionString: " + settings.ConnectionString);

            var blobServiceClient = new BlobServiceClient(settings.ConnectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(settings.ContainerName);
            // _containerClient.CreateIfNotExists(PublicAccessType.Blob);
            _containerClient.CreateIfNotExists(PublicAccessType.None);
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(file.FileName);
                await using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, overwrite: true);

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file to blob storage: {ex.Message}");
                throw;
            }
        }

    }
}
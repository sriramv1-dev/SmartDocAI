namespace SmartDocAiApi.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
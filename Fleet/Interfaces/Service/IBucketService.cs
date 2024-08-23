namespace Fleet.Interfaces.Service
{
    public interface IBucketService
    {
        Task<string> UploadAsync(Stream stream, string fileExtension);
        Task DeleteAsync(string filename);
    }
}

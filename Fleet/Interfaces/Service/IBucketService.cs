namespace Fleet.Interfaces.Service
{
    public interface IBucketService
    {
        Task<string> UploadAsync(Stream stream, string fileExtension, string folder);
        Task DeleteAsync(string filename, string folder);
    }
}

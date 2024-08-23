using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Fleet.Interfaces.Service;

namespace Fleet.Service
{
    public class BlobService : IBucketService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly string connectionString = string.Empty;
        private readonly string containerName = string.Empty;

        public BlobService(IConfiguration configuration)
        {
            connectionString = configuration["AzureBlobStorage:ConnectionString"];
            containerName = configuration["AzureBlobStorage:UserContainerName"];

            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadAsync(Stream stream, string fileExtension)
        {
            var filename = $"{Guid.NewGuid().ToString()}.{fileExtension}";
            BlobClient blobClient = _containerClient.GetBlobClient(filename);
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = $"image/{fileExtension}" });

            return filename;
        }

        public async Task DeleteAsync(string filename)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(filename);

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient cont = blobServiceClient.GetBlobContainerClient(containerName);
            await cont.GetBlobClient(filename).DeleteIfExistsAsync();
        }
    }
}

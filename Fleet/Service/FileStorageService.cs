using Fleet.Interfaces.Service;

namespace Fleet.Service
{
    public class FileStorageService : IBucketService
    {
        public async Task DeleteAsync(string filename)
        {
            var filepath = $"{AppContext.BaseDirectory}\\imagens\\pefil\\{filename}";
            await Task.Run(() =>
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
            });
        }

        public async Task<string> UploadAsync(Stream stream, string fileExtension)
        {
            var filename = $"{Guid.NewGuid().ToString()}.{fileExtension}";
            var filepath = $"{AppContext.BaseDirectory}\\imagens\\pefil";

            await Task.Run(() =>
            {
                if(!File.Exists(filepath)) Directory.CreateDirectory(filepath);

                Stream file = File.Create($"{filepath}\\{filename}");
                stream.CopyTo(file);
            });

            return filename;
        }
    }
}

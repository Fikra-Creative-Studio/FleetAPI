using Fleet.Interfaces.Service;

namespace Fleet.Service
{
    public class FileStorageService : IBucketService
    {
        public async Task DeleteAsync(string filename)
        {
            var filepath = $"{AppContext.BaseDirectory}\\images\\profile\\{filename}";
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
            var filepath = $"{AppContext.BaseDirectory}\\images\\profile";

            await Task.Run(() =>
            {
                if(!File.Exists(filepath)) Directory.CreateDirectory(filepath);

                using (var file = File.Create($"{filepath}\\{filename}"))
                {
                    stream.CopyTo(file);
                }
            });

            return filename;
        }
    }
}

using System.Text.Json;
using SoftSpot_Hein_Myat_Thu.Models;

namespace SoftSpot_Hein_Myat_Thu.Services
{
    public class StorageService : IStorageService 
    {
        private string GetFilePath(string fileName)  // get the file path in the app data directory for the given file name
        {
            return Path.Combine(FileSystem.AppDataDirectory, fileName);
        }

        public async Task<T?> LoadAsync<T>(string fileName) // loads data of type T from a JSON file. 
        {
            var path = GetFilePath(fileName);

            if (!File.Exists(path))
            {
                return default;
            }

            var json = await File.ReadAllTextAsync(path); // read the JSON content from the file
            return JsonSerializer.Deserialize<T>(json); // deserialize the JSON content into an object of type T and return it
        }

        public async Task SaveAsync<T>(string fileName, T data) // saves data of type T to a JSON file
        {
            var path = GetFilePath(fileName);
            var json = JsonSerializer.Serialize(data); // serialize the data object into a JSON string
            await File.WriteAllTextAsync(path, json);

        }
    }
}
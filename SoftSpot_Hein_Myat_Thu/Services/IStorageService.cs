using SoftSpot_Hein_Myat_Thu.Models;

namespace SoftSpot_Hein_Myat_Thu.Services
{
    public interface IStorageService
    {
        Task<T?> LoadAsync<T>(string fileName);
        Task SaveAsync<T>(string fileName, T data);

    }
}
using SoftSpot_Hein_Myat_Thu.Models;

namespace SoftSpot_Hein_Myat_Thu.Services
{
    public interface IStorageService
    {
        Task<List<Place>> GetAllPlacesAsync();
        Task SavePlacesAsync(List<Place> places);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task SaveNotificationsAsync(List<Notification> notifications);

    }
}
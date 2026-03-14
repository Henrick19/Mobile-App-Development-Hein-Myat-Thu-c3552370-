using SoftSpot_Hein_Myat_Thu.Models;

namespace SoftSpot_Hein_Myat_Thu.Services
{
    public interface IPlaceService
    {
        Task<List<Place>> GetAllPlacesAsync(); 
        Task AddAsync(Place place);

    }
}
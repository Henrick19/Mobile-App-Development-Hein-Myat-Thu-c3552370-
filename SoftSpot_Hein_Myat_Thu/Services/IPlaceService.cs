using SoftSpot_Hein_Myat_Thu.Models;

namespace SoftSpot_Hein_Myat_Thu.Services;

public interface IPlaceService
{
    Task<List<Place>> GetPlacesAsync();
    Task AddAsync(Place place);
    Task AddToFavouriteAsync(Place place);
    Task RemoveFromFavouriteAsync(Place place);

    Task ClearAllFavouritesAsync();
    Task SetNotifyWhenQuietAsync(Place place, bool value);


}

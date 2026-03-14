using SoftSpot_Hein_Myat_Thu.Models;

namespace SoftSpot_Hein_Myat_Thu.Services;

public class PlaceService : IPlaceService
{
    private readonly IStorageService _storageService;
    private const string FileName = "places.json";

    public PlaceService(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<List<Place>> GetAllPlacesAsync() // This method retrieves all places from storage. If no places are found, it seeds the storage with default places.
    {
        var places = await _storageService.LoadAsync<List<Place>>(FileName); 

        if (places == null || places.Count == 0) 
        {
            places = GetSeedPlaces();
            await _storageService.SaveAsync(FileName, places);
        }
        return places;
    }

    public async Task AddAsync(Place place) // This method adds a new place to the existing list of places and saves it back to storage.
    {
        var places = await GetAllPlacesAsync();
        places.Add(place);
        await _storageService.SaveAsync(FileName, places);
    }

    private static List<Place> GetSeedPlaces() // This method creates a list of default places to be used when the storage is empty. 
    {
        List<Place> places = new List<Place>();

        Place place1 = new Place();
        place1.Name = "NUS Central Library";
        place1.LocationLink = "https://maps.google.com/?q=NUS+Central+Library";
        place1.Type = "Library";
        place1.NoiseLevel = NoiseLevel.Low;
        place1.CrowdLevel = CrowdLevel.Medium;
        place1.HasWifi = true;
        place1.Rating = 5;
        place1.BestTime = "6pm - 9pm";
        place1.Description = "Quiet study space with lots of desks and power sockets.";
        place1.IsFavorite = false;

        places.Add(place1);


        Place place2 = new Place();
        place2.Name = "Bishan Public Library";
        place2.LocationLink = "https://maps.google.com/?q=Bishan+Public+Library";
        place2.Type = "Library";
        place2.NoiseLevel = NoiseLevel.Low;
        place2.CrowdLevel = CrowdLevel.Medium;
        place2.HasWifi = true;
        place2.Rating = 4;
        place2.BestTime = "9am - 12pm";
        place2.Description = "Bright library with large windows and quiet corners.";
        place2.IsFavorite = true;

        places.Add(place2);


        Place place3 = new Place();
        place3.Name = "Tampines Hub Study Area";
        place3.LocationLink = "https://maps.google.com/?q=Tampines+Hub+Study+Area";
        place3.Type = "Study Area";
        place3.NoiseLevel = NoiseLevel.Medium;
        place3.CrowdLevel = CrowdLevel.High;
        place3.HasWifi = true;
        place3.Rating = 3;
        place3.BestTime = "8pm - 11pm";
        place3.Description = "Open study area, best during late evenings when it is quieter.";
        place3.IsFavorite = false;

        places.Add(place3);

        return places;
    }
}

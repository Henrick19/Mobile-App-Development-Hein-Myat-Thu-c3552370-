using System.Text.Json.Serialization;
namespace SoftSpot_Hein_Myat_Thu.Models;

public class Place
{
    public string Name { get; set; }
    public string LocationLink { get; set; }
    public string Type { get; set; }
    public NoiseLevel NoiseLevel { get; set; }
    public CrowdLevel CrowdLevel { get; set; }
    public bool HasWifi { get; set; }
    public string BestTime { get; set; }
    public string Description { get; set; }
    public bool IsFavorite { get; set; }
    public bool NotifyWhenQuiet { get; set; }
    public int Rating { get; set; } = 3;

    // This property is used to display the star rating in the UI, but it is not serialized to JSON

    [JsonIgnore]
    public int DisplayStarCount
    {
        get {  return Rating; }
    }

}





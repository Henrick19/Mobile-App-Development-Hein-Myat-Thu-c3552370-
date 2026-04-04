
namespace SoftSpot_Hein_Myat_Thu.Models;

public class Place
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // special id
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

  
    public int DisplayStarCount
    {
        get {  return Rating; }
    }

    // filled and unfilled dots for noise and crowd level for detail page
    

    public int FilledNoiseDots
    {
        get
        {
            int value = (int)NoiseLevel;

            value += 2;

            return value;
        }
    }


    public int FilledCrowdDots
    {
        get
        {
            int value = (int)CrowdLevel;

            value = value + 2;

            return value;
        }
    }

}





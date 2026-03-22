using System.Text.Json.Serialization;

namespace SoftSpot_Hein_Myat_Thu.Models;

public enum NotificationType
{
    NewPlaceAlert,
    NotifyWhenQuiet,
    AddedToFavourite,
    RemovedFromFavourite,
    ClearedFavourites,
}

public class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Message {  get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }
    public NotificationType Type { get; set; }
    public bool ShowUnreadDot // dot for unread msg
    {
        get
        {
            return !IsRead;
        }
    }

}
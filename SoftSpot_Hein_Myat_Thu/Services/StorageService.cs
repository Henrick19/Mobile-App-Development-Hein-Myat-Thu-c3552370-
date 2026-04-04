using SQLite;
using SoftSpot_Hein_Myat_Thu.Models;

namespace SoftSpot_Hein_Myat_Thu.Services;

public class StorageService : IStorageService
{
    private const string DatabaseFileName = "softspot.db3";

    private readonly SQLiteAsyncConnection _db; // async connection

  
    private bool _initialized;

    public StorageService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
        _db = new SQLiteAsyncConnection(dbPath);
    }

    private async Task EnsureDBInitializedAsync()
    {
        if (_initialized)
        {
            return;
        }

        await _db.CreateTableAsync<Place>();
        await _db.CreateTableAsync<Notification>();
        _initialized = true;   
        
    }

    // get all places from db
    public async Task<List<Place>> GetAllPlacesAsync()
    {
        await EnsureDBInitializedAsync();
        return await _db.Table<Place>().ToListAsync(); // get all places from db
    }

    public async Task SavePlacesAsync(List<Place> places)
    {
        await EnsureDBInitializedAsync();

        await _db.DeleteAllAsync<Place>(); // remove existing places to avoid duplication

        if (places.Count > 0)
        {
            await _db.InsertAllAsync(places); // insert new places into db
        }
    }

    // get all notifications from db
    public async Task<List<Notification>> GetAllNotificationsAsync()
    {
        await EnsureDBInitializedAsync();

        var list = await _db.Table<Notification>().ToListAsync(); // get all notifications from db

        // ensure every noti has an ID
        foreach (var noti in list)
        {
            if (string.IsNullOrEmpty(noti.Id))
            {
                noti.Id = Guid.NewGuid().ToString(); // assign new ID if missing
            }
        }
        return list;
    }

    // save notifications to db
    public async Task SaveNotificationsAsync(List<Notification> notifications)
    {
        await EnsureDBInitializedAsync();

        await _db.DeleteAllAsync<Notification>(); // remove existing notifications to avoid duplication
        if (notifications.Count > 0)
        {
            await _db.InsertAllAsync(notifications); // insert new notifications into db
        }
    }

    
}
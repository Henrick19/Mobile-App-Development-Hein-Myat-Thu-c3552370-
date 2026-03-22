using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

using SoftSpot_Hein_Myat_Thu.Services;
using SoftSpot_Hein_Myat_Thu.ViewModels;
using SoftSpot_Hein_Myat_Thu.Views;

namespace SoftSpot_Hein_Myat_Thu;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
#if ANDROID || IOS || MACCATALYST
            .UseLocalNotification()
#endif
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif


        // =============================
        // Services
        // =============================
        builder.Services.AddSingleton<IStorageService, StorageService>();
        builder.Services.AddSingleton<IPlaceService, PlaceService>();
        builder.Services.AddSingleton<IAppNotificationService, NotificationService>();

        // =============================
        // ViewModels
        // =============================
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<AddPlaceViewModel>();
        builder.Services.AddTransient<FavouritesViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<NotificationsViewModel>();
        builder.Services.AddTransient<DetailsViewModel>();

        // =============================
        // Pages
        // =============================
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<AddPlacePage>();
        builder.Services.AddTransient<FavouritesPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<NotificationsPage>();
        builder.Services.AddTransient<DetailsPage>();


        return builder.Build();
    }
}
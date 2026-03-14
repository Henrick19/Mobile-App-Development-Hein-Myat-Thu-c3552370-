using SoftSpot_Hein_Myat_Thu.Models;
using SoftSpot_Hein_Myat_Thu.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class NotificationsViewModel : BaseViewModel
{
    private readonly IStorageService _storageService;

    public NotificationsViewModel(IStorageService storageService)
    {
        _storageService = storageService;
       
    }
}
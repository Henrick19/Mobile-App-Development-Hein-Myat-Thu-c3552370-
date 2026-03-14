using SoftSpot_Hein_Myat_Thu.Services;
using System.Windows.Input; 

namespace SoftSpot_Hein_Myat_Thu.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private readonly IStorageService _storageService;

    public ProfileViewModel(IStorageService storageService)
    {
        _storageService = storageService;
    }
}
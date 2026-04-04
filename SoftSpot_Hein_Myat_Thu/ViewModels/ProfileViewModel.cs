using SoftSpot_Hein_Myat_Thu.Services;
using System.Windows.Input;

namespace SoftSpot_Hein_Myat_Thu.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IStorageService _storageService;
        

        private int _unreadNotificationCount;
        public int UnreadNotificationCount
        {
            get { return _unreadNotificationCount; }

            set { SetProperty(ref _unreadNotificationCount, value); }
        }

        public ICommand LoadCommand { get; }

        public ProfileViewModel(IStorageService storageService)
        {
            _storageService = storageService;
            LoadCommand = new Command(ExecuteLoadCommand);
        }

        // helper method for loadcommand

        private async void ExecuteLoadCommand()
        {
            await RefreshUnreadCountAsync();
        }

        // helper method to refresh unread noti count
        internal async Task RefreshUnreadCountAsync()
        {
            var list = await _storageService.GetAllNotificationsAsync();

            int count = 0;

            foreach (var noti in list)
            {
                if (!noti.IsRead)
                {
                    count++;
                }
            }

            UnreadNotificationCount = count;
        }
    }
}
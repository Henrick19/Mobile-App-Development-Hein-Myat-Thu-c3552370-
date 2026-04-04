namespace SoftSpot_Hein_Myat_Thu
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Match UI to saved preference on cold start (toggle in Settings only updated Current at runtime).
            UserAppTheme = Preferences.Get("DarkMode", false) ? AppTheme.Dark : AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
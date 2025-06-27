public partial class App : Application
{
    public static LocationDatabaseHelper DatabaseHelper;

    public App()
    {
        InitializeComponent();

        DatabaseHelper = new LocationDatabaseHelper();
        LocationTrackingService.Initialize(DatabaseHelper);

        MainPage = new NavigationPage(new LocationHeatMapPage());

        StartLocationTracking();
    }

    private async void StartLocationTracking()
    {
        var hasPermission = await PermissionService.RequestLocationPermission();
        if (!hasPermission)
        {
            await MainPage.DisplayAlert("Permission denied", "Location permission is required", "OK");
            return;
        }

        LocationTrackingService.Instance.StartTracking();
    }
}

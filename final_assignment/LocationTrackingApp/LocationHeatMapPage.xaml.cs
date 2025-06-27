using System.Windows.Input;
using Microsoft.Maui.Dispatching;

public partial class LocationHeatMapPage : ContentPage
{
    private readonly LocationDatabaseHelper _dbHelper;
    private readonly DispatcherTimer _timer;

    public LocationHeatMapPage(LocationDatabaseHelper dbHelper)
    {
        InitializeComponent();
        _dbHelper = dbHelper;

        _timer = Application.Current.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(3);
        _timer.Tick += (s, e) => LoadHeatMap();
        _timer.Start();
    }

    private async void LoadHeatMap()
    {
        var locations = await _dbHelper.GetLocationsAsync();

        if (locations == null || locations.Count == 0)
            return;

        map.MapElements.Clear();

        var lastLocation = locations.Last();
        map.MoveToRegion(MapSpan.FromCenterAndRadius(
            new Location(lastLocation.Latitude, lastLocation.Longitude),
            Distance.FromKilometers(1)));

        foreach (var loc in locations)
        {
            var circle = new Circle
            {
                Center = new Location(loc.Latitude, loc.Longitude),
                Radius = new Distance(50),
                StrokeColor = Color.FromRgba(255, 0, 0, 50),
                FillColor = Color.FromRgba(255, 0, 0, 80),
                StrokeWidth = 1
            };
            map.MapElements.Add(circle);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LocationTrackingService.LocationUpdated += RefreshHeatMap;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        LocationTrackingService.LocationUpdated -= RefreshHeatMap;
    }

    private async void RefreshHeatMap()
    {
        var locations = await App.DatabaseHelper.GetAllLocationsAsync();
        // Update your map heatmap layer here with new location data
    }

}

// File: LocationTrackingService.cs

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;

public class LocationTrackingService
{
    private static LocationTrackingService _instance;
    private readonly LocationDatabaseHelper _dbHelper;
    private CancellationTokenSource _cts;

    // Event to notify subscribers when location is updated
    public static event Action LocationUpdated;

    private LocationTrackingService(LocationDatabaseHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    // Singleton instance initialization
    public static void Initialize(LocationDatabaseHelper dbHelper)
    {
        if (_instance == null)
        {
            _instance = new LocationTrackingService(dbHelper);
        }
    }

    public static LocationTrackingService Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new InvalidOperationException("LocationTrackingService is not initialized. Call Initialize() first.");
            }
            return _instance;
        }
    }

    public void StartTracking()
    {
        if (_cts != null && !_cts.IsCancellationRequested)
            return; // already tracking

        _cts = new CancellationTokenSource();
        _ = TrackLocationAsync(_cts.Token);
    }

    public void StopTracking()
    {
        _cts?.Cancel();
    }

    private async Task TrackLocationAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync();

                if (location != null)
                {
                    var locData = new LocationData
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Timestamp = DateTime.UtcNow
                    };

                    await _dbHelper.SaveLocationAsync(locData);

                    // Notify subscribers a new location is saved
                    LocationUpdated?.Invoke();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., permissions, no GPS)
                Console.WriteLine($"Error fetching location: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(3), token);
        }
    }
}

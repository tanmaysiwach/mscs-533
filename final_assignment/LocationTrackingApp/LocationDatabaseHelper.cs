// File: LocationDatabaseHelper.cs

using System;
using System.Collections.Generic;
using System.IO;
using SQLite;
using System.Threading.Tasks;

public class LocationData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateTime Timestamp { get; set; }
}

public class LocationDatabaseHelper
{
    private readonly SQLiteAsyncConnection _database;

    public LocationDatabaseHelper(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<LocationData>().Wait();
    }

    public Task<int> SaveLocationAsync(LocationData location)
    {
        if (location.Id != 0)
        {
            return _database.UpdateAsync(location);
        }
        else
        {
            return _database.InsertAsync(location);
        }
    }

    public Task<List<LocationData>> GetAllLocationsAsync()
    {
        return _database.Table<LocationData>().ToListAsync();
    }

    public Task<LocationData> GetLocationAsync(int id)
    {
        return _database.Table<LocationData>()
                        .Where(i => i.Id == id)
                        .FirstOrDefaultAsync();
    }

    public Task<int> DeleteLocationAsync(LocationData location)
    {
        return _database.DeleteAsync(location);
    }
}

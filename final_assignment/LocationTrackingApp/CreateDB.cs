// File: CreateDatabase.cs
using System.Data.SQLite;
using System.IO;

public class DatabaseHelper
{
    private const string DbFileName = "LocationData.db";

    public static void CreateDatabase()
    {
        if (!File.Exists(DbFileName))
        {
            SQLiteConnection.CreateFile(DbFileName);
            using (var connection = new SQLiteConnection($"Data Source={DbFileName};Version=3;"))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS LocationEntry (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Latitude REAL NOT NULL,
                        Longitude REAL NOT NULL,
                        Timestamp TEXT NOT NULL
                    );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

using System;
using System.IO;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace OtelYonetimSistemi.Database;

public static class DatabaseHelper
{
    private static readonly string ConnectionString = LoadConnectionString();

    private static string LoadConnectionString()
    {
        // appsettings.json dosyasını çalışma dizininde veya proje ana dizininde arar
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDir, "appsettings.json");

        if (!File.Exists(filePath))
        {
            filePath = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\appsettings.json"));
        }

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            using JsonDocument doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("ConnectionStrings", out JsonElement connSection) &&
                connSection.TryGetProperty("OtelDbConnection", out JsonElement connValue))
            {
                return connValue.GetString() ?? string.Empty;
            }
        }

        throw new InvalidOperationException("Veritabanı bağlantı dizesi 'appsettings.json' içinde bulunamadı.");
    }

    public static SqlConnection GetConnection() => new(ConnectionString);

    public static bool TestConnection()
    {
        try
        {
            using SqlConnection conn = GetConnection();
            conn.Open();
            return true;
        }
        catch { return false; }
    }
}
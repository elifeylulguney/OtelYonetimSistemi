using System;
using System.Net.Http;
using System.Windows.Forms;

namespace OtelYonetimSistemi;

public static class NetworkCheck
{
    public static bool IsInternetAvailable()
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
            var resp = client.GetAsync("https://www.bing.com").GetAwaiter().GetResult();
            return resp.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public static bool EnsureOnline(bool showMessage = true)
    {
        if (IsInternetAvailable()) return true;
        if (showMessage)
        {
            MessageBox.Show("İnternet bağlantısı yok. Lütfen bağlantınızı kontrol edin.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        return false;
    }
}

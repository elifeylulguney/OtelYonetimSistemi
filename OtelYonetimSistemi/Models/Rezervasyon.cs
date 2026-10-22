namespace OtelYonetimSistemi.Models;

public class Rezervasyon
{
    public int RezervasyonId { get; set; }
    public int OdaNo { get; set; }
    public string MusteriSsn { get; set; } = "";
    public DateTime Giris { get; set; }
    public DateTime Cikis { get; set; }
}

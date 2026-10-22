namespace OtelYonetimSistemi.Models;

public class OdaTemizlik
{
    public int TemizlikId { get; set; }
    public int OdaNo { get; set; }
    public string CalisanSsn { get; set; } = "";
    public DateTime TemizlikTarihi { get; set; }
    public string TemizlikDurumu { get; set; } = "";
}

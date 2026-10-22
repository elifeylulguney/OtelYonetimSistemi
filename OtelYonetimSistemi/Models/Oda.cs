namespace OtelYonetimSistemi.Models;

public class Oda
{
    public int OdaNo { get; set; }
    public string OdaIsmi { get; set; } = "";
    public int Kat { get; set; }
    public int Yatak { get; set; }
    public bool Balkon { get; set; }
    public string Manzara { get; set; } = "";
    public decimal Fiyat { get; set; }
}

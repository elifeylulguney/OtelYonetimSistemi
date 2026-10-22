using Microsoft.Data.SqlClient;
using OtelYonetimSistemi.Database;
using OtelYonetimSistemi.Models;

namespace OtelYonetimSistemi.Repositories;

public class OdaTemizlikRepository
{
    public List<OdaTemizlik> GetAll()
    {
        var list = new List<OdaTemizlik>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("SELECT * FROM ODA_TEMIZLIK ORDER BY temizlik_id", conn);
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public List<OdaTemizlik> Search(string keyword)
    {
        var list = new List<OdaTemizlik>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "SELECT * FROM ODA_TEMIZLIK WHERE " +
            "CAST(temizlik_id AS NVARCHAR) LIKE @kw OR " +
            "CAST(oda_no AS NVARCHAR) LIKE @kw OR " +
            "calisan_ssn LIKE @kw OR temizlik_durumu LIKE @kw " +
            "ORDER BY temizlik_id", conn);
        cmd.Parameters.AddWithValue("@kw", $"%{keyword}%");
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public void Insert(OdaTemizlik t)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "INSERT INTO ODA_TEMIZLIK (oda_no,calisan_ssn,temizlik_tarihi,temizlik_durumu) " +
            "VALUES (@oda,@ssn,@tarih,@durum)", conn);
        Params(cmd, t);
        cmd.ExecuteNonQuery();
    }

    public void Update(OdaTemizlik t)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "UPDATE ODA_TEMIZLIK SET oda_no=@oda,calisan_ssn=@ssn," +
            "temizlik_tarihi=@tarih,temizlik_durumu=@durum WHERE temizlik_id=@id", conn);
        cmd.Parameters.AddWithValue("@id", t.TemizlikId);
        Params(cmd, t);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("DELETE FROM ODA_TEMIZLIK WHERE temizlik_id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    private static OdaTemizlik Map(SqlDataReader r) => new()
    {
        TemizlikId = Convert.ToInt32(r["temizlik_id"]),
        OdaNo = Convert.ToInt32(r["oda_no"]),
        CalisanSsn = r["calisan_ssn"].ToString()!,
        TemizlikTarihi = Convert.ToDateTime(r["temizlik_tarihi"]),
        TemizlikDurumu = r["temizlik_durumu"].ToString()!
    };

    private static void Params(SqlCommand cmd, OdaTemizlik t)
    {
        cmd.Parameters.AddWithValue("@oda", t.OdaNo);
        cmd.Parameters.AddWithValue("@ssn", t.CalisanSsn);
        cmd.Parameters.AddWithValue("@tarih", t.TemizlikTarihi);
        cmd.Parameters.AddWithValue("@durum", t.TemizlikDurumu);
    }
}

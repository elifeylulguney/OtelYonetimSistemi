using Microsoft.Data.SqlClient;
using OtelYonetimSistemi.Database;
using OtelYonetimSistemi.Models;

namespace OtelYonetimSistemi.Repositories;

public class RezervasyonRepository
{
    public List<Rezervasyon> GetAll()
    {
        var list = new List<Rezervasyon>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("SELECT * FROM REZERVASYON ORDER BY rezervasyon_id", conn);
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public List<Rezervasyon> Search(string keyword)
    {
        var list = new List<Rezervasyon>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "SELECT * FROM REZERVASYON WHERE " +
            "CAST(rezervasyon_id AS NVARCHAR) LIKE @kw OR " +
            "CAST(oda_no AS NVARCHAR) LIKE @kw OR musteri_ssn LIKE @kw " +
            "ORDER BY rezervasyon_id", conn);
        cmd.Parameters.AddWithValue("@kw", $"%{keyword}%");
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public void Insert(Rezervasyon rez)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "INSERT INTO REZERVASYON (oda_no,musteri_ssn,giris,cikis) " +
            "VALUES (@oda,@ssn,@giris,@cikis)", conn);
        Params(cmd, rez);
        cmd.ExecuteNonQuery();
    }

    public void Update(Rezervasyon rez)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "UPDATE REZERVASYON SET oda_no=@oda,musteri_ssn=@ssn," +
            "giris=@giris,cikis=@cikis WHERE rezervasyon_id=@id", conn);
        cmd.Parameters.AddWithValue("@id", rez.RezervasyonId);
        Params(cmd, rez);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("DELETE FROM REZERVASYON WHERE rezervasyon_id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    private static Rezervasyon Map(SqlDataReader r) => new()
    {
        RezervasyonId = Convert.ToInt32(r["rezervasyon_id"]),
        OdaNo = Convert.ToInt32(r["oda_no"]),
        MusteriSsn = r["musteri_ssn"].ToString()!,
        Giris = Convert.ToDateTime(r["giris"]),
        Cikis = Convert.ToDateTime(r["cikis"])
    };

    private static void Params(SqlCommand cmd, Rezervasyon rez)
    {
        cmd.Parameters.AddWithValue("@oda", rez.OdaNo);
        cmd.Parameters.AddWithValue("@ssn", rez.MusteriSsn);
        cmd.Parameters.AddWithValue("@giris", rez.Giris);
        cmd.Parameters.AddWithValue("@cikis", rez.Cikis);
    }
}

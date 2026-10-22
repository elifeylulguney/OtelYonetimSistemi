using Microsoft.Data.SqlClient;
using OtelYonetimSistemi.Database;
using OtelYonetimSistemi.Models;

namespace OtelYonetimSistemi.Repositories;

public class CalisanRepository
{
    public List<Calisan> GetAll()
    {
        var list = new List<Calisan>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("SELECT * FROM CALISANLAR ORDER BY soyadi,adi", conn);
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public List<Calisan> Search(string keyword)
    {
        var list = new List<Calisan>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "SELECT * FROM CALISANLAR WHERE " +
            "calisan_ssn LIKE @kw OR adi LIKE @kw OR soyadi LIKE @kw OR " +
            "pozisyon LIKE @kw OR calisma_saati LIKE @kw " +
            "ORDER BY soyadi,adi", conn);
        cmd.Parameters.AddWithValue("@kw", $"%{keyword}%");
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public void Insert(Calisan c)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "INSERT INTO CALISANLAR (calisan_ssn,adi,soyadi,pozisyon,maas,calisma_saati) " +
            "VALUES (@ssn,@adi,@soyadi,@poz,@maas,@saat)", conn);
        Params(cmd, c);
        cmd.ExecuteNonQuery();
    }

    public void Update(Calisan c)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "UPDATE CALISANLAR SET adi=@adi,soyadi=@soyadi,pozisyon=@poz," +
            "maas=@maas,calisma_saati=@saat WHERE calisan_ssn=@ssn", conn);
        Params(cmd, c);
        cmd.ExecuteNonQuery();
    }

    public void Delete(string ssn)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("DELETE FROM CALISANLAR WHERE calisan_ssn=@ssn", conn);
        cmd.Parameters.AddWithValue("@ssn", ssn);
        cmd.ExecuteNonQuery();
    }

    private static Calisan Map(SqlDataReader r) => new()
    {
        CalisanSsn = r["calisan_ssn"].ToString()!,
        Adi = r["adi"].ToString()!,
        Soyadi = r["soyadi"].ToString()!,
        Pozisyon = r["pozisyon"].ToString()!,
        Maas = Convert.ToDecimal(r["maas"]),
        CalismaSaati = r["calisma_saati"].ToString()!
    };

    private static void Params(SqlCommand cmd, Calisan c)
    {
        cmd.Parameters.AddWithValue("@ssn", c.CalisanSsn);
        cmd.Parameters.AddWithValue("@adi", c.Adi);
        cmd.Parameters.AddWithValue("@soyadi", c.Soyadi);
        cmd.Parameters.AddWithValue("@poz", c.Pozisyon);
        cmd.Parameters.AddWithValue("@maas", c.Maas);
        cmd.Parameters.AddWithValue("@saat", c.CalismaSaati);
    }
}

using Microsoft.Data.SqlClient;
using OtelYonetimSistemi.Database;
using OtelYonetimSistemi.Models;

namespace OtelYonetimSistemi.Repositories;

public class MusteriRepository
{
    public List<Musteri> GetAll()
    {
        var list = new List<Musteri>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("SELECT * FROM MUSTERILER ORDER BY soyadi,adi", conn);
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public List<Musteri> Search(string keyword)
    {
        var list = new List<Musteri>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "SELECT * FROM MUSTERILER WHERE " +
            "musteri_ssn LIKE @kw OR adi LIKE @kw OR soyadi LIKE @kw OR " +
            "telefon LIKE @kw OR email LIKE @kw OR odeme_durumu LIKE @kw " +
            "ORDER BY soyadi,adi", conn);
        cmd.Parameters.AddWithValue("@kw", $"%{keyword}%");
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public void Insert(Musteri m)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "INSERT INTO MUSTERILER (musteri_ssn,adi,soyadi,telefon,email,odeme_durumu) " +
            "VALUES (@ssn,@adi,@soyadi,@tel,@email,@odeme)", conn);
        Params(cmd, m);
        cmd.ExecuteNonQuery();
    }

    public void Update(Musteri m)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "UPDATE MUSTERILER SET adi=@adi,soyadi=@soyadi,telefon=@tel," +
            "email=@email,odeme_durumu=@odeme WHERE musteri_ssn=@ssn", conn);
        Params(cmd, m);
        cmd.ExecuteNonQuery();
    }

    public void Delete(string ssn)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("DELETE FROM MUSTERILER WHERE musteri_ssn=@ssn", conn);
        cmd.Parameters.AddWithValue("@ssn", ssn);
        cmd.ExecuteNonQuery();
    }

    private static Musteri Map(SqlDataReader r) => new()
    {
        MusteriSsn = r["musteri_ssn"].ToString()!,
        Adi = r["adi"].ToString()!,
        Soyadi = r["soyadi"].ToString()!,
        Telefon = r["telefon"].ToString()!,
        Email = r["email"].ToString()!,
        OdemeDurumu = r["odeme_durumu"].ToString()!
    };

    private static void Params(SqlCommand cmd, Musteri m)
    {
        cmd.Parameters.AddWithValue("@ssn", m.MusteriSsn);
        cmd.Parameters.AddWithValue("@adi", m.Adi);
        cmd.Parameters.AddWithValue("@soyadi", m.Soyadi);
        cmd.Parameters.AddWithValue("@tel", m.Telefon);
        cmd.Parameters.AddWithValue("@email", m.Email);
        cmd.Parameters.AddWithValue("@odeme", m.OdemeDurumu);
    }
}

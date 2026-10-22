using Microsoft.Data.SqlClient;
using OtelYonetimSistemi.Database;
using OtelYonetimSistemi.Models;

namespace OtelYonetimSistemi.Repositories;

public class OdaRepository
{
    public List<Oda> GetAll()
    {
        var list = new List<Oda>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("SELECT * FROM ODALAR ORDER BY oda_no", conn);
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public List<Oda> Search(string keyword)
    {
        var list = new List<Oda>();
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "SELECT * FROM ODALAR WHERE " +
            "CAST(oda_no AS NVARCHAR) LIKE @kw OR oda_ismi LIKE @kw OR " +
            "manzara LIKE @kw OR CAST(kat AS NVARCHAR) LIKE @kw " +
            "ORDER BY oda_no", conn);
        cmd.Parameters.AddWithValue("@kw", $"%{keyword}%");
        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read()) list.Add(Map(r));
        return list;
    }

    public void Insert(Oda o)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "INSERT INTO ODALAR (oda_no,oda_ismi,kat,yatak,balkon,manzara,fiyat) " +
            "VALUES (@no,@isim,@kat,@yatak,@balkon,@manzara,@fiyat)", conn);
        Params(cmd, o);
        cmd.ExecuteNonQuery();
    }

    public void Update(Oda o)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new(
            "UPDATE ODALAR SET oda_ismi=@isim,kat=@kat,yatak=@yatak," +
            "balkon=@balkon,manzara=@manzara,fiyat=@fiyat WHERE oda_no=@no", conn);
        Params(cmd, o);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int odaNo)
    {
        using SqlConnection conn = DatabaseHelper.GetConnection();
        conn.Open();
        using SqlCommand cmd = new("DELETE FROM ODALAR WHERE oda_no=@no", conn);
        cmd.Parameters.AddWithValue("@no", odaNo);
        cmd.ExecuteNonQuery();
    }

    private static Oda Map(SqlDataReader r) => new()
    {
        OdaNo = Convert.ToInt32(r["oda_no"]),
        OdaIsmi = r["oda_ismi"].ToString()!,
        Kat = Convert.ToInt32(r["kat"]),
        Yatak = Convert.ToInt32(r["yatak"]),
        Balkon = Convert.ToBoolean(r["balkon"]),
        Manzara = r["manzara"].ToString()!,
        Fiyat = Convert.ToDecimal(r["fiyat"])
    };

    private static void Params(SqlCommand cmd, Oda o)
    {
        cmd.Parameters.AddWithValue("@no", o.OdaNo);
        cmd.Parameters.AddWithValue("@isim", o.OdaIsmi);
        cmd.Parameters.AddWithValue("@kat", o.Kat);
        cmd.Parameters.AddWithValue("@yatak", o.Yatak);
        cmd.Parameters.AddWithValue("@balkon", o.Balkon);
        cmd.Parameters.AddWithValue("@manzara", o.Manzara);
        cmd.Parameters.AddWithValue("@fiyat", o.Fiyat);
    }
}

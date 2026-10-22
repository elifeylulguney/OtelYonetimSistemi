using OtelYonetimSistemi.Models;
using OtelYonetimSistemi.Repositories;

namespace OtelYonetimSistemi.Forms;

public class CalisanForm : BaseForm
{
    private readonly CalisanRepository _repo = new();

    private DataGridView dgv = new();
    private TextBox txtAra = new();
    private TextBox txtSsn = new();
    private TextBox txtAdi = new();
    private TextBox txtSoyadi = new();
    private ComboBox cmbPozisyon = new();
    private TextBox txtMaas = new();
    private TextBox txtCalismaSaati = new();
    private Label lblSayac = new();

    public CalisanForm()
    {
        Text = "Çalışan Yönetimi";
        Size = new Size(1050, 680);
        BuildUI();
        Listele();
    }

    private void BuildUI()
    {
        var pnlLeft = new Panel
        {
            Dock = DockStyle.Left,
            Width = 285,
            BackColor = ClrPanel
        };

        pnlLeft.Controls.Add(new Label
        {
            Text = "ÇALIŞAN BİLGİLERİ",
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = ClrSubText,
            Location = new Point(15, 15),
            Size = new Size(255, 26)
        });

        int y = 50;

        pnlLeft.Controls.Add(MakeLabel("TC / SSN  *", new Point(15, y)));
        txtSsn = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtSsn); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Adı  *", new Point(15, y)));
        txtAdi = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtAdi); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Soyadı  *", new Point(15, y)));
        txtSoyadi = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtSoyadi); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Pozisyon", new Point(15, y)));
        cmbPozisyon = MakeComboBox(new Point(15, y + 23), 240,
            ["Resepsiyonist", "Temizlik Görevlisi", "Güvenlik",
             "Yönetici", "Aşçı", "Garson", "Teknisyen", "Muhasebe"]);
        pnlLeft.Controls.Add(cmbPozisyon); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Maaş (₺)  *", new Point(15, y)));
        txtMaas = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtMaas); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Haftalık Çalışma Saati", new Point(15, y)));
        txtCalismaSaati = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtCalismaSaati); y += 62;

        var btnEkle = MakeButton("➕  Ekle", ClrBlue, new Point(15, y));
        var btnGuncelle = MakeButton("✏  Güncelle", ClrBlue, new Point(145, y)); y += 46;
        var btnSil = MakeButton("🗑  Sil", ClrBlue, new Point(15, y));
        var btnTemizle = MakeButton("🔄  Temizle", ClrBlue, new Point(145, y));

        btnEkle.Click += (_, _) => { if (EnsureOnline()) Ekle(); };
        btnGuncelle.Click += (_, _) => { if (EnsureOnline()) Guncelle(); };
        btnSil.Click += (_, _) => { if (EnsureOnline()) Sil(); };
        btnTemizle.Click += (_, _) => Temizle();

        pnlLeft.Controls.AddRange([btnEkle, btnGuncelle, btnSil, btnTemizle]);

        var pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
        var pnlAra = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = ClrBackground };

        txtAra = MakeTextBox(new Point(0, 10), 320);
        txtAra.BackColor = Color.White;
        txtAra.PlaceholderText = "🔍  Ara  (SSN, ad, soyad, pozisyon...)";
        txtAra.Click += (_, _) => { if (!EnsureOnline()) ActiveControl = null; };
        txtAra.Enter += (_, _) => { if (!EnsureOnline()) ActiveControl = null; };
        txtAra.TextChanged += (_, _) =>
        {
            if (!EnsureOnline()) return;
            if (string.IsNullOrWhiteSpace(txtAra.Text)) Listele();
            else Ara(txtAra.Text);
        };

        var btnYenile = MakeButton("Yenile", ClrBlue, new Point(330, 10), 95);
        btnYenile.Click += (_, _) => { if (EnsureOnline()) Listele(); };
        lblSayac = new Label { Location = new Point(435, 16), AutoSize = true, ForeColor = Color.Black };

        pnlAra.Controls.AddRange([txtAra, btnYenile, lblSayac]);
        dgv = new DataGridView { Dock = DockStyle.Fill };
        StyleGrid(dgv);
        dgv.CellClick += (_, e) => GridSecimiDoldur(e.RowIndex);

        pnlRight.Controls.Add(dgv);
        pnlRight.Controls.Add(pnlAra);
        Controls.Add(pnlRight);
        Controls.Add(pnlLeft);
    }

    private void Listele()
    {
        try
        {
            var data = _repo.GetAll();
            dgv.DataSource = null;
            dgv.DataSource = data;
            SutunAdlari();
            lblSayac.Text = $"Toplam: {data.Count} kayıt";
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Ara(string kw)
    {
        try
        {
            var data = _repo.Search(kw);
            dgv.DataSource = null;
            dgv.DataSource = data;
            SutunAdlari();
            lblSayac.Text = $"Bulunan: {data.Count} kayıt";
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Ekle()
    {
        if (!Dogrula()) return;
        try { _repo.Insert(FormdenAl()); ShowSuccess("Çalışan eklendi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Guncelle()
    {
        if (string.IsNullOrWhiteSpace(txtSsn.Text))
        { MessageBox.Show("Güncellemek için listeden satır seçin."); return; }
        if (!Dogrula()) return;
        try { _repo.Update(FormdenAl()); ShowSuccess("Çalışan güncellendi."); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Sil()
    {
        if (string.IsNullOrWhiteSpace(txtSsn.Text))
        { MessageBox.Show("Silmek için listeden satır seçin."); return; }
        if (!Confirm($"{txtAdi.Text} {txtSoyadi.Text} silinsin mi?")) return;
        try { _repo.Delete(txtSsn.Text); ShowSuccess("Çalışan silindi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void GridSecimiDoldur(int rowIndex)
    {
        if (rowIndex < 0) return;
        var row = dgv.Rows[rowIndex];
        txtSsn.Text = row.Cells["CalisanSsn"].Value?.ToString() ?? "";
        txtAdi.Text = row.Cells["Adi"].Value?.ToString() ?? "";
        txtSoyadi.Text = row.Cells["Soyadi"].Value?.ToString() ?? "";
        txtMaas.Text = row.Cells["Maas"].Value?.ToString() ?? "";
        txtCalismaSaati.Text = row.Cells["CalismaSaati"].Value?.ToString() ?? "";
        var poz = row.Cells["Pozisyon"].Value?.ToString() ?? "";
        int idx = cmbPozisyon.Items.IndexOf(poz);
        cmbPozisyon.SelectedIndex = idx >= 0 ? idx : 0;
    }

    private Calisan FormdenAl() => new()
    {
        CalisanSsn = txtSsn.Text.Trim(),
        Adi = txtAdi.Text.Trim(),
        Soyadi = txtSoyadi.Text.Trim(),
        Pozisyon = cmbPozisyon.SelectedItem?.ToString() ?? "",
        Maas = decimal.Parse(txtMaas.Text.Trim()),
        CalismaSaati = txtCalismaSaati.Text.Trim()
    };

    private bool Dogrula()
    {
        if (string.IsNullOrWhiteSpace(txtSsn.Text) ||
            string.IsNullOrWhiteSpace(txtAdi.Text) ||
            string.IsNullOrWhiteSpace(txtSoyadi.Text) ||
            string.IsNullOrWhiteSpace(txtMaas.Text))
        {
            MessageBox.Show("TC/SSN, Adı, Soyadı ve Maaş zorunludur.", "Eksik Alan",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (!decimal.TryParse(txtMaas.Text, out _))
        {
            MessageBox.Show("Maaş sayısal olmalıdır.", "Format Hatası",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private void Temizle()
    {
        txtSsn.Clear(); txtAdi.Clear(); txtSoyadi.Clear();
        txtMaas.Clear(); txtCalismaSaati.Clear();
        cmbPozisyon.SelectedIndex = 0;
        txtAra.Clear();
    }

    private void SutunAdlari()
    {
        if (dgv.Columns.Count == 0) return;
        var map = new Dictionary<string, string>
        {
            {"CalisanSsn","TC/SSN"},{"Adi","Adı"},{"Soyadi","Soyadı"},
            {"Pozisyon","Pozisyon"},{"Maas","Maaş (₺)"},{"CalismaSaati","Çalışma Saati"}
        };
        foreach (DataGridViewColumn col in dgv.Columns)
            if (map.TryGetValue(col.Name, out var ad)) col.HeaderText = ad;
    }
}

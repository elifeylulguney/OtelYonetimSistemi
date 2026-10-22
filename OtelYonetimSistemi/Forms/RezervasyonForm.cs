using OtelYonetimSistemi.Models;
using OtelYonetimSistemi.Repositories;

namespace OtelYonetimSistemi.Forms;

public class RezervasyonForm : BaseForm
{
    private readonly RezervasyonRepository _repo = new();

    private DataGridView dgv = new();
    private TextBox txtAra = new();
    private TextBox txtId = new();
    private TextBox txtOdaNo = new();
    private TextBox txtMusteriSsn = new();
    private DateTimePicker dtpGiris = new();
    private DateTimePicker dtpCikis = new();
    private Label lblGece = new();
    private Label lblSayac = new();

    public RezervasyonForm()
    {
        Text = "Rezervasyon Yönetimi";
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
            Text = "REZERVASYON BİLGİLERİ",
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = ClrSubText,
            Location = new Point(15, 15),
            Size = new Size(255, 26)
        });

        int y = 50;

        pnlLeft.Controls.Add(MakeLabel("Rezervasyon ID (otomatik)", new Point(15, y)));
        txtId = MakeTextBox(new Point(15, y + 23));
        txtId.Enabled = false;
        pnlLeft.Controls.Add(txtId); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Oda No  *", new Point(15, y)));
        txtOdaNo = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtOdaNo); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Müşteri TC / SSN  *", new Point(15, y)));
        txtMusteriSsn = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtMusteriSsn); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Giriş Tarihi  *", new Point(15, y)));
        dtpGiris = MakeDatePicker(new Point(15, y + 23));
        dtpGiris.ValueChanged += (_, _) => HesaplaGece();
        pnlLeft.Controls.Add(dtpGiris); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Çıkış Tarihi  *", new Point(15, y)));
        dtpCikis = MakeDatePicker(new Point(15, y + 23));
        dtpCikis.Value = DateTime.Today.AddDays(1);
        dtpCikis.ValueChanged += (_, _) => HesaplaGece();
        pnlLeft.Controls.Add(dtpCikis); y += 62;

        lblGece = new Label
        {
            Text = "📅  1 gece",
            Location = new Point(15, y),
            Size = new Size(255, 22),
            ForeColor = Color.FromArgb(239, 239, 236),
            Font = new Font("Segoe UI", 9f, FontStyle.Italic)
        };
        pnlLeft.Controls.Add(lblGece); y += 36;

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
        txtAra.PlaceholderText = "🔍  Ara  (ID, oda no, müşteri SSN...)";
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

    private void HesaplaGece()
    {
        int fark = (dtpCikis.Value.Date - dtpGiris.Value.Date).Days;
        lblGece.Text = fark > 0 ? $"📅  {fark} gece" : "⚠  Geçersiz tarih aralığı";
        lblGece.ForeColor = fark > 0 ? Color.FromArgb(239, 239, 236) : Color.OrangeRed;
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
        try { _repo.Insert(FormdenAl()); ShowSuccess("Rezervasyon eklendi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Guncelle()
    {
        if (string.IsNullOrWhiteSpace(txtId.Text))
        { MessageBox.Show("Güncellemek için listeden satır seçin."); return; }
        if (!Dogrula()) return;
        try { _repo.Update(FormdenAl()); ShowSuccess("Rezervasyon güncellendi."); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Sil()
    {
        if (string.IsNullOrWhiteSpace(txtId.Text))
        { MessageBox.Show("Silmek için listeden satır seçin."); return; }
        if (!Confirm($"Rezervasyon #{txtId.Text} silinsin mi?")) return;
        try { _repo.Delete(int.Parse(txtId.Text)); ShowSuccess("Rezervasyon silindi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void GridSecimiDoldur(int rowIndex)
    {
        if (rowIndex < 0) return;
        var row = dgv.Rows[rowIndex];
        txtId.Text = row.Cells["RezervasyonId"].Value?.ToString() ?? "";
        txtOdaNo.Text = row.Cells["OdaNo"].Value?.ToString() ?? "";
        txtMusteriSsn.Text = row.Cells["MusteriSsn"].Value?.ToString() ?? "";
        if (DateTime.TryParse(row.Cells["Giris"].Value?.ToString(), out var g)) dtpGiris.Value = g;
        if (DateTime.TryParse(row.Cells["Cikis"].Value?.ToString(), out var c)) dtpCikis.Value = c;
        HesaplaGece();
    }

    private Rezervasyon FormdenAl() => new()
    {
        RezervasyonId = string.IsNullOrWhiteSpace(txtId.Text) ? 0 : int.Parse(txtId.Text),
        OdaNo = int.Parse(txtOdaNo.Text.Trim()),
        MusteriSsn = txtMusteriSsn.Text.Trim(),
        Giris = dtpGiris.Value.Date,
        Cikis = dtpCikis.Value.Date
    };

    private bool Dogrula()
    {
        if (string.IsNullOrWhiteSpace(txtOdaNo.Text) ||
            string.IsNullOrWhiteSpace(txtMusteriSsn.Text))
        {
            MessageBox.Show("Oda No ve Müşteri SSN zorunludur.", "Eksik Alan",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (!int.TryParse(txtOdaNo.Text, out _))
        {
            MessageBox.Show("Oda No sayı olmalıdır.", "Format Hatası",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (dtpCikis.Value.Date <= dtpGiris.Value.Date)
        {
            MessageBox.Show("Çıkış tarihi giriş tarihinden sonra olmalıdır.", "Tarih Hatası",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private void Temizle()
    {
        txtId.Clear(); txtOdaNo.Clear(); txtMusteriSsn.Clear();
        dtpGiris.Value = DateTime.Today;
        dtpCikis.Value = DateTime.Today.AddDays(1);
        txtAra.Clear();
        HesaplaGece();
    }

    private void SutunAdlari()
    {
        if (dgv.Columns.Count == 0) return;
        var map = new Dictionary<string, string>
        {
            {"RezervasyonId","Rez. ID"},{"OdaNo","Oda No"},
            {"MusteriSsn","Müşteri SSN"},{"Giris","Giriş"},{"Cikis","Çıkış"}
        };
        foreach (DataGridViewColumn col in dgv.Columns)
            if (map.TryGetValue(col.Name, out var ad)) col.HeaderText = ad;
    }
}

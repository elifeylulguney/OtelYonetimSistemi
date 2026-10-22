using OtelYonetimSistemi.Models;
using OtelYonetimSistemi.Repositories;

namespace OtelYonetimSistemi.Forms;

public class OdaTemizlikForm : BaseForm
{
    private readonly OdaTemizlikRepository _repo = new();

    private DataGridView dgv = new();
    private TextBox txtAra = new();
    private TextBox txtId = new();
    private TextBox txtOdaNo = new();
    private TextBox txtCalisanSsn = new();
    private DateTimePicker dtpTarih = new();
    private ComboBox cmbDurum = new();
    private Label lblSayac = new();

    public OdaTemizlikForm()
    {
        Text = "Oda Temizlik Yönetimi";
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
            Text = "TEMİZLİK BİLGİLERİ",
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = ClrSubText,
            Location = new Point(15, 15),
            Size = new Size(255, 26)
        });

        int y = 50;

        pnlLeft.Controls.Add(MakeLabel("Temizlik ID (otomatik)", new Point(15, y)));
        txtId = MakeTextBox(new Point(15, y + 23));
        txtId.Enabled = false;
        pnlLeft.Controls.Add(txtId); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Oda No  *", new Point(15, y)));
        txtOdaNo = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtOdaNo); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Çalışan TC / SSN  *", new Point(15, y)));
        txtCalisanSsn = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtCalisanSsn); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Temizlik Tarihi  *", new Point(15, y)));
        dtpTarih = MakeDatePicker(new Point(15, y + 23));
        pnlLeft.Controls.Add(dtpTarih); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Temizlik Durumu  *", new Point(15, y)));
        cmbDurum = MakeComboBox(new Point(15, y + 23), 240,
            ["Tamamlandı", "Devam Ediyor", "Beklemede", "İptal"]);
        pnlLeft.Controls.Add(cmbDurum); y += 62;

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
        txtAra.PlaceholderText = "🔍  Ara  (ID, oda no, çalışan SSN, durum...)";
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
        try { _repo.Insert(FormdenAl()); ShowSuccess("Temizlik kaydı eklendi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Guncelle()
    {
        if (string.IsNullOrWhiteSpace(txtId.Text))
        { MessageBox.Show("Güncellemek için listeden satır seçin."); return; }
        if (!Dogrula()) return;
        try { _repo.Update(FormdenAl()); ShowSuccess("Temizlik kaydı güncellendi."); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Sil()
    {
        if (string.IsNullOrWhiteSpace(txtId.Text))
        { MessageBox.Show("Silmek için listeden satır seçin."); return; }
        if (!Confirm($"Temizlik kaydı #{txtId.Text} silinsin mi?")) return;
        try { _repo.Delete(int.Parse(txtId.Text)); ShowSuccess("Temizlik kaydı silindi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void GridSecimiDoldur(int rowIndex)
    {
        if (rowIndex < 0) return;
        var row = dgv.Rows[rowIndex];
        txtId.Text = row.Cells["TemizlikId"].Value?.ToString() ?? "";
        txtOdaNo.Text = row.Cells["OdaNo"].Value?.ToString() ?? "";
        txtCalisanSsn.Text = row.Cells["CalisanSsn"].Value?.ToString() ?? "";
        if (DateTime.TryParse(row.Cells["TemizlikTarihi"].Value?.ToString(), out var t)) dtpTarih.Value = t;
        var durum = row.Cells["TemizlikDurumu"].Value?.ToString() ?? "";
        int idx = cmbDurum.Items.IndexOf(durum);
        cmbDurum.SelectedIndex = idx >= 0 ? idx : 0;
    }

    private OdaTemizlik FormdenAl() => new()
    {
        TemizlikId = string.IsNullOrWhiteSpace(txtId.Text) ? 0 : int.Parse(txtId.Text),
        OdaNo = int.Parse(txtOdaNo.Text.Trim()),
        CalisanSsn = txtCalisanSsn.Text.Trim(),
        TemizlikTarihi = dtpTarih.Value.Date,
        TemizlikDurumu = cmbDurum.SelectedItem?.ToString() ?? ""
    };

    private bool Dogrula()
    {
        if (string.IsNullOrWhiteSpace(txtOdaNo.Text) ||
            string.IsNullOrWhiteSpace(txtCalisanSsn.Text))
        {
            MessageBox.Show("Oda No ve Çalışan SSN zorunludur.", "Eksik Alan",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (!int.TryParse(txtOdaNo.Text, out _))
        {
            MessageBox.Show("Oda No sayı olmalıdır.", "Format Hatası",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private void Temizle()
    {
        txtId.Clear(); txtOdaNo.Clear(); txtCalisanSsn.Clear();
        dtpTarih.Value = DateTime.Today;
        cmbDurum.SelectedIndex = 0;
        txtAra.Clear();
    }

    private void SutunAdlari()
    {
        if (dgv.Columns.Count == 0) return;
        var map = new Dictionary<string, string>
        {
            {"TemizlikId","ID"},{"OdaNo","Oda No"},{"CalisanSsn","Çalışan SSN"},
            {"TemizlikTarihi","Tarih"},{"TemizlikDurumu","Durum"}
        };
        foreach (DataGridViewColumn col in dgv.Columns)
            if (map.TryGetValue(col.Name, out var ad)) col.HeaderText = ad;
    }
}

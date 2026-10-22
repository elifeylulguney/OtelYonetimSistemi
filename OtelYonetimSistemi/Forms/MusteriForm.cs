using OtelYonetimSistemi.Models;
using OtelYonetimSistemi.Repositories;

namespace OtelYonetimSistemi.Forms;

public class MusteriForm : BaseForm
{
    private readonly MusteriRepository _repo = new();

    private DataGridView dgv = new();
    private TextBox txtAra = new();
    private TextBox txtSsn = new();
    private TextBox txtAdi = new();
    private TextBox txtSoyadi = new();
    private TextBox txtTelefon = new();
    private TextBox txtEmail = new();
    private ComboBox cmbOdeme = new();
    private Label lblSayac = new();

    public MusteriForm()
    {
        Text = "Müşteri Yönetimi";
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
            Text = "MÜŞTERİ BİLGİLERİ",
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

        pnlLeft.Controls.Add(MakeLabel("Telefon", new Point(15, y)));
        txtTelefon = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtTelefon); y += 62;

        pnlLeft.Controls.Add(MakeLabel("E-posta", new Point(15, y)));
        txtEmail = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtEmail); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Ödeme Durumu", new Point(15, y)));
        cmbOdeme = MakeComboBox(new Point(15, y + 23), 240,
            ["Ödendi", "Beklemede", "Kısmi Ödeme", "İptal"]);
        pnlLeft.Controls.Add(cmbOdeme); y += 62;

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
        txtAra.PlaceholderText = "🔍  Ara  (SSN, ad, soyad, telefon, email...)";
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
        try { _repo.Insert(FormdenAl()); ShowSuccess("Müşteri eklendi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Guncelle()
    {
        if (string.IsNullOrWhiteSpace(txtSsn.Text))
        { MessageBox.Show("Güncellemek için listeden satır seçin."); return; }
        if (!Dogrula()) return;
        try { _repo.Update(FormdenAl()); ShowSuccess("Müşteri güncellendi."); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Sil()
    {
        if (string.IsNullOrWhiteSpace(txtSsn.Text))
        { MessageBox.Show("Silmek için listeden satır seçin."); return; }
        if (!Confirm($"{txtAdi.Text} {txtSoyadi.Text} silinsin mi?")) return;
        try { _repo.Delete(txtSsn.Text); ShowSuccess("Müşteri silindi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void GridSecimiDoldur(int rowIndex)
    {
        if (rowIndex < 0) return;
        var row = dgv.Rows[rowIndex];
        txtSsn.Text = row.Cells["MusteriSsn"].Value?.ToString() ?? "";
        txtAdi.Text = row.Cells["Adi"].Value?.ToString() ?? "";
        txtSoyadi.Text = row.Cells["Soyadi"].Value?.ToString() ?? "";
        txtTelefon.Text = row.Cells["Telefon"].Value?.ToString() ?? "";
        txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
        var odeme = row.Cells["OdemeDurumu"].Value?.ToString() ?? "";
        int idx = cmbOdeme.Items.IndexOf(odeme);
        cmbOdeme.SelectedIndex = idx >= 0 ? idx : 0;
    }

    private Musteri FormdenAl() => new()
    {
        MusteriSsn = txtSsn.Text.Trim(),
        Adi = txtAdi.Text.Trim(),
        Soyadi = txtSoyadi.Text.Trim(),
        Telefon = txtTelefon.Text.Trim(),
        Email = txtEmail.Text.Trim(),
        OdemeDurumu = cmbOdeme.SelectedItem?.ToString() ?? ""
    };

    private bool Dogrula()
    {
        if (string.IsNullOrWhiteSpace(txtSsn.Text) ||
            string.IsNullOrWhiteSpace(txtAdi.Text) ||
            string.IsNullOrWhiteSpace(txtSoyadi.Text))
        {
            MessageBox.Show("TC/SSN, Adı ve Soyadı zorunludur.", "Eksik Alan",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private void Temizle()
    {
        txtSsn.Clear(); txtAdi.Clear(); txtSoyadi.Clear();
        txtTelefon.Clear(); txtEmail.Clear();
        cmbOdeme.SelectedIndex = 0;
        txtAra.Clear();
    }

    private void SutunAdlari()
    {
        if (dgv.Columns.Count == 0) return;
        var map = new Dictionary<string, string>
        {
            {"MusteriSsn","TC/SSN"},{"Adi","Adı"},{"Soyadi","Soyadı"},
            {"Telefon","Telefon"},{"Email","E-posta"},{"OdemeDurumu","Ödeme Durumu"}
        };
        foreach (DataGridViewColumn col in dgv.Columns)
            if (map.TryGetValue(col.Name, out var ad)) col.HeaderText = ad;
    }
}

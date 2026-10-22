using OtelYonetimSistemi.Models;
using OtelYonetimSistemi.Repositories;

namespace OtelYonetimSistemi.Forms;

public class OdaForm : BaseForm
{
    private readonly OdaRepository _repo = new();

    private DataGridView dgv = new();
    private TextBox txtAra = new();
    private TextBox txtOdaNo = new();
    private TextBox txtOdaIsmi = new();
    private TextBox txtKat = new();
    private TextBox txtYatak = new();
    private CheckBox chkBalkon = new();
    private TextBox txtManzara = new();
    private TextBox txtFiyat = new();
    private Label lblSayac = new();

    public OdaForm()
    {
        Text = "Oda Yönetimi";
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
            Text = "ODA BİLGİLERİ",
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = ClrSubText,
            Location = new Point(15, 15),
            Size = new Size(255, 26)
        });

        int y = 50;

        pnlLeft.Controls.Add(MakeLabel("Oda No  *", new Point(15, y)));
        txtOdaNo = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtOdaNo); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Oda İsmi  *", new Point(15, y)));
        txtOdaIsmi = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtOdaIsmi); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Kat  *", new Point(15, y)));
        txtKat = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtKat); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Yatak Sayısı  *", new Point(15, y)));
        txtYatak = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtYatak); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Manzara", new Point(15, y)));
        txtManzara = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtManzara); y += 62;

        pnlLeft.Controls.Add(MakeLabel("Fiyat (₺)  *", new Point(15, y)));
        txtFiyat = MakeTextBox(new Point(15, y + 23));
        pnlLeft.Controls.Add(txtFiyat); y += 62;

        chkBalkon = MakeCheckBox("Balkon Var", new Point(15, y));
        pnlLeft.Controls.Add(chkBalkon); y += 42;

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

        var pnlAra = new Panel
        {
            Dock = DockStyle.Top,
            Height = 50,
            BackColor = ClrBackground
        };

        txtAra = MakeTextBox(new Point(0, 10), 320);
        txtAra.BackColor = Color.White;
        txtAra.PlaceholderText = "🔍  Ara  (oda no, isim, manzara, kat...)";
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

        lblSayac = new Label
        {
            Location = new Point(435, 16),
            AutoSize = true,
            ForeColor = Color.Black
        };

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

    private void Ara(string keyword)
    {
        try
        {
            var data = _repo.Search(keyword);
            dgv.DataSource = null;
            dgv.DataSource = data;
            SutunAdlari();
            lblSayac.Text = $"Bulunan: {data.Count} kayıt";
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Ekle()
    {
        if (!DogrulaBosAlanlari()) return;
        try { _repo.Insert(FormdenAl()); ShowSuccess("Oda eklendi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Guncelle()
    {
        if (string.IsNullOrWhiteSpace(txtOdaNo.Text))
        { MessageBox.Show("Güncellemek için listeden satır seçin."); return; }
        if (!DogrulaBosAlanlari()) return;
        try { _repo.Update(FormdenAl()); ShowSuccess("Oda güncellendi."); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Sil()
    {
        if (string.IsNullOrWhiteSpace(txtOdaNo.Text))
        { MessageBox.Show("Silmek için listeden satır seçin."); return; }
        if (!Confirm($"Oda No {txtOdaNo.Text} silinsin mi?")) return;
        try { _repo.Delete(int.Parse(txtOdaNo.Text)); ShowSuccess("Oda silindi."); Temizle(); Listele(); }
        catch (Exception ex) { ShowError(ex); }
    }

    private void GridSecimiDoldur(int rowIndex)
    {
        if (rowIndex < 0) return;
        var row = dgv.Rows[rowIndex];
        txtOdaNo.Text = row.Cells["OdaNo"].Value?.ToString() ?? "";
        txtOdaIsmi.Text = row.Cells["OdaIsmi"].Value?.ToString() ?? "";
        txtKat.Text = row.Cells["Kat"].Value?.ToString() ?? "";
        txtYatak.Text = row.Cells["Yatak"].Value?.ToString() ?? "";
        txtManzara.Text = row.Cells["Manzara"].Value?.ToString() ?? "";
        txtFiyat.Text = row.Cells["Fiyat"].Value?.ToString() ?? "";
        chkBalkon.Checked = Convert.ToBoolean(row.Cells["Balkon"].Value);
    }

    private Oda FormdenAl() => new()
    {
        OdaNo = int.Parse(txtOdaNo.Text.Trim()),
        OdaIsmi = txtOdaIsmi.Text.Trim(),
        Kat = int.Parse(txtKat.Text.Trim()),
        Yatak = int.Parse(txtYatak.Text.Trim()),
        Balkon = chkBalkon.Checked,
        Manzara = txtManzara.Text.Trim(),
        Fiyat = decimal.Parse(txtFiyat.Text.Trim())
    };

    private bool DogrulaBosAlanlari()
    {
        if (string.IsNullOrWhiteSpace(txtOdaNo.Text) ||
            string.IsNullOrWhiteSpace(txtOdaIsmi.Text) ||
            string.IsNullOrWhiteSpace(txtKat.Text) ||
            string.IsNullOrWhiteSpace(txtYatak.Text) ||
            string.IsNullOrWhiteSpace(txtFiyat.Text))
        {
            MessageBox.Show("Yıldızlı (*) alanlar zorunludur.", "Eksik Alan",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (!int.TryParse(txtOdaNo.Text, out _) ||
            !int.TryParse(txtKat.Text, out _) ||
            !int.TryParse(txtYatak.Text, out _) ||
            !decimal.TryParse(txtFiyat.Text, out _))
        {
            MessageBox.Show("Oda No / Kat / Yatak tam sayı, Fiyat ondalık sayı olmalıdır.", "Format Hatası",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private void Temizle()
    {
        txtOdaNo.Clear(); txtOdaIsmi.Clear(); txtKat.Clear();
        txtYatak.Clear(); txtManzara.Clear(); txtFiyat.Clear();
        chkBalkon.Checked = false;
        txtAra.Clear();
    }

    private void SutunAdlari()
    {
        if (dgv.Columns.Count == 0) return;
        var map = new Dictionary<string, string>
        {
            { "OdaNo",   "Oda No"   }, { "OdaIsmi", "Oda İsmi" },
            { "Kat",     "Kat"      }, { "Yatak",   "Yatak"    },
            { "Balkon",  "Balkon"   }, { "Manzara", "Manzara"  },
            { "Fiyat",   "Fiyat (₺)"}
        };
        foreach (DataGridViewColumn col in dgv.Columns)
            if (map.TryGetValue(col.Name, out var ad)) col.HeaderText = ad;
    }
}

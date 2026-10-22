using OtelYonetimSistemi.Database;
using OtelYonetimSistemi;

namespace OtelYonetimSistemi.Forms;

public class MainForm : Form
{
    private Label lblDurumu = new();

    public MainForm()
    {
        Text = "Otel Yönetim Sistemi";
        Size = new Size(700, 580);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(255, 255, 255);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Font = new Font("Segoe UI", 9f);

        BuildUI();
    }

    private void BuildUI()
    {
        var pnlHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 105,
            BackColor = Color.FromArgb(0, 27, 61)
        };

        pnlHeader.Controls.Add(new Label
        {
            Text = "OTEL YÖNETİM SİSTEMİ",
            Font = new Font("Segoe UI", 20f, FontStyle.Bold),
            ForeColor = Color.FromArgb(239, 239, 236),
            Location = new Point(20, 14),
            Size = new Size(640, 48),
            TextAlign = ContentAlignment.MiddleCenter
        });

        pnlHeader.Controls.Add(new Label
        {
            Text = "Rezervasyon  •  Oda  •  Müşteri  •  Çalışan  •  Temizlik",
            Font = new Font("Segoe UI", 10f),
            ForeColor = Color.FromArgb(239, 239, 236),
            Location = new Point(20, 62),
            Size = new Size(640, 26),
            TextAlign = ContentAlignment.MiddleCenter
        });

        var pnlCenter = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(239, 239, 236),
            Padding = new Padding(70, 22, 70, 10)
        };

        var menuItems = new (string Text, Color Color, EventHandler Click)[]
        {
            ("ODALAR",           Color.FromArgb(239, 239, 236), (_, _) => new OdaForm().Show()),
            ("MÜŞTERİLER",       Color.FromArgb(239, 239, 236), (_, _) => new MusteriForm().Show()),
            ("REZERVASYONLAR",   Color.FromArgb(239, 239, 236), (_, _) => new RezervasyonForm().Show()),
            ("ÇALIŞANLAR",       Color.FromArgb(239, 239, 236), (_, _) => new CalisanForm().Show()),
            ("ODA TEMİZLİK",     Color.FromArgb(239, 239, 236), (_, _) => new OdaTemizlikForm().Show()),
        };

        int y = 18;
        foreach (var (text, color, click) in menuItems)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(474, 58),
                Location = new Point(0, y),
                BackColor = color,
                ForeColor = Color.FromArgb(12, 12, 12),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top
            };
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.Black;
            btn.TabStop = false;
            var orig = color;
            btn.MouseEnter += (_, _) => btn.BackColor = ControlPaint.Light(orig, 0.2f);
            btn.MouseLeave += (_, _) => btn.BackColor = orig;
            btn.Click += (s, e) =>
            {
                if (!NetworkCheck.EnsureOnline(true)) return;
                click(s, e);
            };
            pnlCenter.Controls.Add(btn);
            btn.Left = (pnlCenter.ClientSize.Width - btn.Width) / 2;
            y += 68;
        }

        Controls.Add(pnlCenter);
        Controls.Add(pnlHeader);
    }

    
}

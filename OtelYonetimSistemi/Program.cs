using OtelYonetimSistemi.Forms;
using OtelYonetimSistemi.Database;
using OtelYonetimSistemi;
using System.Net.Http;

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

while (true)
{
    if (!NetworkCheck.IsInternetAvailable())
    {
        var r = MessageBox.Show("Ýnternet baðlantýsý gerekiyor. Uygulamayý baþlatmak için internet baðlantýnýzý kontrol edin.",
            "Baðlantý Hatasý", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
        if (r == DialogResult.Cancel) return;
        continue;
    }

    if (!DatabaseHelper.TestConnection())
    {
        var r = MessageBox.Show("Veritabanýna baðlanýlamýyor. Ýnternet baðlantýnýzý veya sunucu eriþimini kontrol edin.",
            "Veritabaný Hatasý", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
        if (r == DialogResult.Cancel) return;
        continue;
    }

    break;
}

Application.Run(new MainForm());

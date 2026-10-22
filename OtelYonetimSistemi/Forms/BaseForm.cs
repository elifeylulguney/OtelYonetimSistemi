using OtelYonetimSistemi;

namespace OtelYonetimSistemi.Forms;

public class BaseForm : Form
{
    protected static readonly Color ClrBackground = Color.FromArgb(239, 239, 236);
    protected static readonly Color ClrPanel = Color.FromArgb(0, 27, 61);
    protected static readonly Color ClrInput = Color.FromArgb(239, 239, 239);
    protected static readonly Color ClrText = Color.Black;
    protected static readonly Color ClrSubText = Color.FromArgb(239, 239, 236);
    protected static readonly Color ClrBlue = Color.FromArgb(0, 53, 122);

    public BaseForm()
    {
        BackColor = ClrBackground;
        ForeColor = ClrText;
        Font = new Font("Segoe UI", 9f);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
    }
    protected Label MakeLabel(string text, Point loc, int width = 240)
    {
        return new Label
        {
            Text = text,
            Location = loc,
            Size = new Size(width, 22),
            ForeColor = ClrSubText,
            Font = new Font("Segoe UI", 9f),
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    protected TextBox MakeTextBox(Point loc, int width = 240)
    {
        return new TextBox
        {
            Location = loc,
            Size = new Size(width, 28),
            BackColor = Color.FromArgb(239, 239, 236),
            ForeColor = ClrText,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10f)
        };
    }

    protected DateTimePicker MakeDatePicker(Point loc, int width = 240)
    {
        return new DateTimePicker
        {
            Location = loc,
            Size = new Size(width, 28),
            BackColor = Color.FromArgb(239, 239, 236),
            ForeColor = ClrText,
            Format = DateTimePickerFormat.Short,
            Font = new Font("Segoe UI", 10f)
        };
    }

    protected CheckBox MakeCheckBox(string text, Point loc)
    {
        return new CheckBox
        {
            Text = text,
            Location = loc,
            Size = new Size(200, 26),
            BackColor = Color.FromArgb(239, 239, 236),
            ForeColor = ClrText,
            Font = new Font("Segoe UI", 10f)
        };
    }

    protected ComboBox MakeComboBox(Point loc, int width = 240, string[] items = null!)
    {
        var cmb = new ComboBox
        {
            Location = loc,
            Size = new Size(width, 28),
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = Color.FromArgb(239, 239, 236),
            ForeColor = ClrText,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f)
        };
        if (items != null) cmb.Items.AddRange(items);
        if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
        return cmb;
    }

    protected Button MakeButton(string text, Color backColor, Point loc, int width = 115)
    {
        var btn = new Button
        {
            Text = text,
            Location = loc,
            Size = new Size(width, 36),
            BackColor = backColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        var orig = backColor;
        btn.MouseEnter += (_, _) => btn.BackColor = ControlPaint.Light(orig, 0.2f);
        btn.MouseLeave += (_, _) => btn.BackColor = orig;
        return btn;
    }

    protected void StyleGrid(DataGridView dgv)
    {
        dgv.BackgroundColor = ClrPanel;
        dgv.GridColor = Color.Black;
        dgv.DefaultCellStyle.BackColor = ClrInput;
        dgv.DefaultCellStyle.ForeColor = ClrText;
        dgv.DefaultCellStyle.SelectionBackColor = ClrBlue;
        dgv.DefaultCellStyle.SelectionForeColor = ClrSubText;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = ClrPanel;
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = ClrSubText;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        dgv.EnableHeadersVisualStyles = false;
        dgv.RowHeadersVisible = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.MultiSelect = false;
        dgv.ReadOnly = true;
        dgv.AllowUserToAddRows = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.BorderStyle = BorderStyle.None;
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.RowTemplate.Height = 28;
        dgv.Font = new Font("Segoe UI", 9.5f);
    }

    protected static void ShowSuccess(string msg) =>
        MessageBox.Show(msg, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

    protected static bool Confirm(string msg) =>
        MessageBox.Show(msg, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

    protected static void ShowError(Exception ex) =>
        MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

    protected bool EnsureOnline() => NetworkCheck.EnsureOnline(true);
}

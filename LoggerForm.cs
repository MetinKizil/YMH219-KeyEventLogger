using KeyEventLogger.Logging;

namespace KeyEventLogger;

/// <summary>Arayüz: butonlar, kayıt listesi ve durum göstergesi.</summary>
public class LoggerForm : Form
{
    private readonly KeyLog _log = new();
    private readonly Button _startButton = new() { Text = "Başlat", AutoSize = true };
    private readonly Button _stopButton = new() { Text = "Durdur", AutoSize = true, Enabled = false };
    private readonly Button _clearButton = new() { Text = "Temizle", AutoSize = true };
    private readonly Button _saveButton = new() { Text = "Kaydet (.txt)", AutoSize = true };
    private readonly ListBox _logList = new() { Dock = DockStyle.Fill, IntegralHeight = false };

    private readonly Label _statusLabel = new()
    {
        Dock = DockStyle.Top,
        Height = 36,
        TextAlign = ContentAlignment.MiddleCenter,
        Font = new Font("Segoe UI", 11, FontStyle.Bold)
    };

    public LoggerForm()
    {
        Text = "Key Event Logger (Eğitsel)";
        ClientSize = new Size(560, 440);
        MinimumSize = new Size(480, 320);
        StartPosition = FormStartPosition.CenterScreen;

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(6) };
        buttons.Controls.AddRange(new Control[] { _startButton, _stopButton, _clearButton, _saveButton });

        var notice = new Label
        {
            Dock = DockStyle.Bottom,
            Height = 40,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "Yalnızca bu pencere etkinken basılan tuşlar gösterilir. Parola yazmayın."
        };

        // Dock sırası: son eklenen önce yerleşir
        Controls.Add(_logList);
        Controls.Add(buttons);
        Controls.Add(_statusLabel);
        Controls.Add(notice);

        // Yalnızca formun kendi olayları dinlenir; global klavye kancası yoktur.
        KeyPreview = true;
        KeyDown += (_, e) => Capture("KeyDown", e);
        KeyUp += (_, e) => Capture("KeyUp", e);

        _startButton.Click += (_, _) => { _log.Start(); ShowStatus(true); _logList.Focus(); };
        _stopButton.Click += (_, _) => { _log.Stop(); ShowStatus(false); };

        _clearButton.Click += (_, _) => ClearLog();
        _saveButton.Click += (_, _) => SaveLog();

        ShowStatus(recording: false);
    }

    private void ClearLog()
    {
        _log.Clear();
        _logList.Items.Clear();
    }

    private void SaveLog()
    {
        if (_log.Entries.Count == 0)
        {
            MessageBox.Show(this, "Kaydedilecek olay yok.", Text);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "Metin dosyası (*.txt)|*.txt",
            FileName = $"tus-kayitlari-{DateTime.Now:yyyyMMdd-HHmmss}.txt"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            _log.Save(dialog.FileName);
            MessageBox.Show(this, "Kayıtlar kaydedildi.", Text);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show(this, "Dosya yazılamadı: " + ex.Message, "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Tab, ok ve Enter tuşları da kayıt sırasında KeyDown olayına ulaşsın.
    protected override bool ProcessDialogKey(Keys keyData) =>
        !_log.IsRecording && base.ProcessDialogKey(keyData);

    private void Capture(string eventName, KeyEventArgs e)
    {
        KeyLogEntry? entry = _log.Add(eventName, e.KeyData);
        if (entry is null) return;

        _logList.Items.Add(entry);
        _logList.TopIndex = _logList.Items.Count - 1; // son kayda kaydır

        // Boşluk/Enter butonları tetiklemesin; Alt+F4 çalışmaya devam etsin.
        if (!(e.Alt && e.KeyCode == Keys.F4))
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }

    private void ShowStatus(bool recording)
    {
        _statusLabel.Text = recording ? "● KAYIT AÇIK" : "■ Kayıt kapalı";
        _statusLabel.BackColor = recording ? Color.Firebrick : Color.Gainsboro;
        _statusLabel.ForeColor = recording ? Color.White : Color.Black;
        _startButton.Enabled = !recording;
        _stopButton.Enabled = recording;
    }
}

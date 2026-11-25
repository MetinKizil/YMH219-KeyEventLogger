namespace KeyEventLogger;

/// <summary>Arayüz: butonlar, kayıt listesi ve durum göstergesi.</summary>
public class LoggerForm : Form
{
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

        ShowStatus(recording: false);
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

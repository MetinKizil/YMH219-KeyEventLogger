namespace KeyEventLogger.Logging;

/// <summary>Kayıt durumunu ve olay listesini yönetir. Arayüzden bağımsızdır.</summary>
public class KeyLog
{
    private readonly List<KeyLogEntry> _entries = new();

    public bool IsRecording { get; private set; }
    public IReadOnlyList<KeyLogEntry> Entries => _entries;

    public void Start() => IsRecording = true;
    public void Stop() => IsRecording = false;

    /// <summary>Kayıt açıksa olayı ekler; kapalıysa null döner.</summary>
    public KeyLogEntry? Add(string eventName, Keys keyData)
    {
        if (!IsRecording) return null;

        var entry = new KeyLogEntry(DateTime.Now, eventName, keyData);
        _entries.Add(entry);
        return entry;
    }
}

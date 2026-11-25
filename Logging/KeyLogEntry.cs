namespace KeyEventLogger.Logging;

/// <summary>Tek bir tuş olayı: zaman, olay türü ve tuş adı.</summary>
public record KeyLogEntry(DateTime Time, string EventName, Keys KeyData)
{
    // Örnek: 2026-11-25 14:03:07.120 | KeyDown | A, Shift
    public override string ToString() =>
        $"{Time:yyyy-MM-dd HH:mm:ss.fff} | {EventName,-7} | {KeyData}";
}

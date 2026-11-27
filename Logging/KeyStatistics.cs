namespace KeyEventLogger.Logging;

/// <summary>Kayıtlardan toplam tuş sayısını ve en sık basılan tuşları hesaplar.</summary>
public class KeyStatistics
{
    public int Total { get; }
    public IReadOnlyList<(Keys Key, int Count)> Top { get; }

    public KeyStatistics(IEnumerable<KeyLogEntry> entries, int topCount = 5)
    {
        // Her basış bir KeyDown ve bir KeyUp üretir; çift saymamak için yalnızca KeyDown alınır.
        // KeyCode kullanıldığı için "A" ile "Shift+A" aynı tuş sayılır.
        List<Keys> presses = entries
            .Where(e => e.EventName == "KeyDown")
            .Select(e => e.KeyData & Keys.KeyCode)
            .ToList();

        Total = presses.Count;
        Top = presses
            .GroupBy(key => key)
            .Select(group => (Key: group.Key, Count: group.Count()))
            .OrderByDescending(item => item.Count)
            .Take(topCount)
            .ToList();
    }

    public override string ToString() => Total == 0
        ? "Toplam tuş: 0"
        : $"Toplam tuş: {Total}   |   En sık: " +
          string.Join(", ", Top.Select(item => $"{item.Key} ({item.Count})"));
}

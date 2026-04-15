using System;
using System.Windows.Media;

namespace Windwaker_coop.Models
{
    public class LogEntry
    {
        public string Message { get; }
        public string Timestamp { get; }
        public SolidColorBrush ColorBrush { get; }

        public LogEntry(string message, Color color)
        {
            Message = message;
            Timestamp = DateTime.Now.ToString("HH:mm:ss");
            ColorBrush = new SolidColorBrush(color);
            ColorBrush.Freeze();
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Windwaker_coop.Models;

namespace Windwaker_coop.Services
{
    public class LogService
    {
        private static readonly Lazy<LogService> _instance = new(() => new LogService());
        public static LogService Instance => _instance.Value;

        public ObservableCollection<LogEntry> LogEntries { get; } = new();

        public void AddLog(string message, Color color)
        {
            var entry = new LogEntry(message, color);

            if (Application.Current?.Dispatcher != null && !Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => LogEntries.Add(entry)));
                return;
            }

            LogEntries.Add(entry);
        }

        public void Clear()
        {
            if (Application.Current?.Dispatcher != null && !Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => LogEntries.Clear()));
                return;
            }

            LogEntries.Clear();
        }
    }
}

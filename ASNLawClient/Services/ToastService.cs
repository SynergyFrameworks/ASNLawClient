using System;
using System.Collections.Generic;

namespace ASNLawClient.Client.Services
{
    public enum ToastLevel
    {
        Info,
        Success,
        Warning,
        Error
    }

    public class ToastMessage
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Timestamp { get; } = DateTime.Now;
        public string Title { get; set; }
        public string Message { get; set; }
        public ToastLevel Level { get; set; }
        public bool AutoClose { get; set; } = true;
        public int Timeout { get; set; } = 5000; // 5 seconds by default
    }

    public class ToastService : IDisposable
    {
        public event Action<ToastMessage> OnShow;
        public event Action<Guid> OnHide;
        private readonly Dictionary<Guid, System.Timers.Timer> _timers = new Dictionary<Guid, System.Timers.Timer>();

        public void ShowToast(string message, string title, ToastLevel level, bool autoClose = true, int timeout = 5000)
        {
            Console.WriteLine($"ShowToast called: {title} - {message}");

            var toast = new ToastMessage
            {
                Message = message,
                Title = title,
                Level = level,
                AutoClose = autoClose,
                Timeout = timeout
            };

            OnShow?.Invoke(toast);

            if (autoClose)
            {
                var timer = new System.Timers.Timer(timeout);
                timer.Elapsed += (sender, args) => HideToast(toast.Id);
                timer.AutoReset = false;
                timer.Start();
                _timers.Add(toast.Id, timer);
            }
        }

        public void ShowInfo(string message, string title = "Information", bool autoClose = true, int timeout = 5000)
        {
            ShowToast(message, title, ToastLevel.Info, autoClose, timeout);
        }

        public void ShowSuccess(string message, string title = "Success", bool autoClose = true, int timeout = 5000)
        {
            ShowToast(message, title, ToastLevel.Success, autoClose, timeout);
        }

        public void ShowWarning(string message, string title = "Warning", bool autoClose = true, int timeout = 8000)
        {
            ShowToast(message, title, ToastLevel.Warning, autoClose, timeout);
        }

        public void ShowError(string message, string title = "Error", bool autoClose = false, int timeout = 0)
        {
            ShowToast(message, title, ToastLevel.Error, autoClose, timeout);
        }

        public void HideToast(Guid toastId)
        {
            OnHide?.Invoke(toastId);

            if (_timers.ContainsKey(toastId))
            {
                _timers[toastId].Dispose();
                _timers.Remove(toastId);
            }
        }



        public void Dispose()
        {
            foreach (var timer in _timers.Values)
            {
                timer.Dispose();
            }
            _timers.Clear();
        }
    }
}
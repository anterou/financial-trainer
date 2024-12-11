using Plugin.LocalNotification;

namespace financialapp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        private void ScheduleDailyNotification()
        {
            var notification = new NotificationRequest
            {
                NotificationId = 1000,
                Title = "Напоминание",
                Description = "Инвестируйте сейчас, чтобы не жалеть завтра",
                Schedule = { NotifyTime = DateTime.Now.AddSeconds(10), 
                // Установи время уведомления
                RepeatType = NotificationRepeat.Daily }
            };
            LocalNotificationCenter.Current.Show(notification);

        }
    }
}

using System.Collections.ObjectModel;
using System.Text.Json;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;

namespace financialapp
{
    public class Bank
    {
        public string BankName { get; set; }
        public string Rate { get; set; }
        public string Period { get; set; }
        public string Amount { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Bank> banks;
        public ObservableCollection<Bank> Banks
        {
            get => banks;
            set
            {
                banks = value;
                OnPropertyChanged();
            }
        }

        private bool isResultsVisible;
        public bool IsResultsVisible
        {
            get => isResultsVisible;
            set
            {
                isResultsVisible = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            Banks = new ObservableCollection<Bank>();
            BindingContext = this;
            Debug.WriteLine("Инициализация MainPage");
            LoadJsonData();
            Debug.WriteLine("Данные загружены");
        }

        private void LoadJsonData()
        {
            var assembly = typeof(MainPage).Assembly;
            using Stream stream = assembly.GetManifestResourceStream("financialapp.Resources.Raw.deposits.json");
            using StreamReader reader = new StreamReader(stream);
            string jsonString = reader.ReadToEnd();

            Debug.WriteLine("JSON строка:");
            Debug.WriteLine(jsonString);

            var banks = JsonSerializer.Deserialize<List<Bank>>(jsonString);
            Banks = new ObservableCollection<Bank>(banks);

            Debug.WriteLine("Данные из JSON файла загружены");
            Debug.WriteLine($"Количество банков в коллекции: {Banks?.Count}");

            // Отладочный вывод для каждого банка
            foreach (var bank in Banks)
            {
                Debug.WriteLine($"BankName: {bank.BankName}, Rate: {Regex.Replace(bank.Rate, @"\D", "")}, Period: {bank.Period}, Amount: {bank.Amount}");
            }

            // Динамическое добавление элементов в Grid
            AddBanksToGrid();
        }

        private void AddBanksToGrid()
        {
            ResultsGrid.Children.Clear();
            ResultsGrid.RowDefinitions.Clear();
            ResultsGrid.ColumnDefinitions.Clear();

            int maxItemsPerColumn = 4;
            int columnCount = (Banks.Count + maxItemsPerColumn - 1) / maxItemsPerColumn;

            for (int i = 0; i < columnCount; i++)
            {
                ResultsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            for (int i = 0; i < Banks.Count; i++)
            {
                int row = i % maxItemsPerColumn;
                int column = i / maxItemsPerColumn;

                if (row >= ResultsGrid.RowDefinitions.Count)
                {
                    ResultsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }

                var bank = Banks[i];
                var stackLayout = new StackLayout
                {
                    Padding = new Thickness(10),
                    BackgroundColor = Colors.White,
                    Margin = new Thickness(0, 0, 0, 10),
                    Children =
            {
                new Label { Text = bank.BankName, FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = Colors.Black },
                new Label { Text = bank.Rate, TextColor = Colors.Black },
                new Label { Text = bank.Period, TextColor = Colors.Black },
                new Label { Text = bank.Amount, TextColor = Colors.Black }
            }
                };

                // Call a separate method to calculate and update expected income
                UpdateExpectedIncome(stackLayout, bank);

                ResultsGrid.Children.Add(stackLayout);
                Grid.SetRow(stackLayout, row);
                Grid.SetColumn(stackLayout, column);
            }
        }



        private void UpdateExpectedIncome(StackLayout stackLayout, Bank bank)
        {
            if (decimal.TryParse(MainEntry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) &&
                int.TryParse(Time.Text, out int period) &&
                decimal.TryParse(Regex.Replace(bank.Rate, @"\D", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rateParsed))
            {
                decimal rate = rateParsed / 100;
                decimal expectedIncome = amount * rate / 12 * period;

                // Remove any existing expected income label before adding a new one
                var existingIncomeLabel = stackLayout.Children.FirstOrDefault(c => (c as Label)?.Text?.StartsWith("Ожидаемый доход") == true);
                if (existingIncomeLabel != null)
                {
                    stackLayout.Children.Remove(existingIncomeLabel);
                }

                stackLayout.Children.Add(new Label { Text = $"Ожидаемый доход за период: {expectedIncome}", TextColor = Colors.Black });
            }
            else
            {
                // Optionally, handle cases where parsing fails (e.g., invalid input)
                var existingIncomeLabel = stackLayout.Children.FirstOrDefault(c => (c as Label)?.Text?.StartsWith("Ожидаемый доход") == true);
                if (existingIncomeLabel != null)
                {
                    stackLayout.Children.Remove(existingIncomeLabel);
                }
            }
        }





        private void OnMenuButtonClicked(object sender, EventArgs e)
        {
            ShowMenu();
        }

        private void ShowMenu()
        {
            MenuBackground.IsVisible = true;
            ContextMenu.IsVisible = true;

            // Анимация появления
            ContextMenu.Opacity = 0;
            ContextMenu.TranslationY = -10;

            Animation animation = new Animation();
            animation.Add(0, 1, new Animation((value) => ContextMenu.Opacity = value, 0, 1));
            animation.Add(0, 1, new Animation((value) => ContextMenu.TranslationY = value, -10, 0));
            animation.Commit(this, "MenuAnimation", 16, 250, Easing.CubicOut);
        }

        private void HideMenu()
        {
            MenuBackground.IsVisible = false;
            ContextMenu.IsVisible = false;
            ContextMenu.TranslationY = 0;
        }

        private void OnBackgroundTapped(object sender, EventArgs e)
        {
            HideMenu();
        }

        // Обработчики пунктов меню
        private async void OnFinancialAnalysisClicked(object sender, EventArgs e)
        {
            IsResultsVisible = true;
            HideMenu();
            // Логика для отображения финансового анализа
            await DisplayAlert("Финансовый анализ", "Открыт раздел финансового анализа", "OK");
        }

        private async void OnInitialDataClicked(object sender, EventArgs e)
        {
            HideMenu();
            // Логика для отображения исходных данных
            await DisplayAlert("Исходные данные", "Открыт раздел исходных данных", "OK");
        }

        private async void OnProfitChartsClicked(object sender, EventArgs e)
        {
            HideMenu();
            // Логика для отображения графиков доходности
            await DisplayAlert("Графики доходности", "Открыт раздел графиков доходности", "OK");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            HideMenu();
        }
    }
}

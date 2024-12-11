using System.Collections.ObjectModel;
using System.Text.Json;
using System.IO;
using System.Diagnostics;

namespace financialapp
{
    public class InvestmentFund
    {
        public string FundName { get; set; }
        public string AnnualReturn { get; set; }
    }

    public partial class pai : ContentPage
    {
        private ObservableCollection<InvestmentFund> funds;
        public ObservableCollection<InvestmentFund> Funds
        {
            get => funds;
            set
            {
                funds = value;
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

        public pai()
        {
            InitializeComponent();
            Funds = new ObservableCollection<InvestmentFund>();
            BindingContext = this;
            Debug.WriteLine("Инициализация pai");
            LoadJsonData();
            Debug.WriteLine("Данные загружены");
        }

        private void LoadJsonData()
        {
            var assembly = typeof(pai).Assembly;
            using Stream stream = assembly.GetManifestResourceStream("financialapp.Resources.Raw.investment_funds.json");
            using StreamReader reader = new StreamReader(stream);
            string jsonString = reader.ReadToEnd();

            Debug.WriteLine("JSON строка:");
            Debug.WriteLine(jsonString);

            var funds = JsonSerializer.Deserialize<List<InvestmentFund>>(jsonString);
            Funds = new ObservableCollection<InvestmentFund>(funds);

            Debug.WriteLine("Данные из JSON файла загружены");
            Debug.WriteLine($"Количество фондов в коллекции: {Funds?.Count}");

            // Отладочный вывод для каждого фонда
            foreach (var fund in Funds)
            {
                Debug.WriteLine($"FundName: {fund.FundName}, AnnualReturn: {fund.AnnualReturn}");
            }

            // Динамическое добавление элементов в Grid
            AddFundsToGrid();
        }

        private void AddFundsToGrid()
        {
            ResultsGrid.Children.Clear();
            ResultsGrid.RowDefinitions.Clear();
            ResultsGrid.ColumnDefinitions.Clear();

            int maxItemsPerColumn = 4;
            int columnCount = (Funds.Count + maxItemsPerColumn - 1) / maxItemsPerColumn;

            for (int i = 0; i < columnCount; i++)
            {
                ResultsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            for (int i = 0; i < Funds.Count; i++)
            {
                int row = i % maxItemsPerColumn;
                int column = i / maxItemsPerColumn;

                if (row >= ResultsGrid.RowDefinitions.Count)
                {
                    ResultsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }

                var fund = Funds[i];
                var stackLayout = new StackLayout
                {
                    Padding = new Thickness(10),
                    BackgroundColor = Colors.White,
                    Margin = new Thickness(0, 0, 0, 10),
                    Children =
                    {
                        new Label { Text = fund.FundName, FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = Colors.Black },
                        new Label { Text = fund.AnnualReturn, TextColor = Colors.Black }
                    }
                };

                ResultsGrid.Children.Add(stackLayout);
                Grid.SetRow(stackLayout, row);
                Grid.SetColumn(stackLayout, column);
            }
        }
    }
}

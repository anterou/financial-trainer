using System.Text.Json;
namespace financialapp;
using financialapp.Models;
using System.Reflection;

public partial class MainPage : ContentPage
{
    View backup = new StackLayout();
    
    public MainPage()
    {
        InitializeComponent();
        PageTitle.Text = "Анализ инвестиций";
        ShowPanel(nameof(InputPanel));
        backup = InputPanel;


    }

    private async void LoadBankData()
    {
        try
        {
            // Получаем текущую сборку
            var assembly = Assembly.GetExecutingAssembly();

            // Путь к встроенному ресурсу
            const string resourcePath = "financialapp.Resources.Raw.deposits.json";

            // Открываем поток ресурса
            using Stream? stream = assembly.GetManifestResourceStream(resourcePath);

            // Проверяем, что ресурс найден
            if (stream == null)
            {
                await DisplayAlert("Ошибка", "Ресурс deposits.json не найден", "OK");
                return;
            }

            // Читаем содержимое ресурса
            using StreamReader reader = new StreamReader(stream);
            string jsonContent = await reader.ReadToEndAsync();

            // Десериализуем JSON
            var deposits = JsonSerializer.Deserialize<List<Deposit>>(jsonContent);

            // Проверяем, что данные не пустые
            if (deposits == null || deposits.Count == 0)
            {
                await DisplayAlert("Ошибка", "Нет данных для отображения", "OK");
                return;
            }

            // Получаем значения фильтров
            int.TryParse(AmountEntry.Text, out int filterAmount);
            int.TryParse(DurationEntry.Text, out int filterDuration);

            // Преобразуем длительность депозита из строки в дни
            List<Deposit> filteredDeposits = deposits.Where(deposit =>
            {
                int.TryParse(deposit.Period.Split(' ')[0], out int depositDays);
                bool matchesAmount = filterAmount == 0 || (filterAmount >= deposit.AmountFrom && filterAmount <= deposit.AmountTo);
                bool matchesDuration = filterDuration == 0 || depositDays >= filterDuration;
                return matchesAmount && matchesDuration;
            }).ToList();

            // Проверяем, что есть совпадения после фильтрации
            if (!filteredDeposits.Any())
            {
                await DisplayAlert("Ошибка", "Нет вкладов, соответствующих заданным критериям", "OK");
                return;
            }

            // Очищаем предыдущие элементы интерфейса
            BankList.Children.Clear();
            Button backFilter = new Button { Text = "Вернуться к фильтрам поиска банка"};
            backFilter.Clicked += BackFilter_Clicked;
            BankList.Children.Add(backFilter);
            // Создаем элементы для каждого отфильтрованного депозита
            foreach (var deposit in filteredDeposits)
            {
                var frame = new Frame
                {
                    CornerRadius = 20,
                    Padding = 10,
                    BackgroundColor = Colors.White,
                    Margin = new Thickness(0, 5)
                };

                var stackLayout = new VerticalStackLayout();
                stackLayout.Children.Add(new Label
                {
                    Text = deposit.BankName,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    TextColor = Colors.Black
                });
                stackLayout.Children.Add(new Label
                {
                    Text = $"{deposit.AmountFrom:n0} ₽ – {deposit.AmountTo:n0} ₽\n{deposit.Rate}%\n{deposit.Period}",
                    TextColor = Colors.Gray
                });
                
                frame.Content = stackLayout;
                
                BankList.Children.Add(frame);
                
            }
           
        }
        catch (Exception ex)
        {
            // Обработка исключений
            await DisplayAlert("Ошибка", $"Произошла ошибка при загрузке данных: {ex.Message}", "OK");
        }
    }

    private void BackFilter_Clicked(object? sender, EventArgs e)
    {
        InputPanel.IsVisible = true;
       //gridPrikol.Children.Add(backup);
    }

    private async void LoadMutualFundsData()
    {
        try
        {
            // Получаем текущую сборку
            var assembly = Assembly.GetExecutingAssembly();

            // Путь к встроенному ресурсу
            const string resourcePath = "financialapp.Resources.Raw.mutual_funds.json";

            // Открываем поток ресурса
            using Stream? stream = assembly.GetManifestResourceStream(resourcePath);

            // Проверяем, что ресурс найден
            if (stream == null)
            {
                await DisplayAlert("Ошибка", "Ресурс mutual_funds.json не найден", "OK");
                return;
            }

            // Читаем содержимое ресурса
            using StreamReader reader = new StreamReader(stream);
            string jsonContent = await reader.ReadToEndAsync();

            // Десериализуем JSON
            var mutualFunds = JsonSerializer.Deserialize<List<MutualFund>>(jsonContent);

            // Проверяем, что данные не пустые
            if (mutualFunds == null || mutualFunds.Count == 0)
            {
                await DisplayAlert("Ошибка", "Нет данных для отображения", "OK");
                return;
            }

            // Очищаем предыдущие элементы интерфейса
            MutualFundsList.Children.Clear();

            // Создаем элементы для каждого фонда
            foreach (var fund in mutualFunds)
            {
                var frame = new Frame
                {
                    CornerRadius = 20,
                    Padding = 10,
                    BackgroundColor = Colors.White,
                    Margin = new Thickness(0, 5)
                };

                var stackLayout = new VerticalStackLayout();
                stackLayout.Children.Add(new Label
                {
                    Text = $"{fund.Name} ({fund.Company})",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    TextColor = Colors.Black
                });
                stackLayout.Children.Add(new Label
                {
                    Text = $"Доходность за год: {fund.AnnualReturn}\nКомиссия: {fund.ManagementFee}\nМин. вложение: {fund.MinimumInvestment}",
                    TextColor = Colors.Gray
                });

                frame.Content = stackLayout;
                MutualFundsList.Children.Add(frame);
            }
        }
        catch (Exception ex)
        {
            // Обработка исключений
            await DisplayAlert("Ошибка", $"Произошла ошибка при загрузке данных: {ex.Message}", "OK");
        }
    }


    private async void LoadSharesData()
    {
        try
        {
            // Получаем текущую сборку
            var assembly = Assembly.GetExecutingAssembly();

            // Путь к встроенному ресурсу
            const string resourcePath = "financialapp.Resources.Raw.shares.json";

            // Открываем поток ресурса
            using Stream? stream = assembly.GetManifestResourceStream(resourcePath);

            // Проверяем, что ресурс найден
            if (stream == null)
            {
                await DisplayAlert("Ошибка", "Ресурс shares.json не найден", "OK");
                return;
            }

            // Читаем содержимое ресурса
            using StreamReader reader = new StreamReader(stream);
            string jsonContent = await reader.ReadToEndAsync();

            // Десериализуем JSON
            var shares = JsonSerializer.Deserialize<List<Share>>(jsonContent);

            // Проверяем, что данные не пустые
            if (shares == null || shares.Count == 0)
            {
                await DisplayAlert("Ошибка", "Нет данных для отображения", "OK");
                return;
            }

            // Очищаем предыдущие элементы интерфейса
            SharesList.Children.Clear();

            // Создаем элементы для каждого объекта акции
            foreach (var share in shares)
            {
                var frame = new Frame
                {
                    CornerRadius = 20,
                    Padding = 10,
                    BackgroundColor = Colors.White,
                    Margin = new Thickness(0, 5)
                };

                var stackLayout = new VerticalStackLayout();
                stackLayout.Children.Add(new Label
                {
                    Text = $"{share.Name} ({share.Ticker})",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    TextColor = Colors.Black
                });
                stackLayout.Children.Add(new Label
                {
                    Text = $"Цена: {share.Price}\nИзменение: {share.PriceChange}\nДоходность за год: {share.AnnualReturn}\nРиск: {share.Risk}",
                    TextColor = Colors.Gray
                });

                frame.Content = stackLayout;
                SharesList.Children.Add(frame);
            }
        }
        catch (Exception ex)
        {
            // Обработка исключений
            await DisplayAlert("Ошибка", $"Произошла ошибка при загрузке данных: {ex.Message}", "OK");
        }
    }

    private async void LoadCurrencyData()
    {
        try
        {
            // Получаем текущую сборку
            var assembly = Assembly.GetExecutingAssembly();

            // Путь к встроенному ресурсу
            const string resourcePath = "financialapp.Resources.Raw.currencies.json";

            // Открываем поток ресурса
            using Stream? stream = assembly.GetManifestResourceStream(resourcePath);

            // Проверяем, что ресурс найден
            if (stream == null)
            {
                await DisplayAlert("Ошибка", "Ресурс currencies.json не найден", "OK");
                return;
            }

            // Читаем содержимое ресурса
            using StreamReader reader = new StreamReader(stream);
            string jsonContent = await reader.ReadToEndAsync();

            // Десериализуем JSON
            var currencies = JsonSerializer.Deserialize<List<Currency>>(jsonContent);

            // Проверяем, что данные не пустые
            if (currencies == null || currencies.Count == 0)
            {
                await DisplayAlert("Ошибка", "Нет данных для отображения", "OK");
                return;
            }

            // Очищаем список интерфейса
            CurrencyList.Children.Clear();

            // Создаем элементы для каждой валюты
            foreach (var currency in currencies)
            {
                if (currency.Rates == null || currency.Rates.Count < 2)
                    continue;

                var previousRate = currency.Rates[0];
                var latestRate = currency.Rates[1];

                var frame = new Frame
                {
                    CornerRadius = 20,
                    Padding = 10,
                    BackgroundColor = Colors.White,
                    Margin = new Thickness(0, 5)
                };

                var stackLayout = new VerticalStackLayout();
                stackLayout.Children.Add(new Label
                {
                    Text = $"{currency.Name} ({currency.Code})",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    TextColor = Colors.Black
                });
                stackLayout.Children.Add(new Label
                {
                    Text = $"Курс на {previousRate.Date}: {previousRate.Value}",
                    TextColor = Colors.Gray
                });
                stackLayout.Children.Add(new Label
                {
                    Text = $"Курс на {latestRate.Date}: {latestRate.Value}",
                    TextColor = Colors.Gray
                });

                frame.Content = stackLayout;
                CurrencyList.Children.Add(frame);
            }
        }
        catch (Exception ex)
        {
            // Обработка ошибок
            await DisplayAlert("Ошибка", $"Произошла ошибка при загрузке данных: {ex.Message}", "OK");
        }
    }




    private async void LoadMetalsData()
    {
        try
        {
            // Получаем текущую сборку
            var assembly = Assembly.GetExecutingAssembly();

            // Путь к встроенному ресурсу
            const string resourcePath = "financialapp.Resources.Raw.metals.json";

            // Открываем поток ресурса
            using Stream? stream = assembly.GetManifestResourceStream(resourcePath);

            // Проверяем, что ресурс найден
            if (stream == null)
            {
                await DisplayAlert("Ошибка", "Ресурс metals.json не найден", "OK");
                return;
            }

            // Читаем содержимое ресурса
            using StreamReader reader = new StreamReader(stream);
            string jsonContent = await reader.ReadToEndAsync();

            // Десериализуем JSON
            var metals = JsonSerializer.Deserialize<List<Metal>>(jsonContent);

            // Проверяем, что данные не пустые
            if (metals == null || metals.Count == 0)
            {
                await DisplayAlert("Ошибка", "Нет данных для отображения", "OK");
                return;
            }

            // Очищаем предыдущие элементы интерфейса
            MetalsList.Children.Clear();

            // Создаем элементы для каждого металла
            foreach (var metal in metals)
            {
                if (metal.Prices == null || metal.Prices.Count < 2)
                    continue;

                var previousPrice = metal.Prices[0];
                var latestPrice = metal.Prices[1];

                var frame = new Frame
                {
                    CornerRadius = 20,
                    Padding = 10,
                    BackgroundColor = Colors.White,
                    Margin = new Thickness(0, 5)
                };

                var stackLayout = new VerticalStackLayout();
                stackLayout.Children.Add(new Label
                {
                    Text = $"{metal.Name} ({metal.Code})",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    TextColor = Colors.Black
                });
                stackLayout.Children.Add(new Label
                {
                    Text = $"Цена на {previousPrice.Date}: {previousPrice.Value}",
                    TextColor = Colors.Gray
                });
                stackLayout.Children.Add(new Label
                {
                    Text = $"Цена на {latestPrice.Date}: {latestPrice.Value}",
                    TextColor = Colors.Gray
                });

                frame.Content = stackLayout;
                MetalsList.Children.Add(frame);
            }
        }
        catch (Exception ex)
        {
            // Обработка исключений
            await DisplayAlert("Ошибка", $"Произошла ошибка при загрузке данных: {ex.Message}", "OK");
        }
    }



    private void OnMutualFundsClicked(object sender, EventArgs e)
    {
        PageTitle.Text = "Паи";
        ShowPanel(nameof(MutualFundsPanel));
        LoadMutualFundsData();

    }
    private void OnDepositsClicked(object sender, EventArgs e)
    {
        PageTitle.Text = "Вклады";
        ShowPanel(nameof(BankPanel));
        LoadBankData();

    }
    private void OnCurrencyClicked(object sender, EventArgs e)
    {
        PageTitle.Text = "Валюта";
        ShowPanel(nameof(CurrencyPanel));
        LoadCurrencyData();
    }
    private void OnMetalsClicked(object sender, EventArgs e)
    {
        PageTitle.Text = "Металлы";
        ShowPanel(nameof(MetalsPanel));
        LoadMetalsData();
    }

    private void OnSharesClicked(object sender, EventArgs e)
    {
        PageTitle.Text = "Акции";
        ShowPanel(nameof(SharesPanel));
        LoadSharesData();
    }

    private void ShowPanel(string panelName)
    {
        BankPanel.IsVisible = panelName == nameof(BankPanel);
        SharesPanel.IsVisible = panelName == nameof(SharesPanel);
        MutualFundsPanel.IsVisible = panelName == nameof(MutualFundsPanel);
        CurrencyPanel.IsVisible = panelName == nameof(CurrencyPanel);
        MetalsPanel.IsVisible = panelName == nameof(MetalsPanel);
        InputPanel.IsVisible = panelName == nameof(InputPanel);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // Скрываем панель с инпутами
        InputPanel.IsVisible = false;

        PageTitle.Text = "Вклады";
        ShowPanel(nameof(BankPanel));
        LoadBankData();
    }
}

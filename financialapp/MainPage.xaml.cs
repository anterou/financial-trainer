namespace financialapp
{
    public partial class MainPage : ContentPage
    {
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

        public string InterestRate { get; set; }
        public string RiskLevel { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
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
            isResultsVisible = true;
            ResultsFrame.IsVisible = true;
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

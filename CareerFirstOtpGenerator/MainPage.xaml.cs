using CareerFirstOtpGenerator.Localization;

namespace CareerFirstOtpGenerator;

public partial class MainPage : ContentPage
{
    private System.Timers.Timer? _timer;
    private bool _isDarkMode = true;
    private bool _isArabic = true;

    private static readonly Color DarkBg = Color.FromArgb("#040918");
    private static readonly Color LightBg = Color.FromArgb("#FBFAFC");
    private static readonly Color DarkSurface = Color.FromArgb("#090F20");
    private static readonly Color LightSurface = Color.FromArgb("#FFFFFF");
    private static readonly Color DarkBorder = Color.FromArgb("#0E122B");
    private static readonly Color LightBorder = Color.FromArgb("#ABABAB");

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _isDarkMode = true;
        _isArabic = true;
        ApplyTheme();
        ApplyLanguage();
        StartTimer();
        UpdateCode();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
        _timer?.Dispose();
    }

    private void StartTimer()
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (s, e) =>
        {
            MainThread.BeginInvokeOnMainThread(UpdateCode);
        };
        _timer.Start();
    }

    private void UpdateCode()
    {
        var code = TotpService.GetCurrentCode();
        var remaining = TotpService.GetRemainingSeconds();

        Digit1.Text = code[0].ToString();
        Digit2.Text = code[1].ToString();
        Digit3.Text = code[2].ToString();
        Digit4.Text = code[3].ToString();
        Digit5.Text = code[4].ToString();
        Digit6.Text = code[5].ToString();

        CountdownLabel.Text = $"{remaining:00}";
        TimerProgress.Progress = remaining / 30.0;

        var expiring = remaining <= 5;
        var accentColor = expiring ? Color.FromArgb("#FF5252") : Color.FromArgb("#0B7A75");

        TimerProgress.ProgressColor = accentColor;
        CountdownLabel.TextColor = accentColor;

        var boxBorder = expiring
            ? Color.FromArgb("#FF5252")
            : (_isDarkMode ? DarkBorder : LightBorder);

        foreach (var box in new[] { Box1, Box2, Box3, Box4, Box5, Box6 })
            box.Stroke = new SolidColorBrush(boxBorder);
    }
    private async void GenerateBtn_Clicked(object sender, EventArgs e)
    {
        GenerateBtn.IsEnabled = false;
        var culture = _isArabic
            ? new System.Globalization.CultureInfo("ar")
            : new System.Globalization.CultureInfo("en");

        var code = TotpService.GetCurrentCode();
        await Clipboard.SetTextAsync(code);

        await GenerateBtn.ScaleToAsync(0.97, 80);
        await GenerateBtn.ScaleToAsync(1.0, 80);

        GenerateBtn.Text = AppResources.ResourceManager.GetString("Copied", culture);

        await Task.Delay(1500);

        GenerateBtn.Text = AppResources.ResourceManager.GetString("CopyCode", culture);
        GenerateBtn.IsEnabled = true;
    }
    private void OnThemeToggle(object? sender, TappedEventArgs e)
    {
        _isDarkMode = !_isDarkMode;
        ApplyTheme();
        UpdateCode();
    }

    private void ApplyTheme()
    {
        if (_isDarkMode)
        {
            BackgroundColor = DarkBg;
            ThemeBtn.Text = "🌙";
            foreach (var box in new[] { Box1, Box2, Box3, Box4, Box5, Box6 })
            {
                box.BackgroundColor = DarkSurface;
                box.Stroke = new SolidColorBrush(DarkBorder);
            }
            foreach (var d in new[] { Digit1, Digit2, Digit3, Digit4, Digit5, Digit6 })
                d.TextColor = Colors.White;
            TitleLabel.TextColor = Colors.White;
            SecureVerificationLabel.TextColor = Color.FromArgb("#0B7A75");
            SubtitleLabel.TextColor = Color.FromArgb("#9E9CA1");
            ExpiresLabel.TextColor = Color.FromArgb("#9E9CA1");
            OtpPasswordLabel.TextColor = Color.FromArgb("#9E9CA1");
            FootnoteLabel.TextColor = Color.FromArgb("#2A2E3E");
            TimerProgress.BackgroundColor = Color.FromArgb("#12182A");
        }
        else
        {
            BackgroundColor = LightBg;
            ThemeBtn.Text = "☀️";
            foreach (var box in new[] { Box1, Box2, Box3, Box4, Box5, Box6 })
            {
                box.BackgroundColor = LightSurface;
                box.Stroke = new SolidColorBrush(LightBorder);
            }
            foreach (var d in new[] { Digit1, Digit2, Digit3, Digit4, Digit5, Digit6 })
                d.TextColor = Colors.Black;
            TitleLabel.TextColor = Colors.Black;
            SecureVerificationLabel.TextColor = Color.FromArgb("#0B7A75");
            SubtitleLabel.TextColor = Color.FromArgb("#3A3A3A");
            ExpiresLabel.TextColor = Color.FromArgb("#3A3A3A");
            OtpPasswordLabel.TextColor = Color.FromArgb("#3A3A3A");
            FootnoteLabel.TextColor = Color.FromArgb("#8C93A1");
            TimerProgress.BackgroundColor = Color.FromArgb("#F1EDFB");
        }
    }

    private void OnLanguageToggle(object? sender, TappedEventArgs e)
    {
        _isArabic = !_isArabic;
        ApplyLanguage();
    }

    private void ApplyLanguage()
    {
        var culture = _isArabic
            ? new System.Globalization.CultureInfo("ar")
            : new System.Globalization.CultureInfo("en");

        System.Globalization.CultureInfo.CurrentUICulture = culture;

        SecureVerificationLabel.Text = AppResources.SecureVerification;
        TitleLabel.Text = AppResources.Title;
        SubtitleLabel.Text = AppResources.Subtitle;
        OtpPasswordLabel.Text = AppResources.OtpPassword;
        ExpiresLabel.Text = AppResources.ExpiresIn;
        GenerateBtn.Text = AppResources.CopyCode;
        FootnoteLabel.Text = AppResources.Footnote;
        LangBtn.Text = AppResources.LangCode;
    }
}
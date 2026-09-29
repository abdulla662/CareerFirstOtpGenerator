namespace CareerFirstOtpGenerator;

public partial class MainPage : ContentPage
{
    private System.Timers.Timer _timer;
    private bool _isAnimatingRing = false;
    private bool _isDarkMode = true;
    private bool _isArabic = true;

    private static readonly Color DarkBg = Color.FromArgb("#060818");
    private static readonly Color DarkCard = Color.FromArgb("#0D1535");
    private static readonly Color DarkCodeBg = Color.FromArgb("#060818");
    private static readonly Color LightBg = Color.FromArgb("#F0F4FF");
    private static readonly Color LightCard = Color.FromArgb("#FFFFFF");
    private static readonly Color LightCodeBg = Color.FromArgb("#E8EEFF");

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        OtpLabel.Opacity = 0;
        OtpLabel.TranslationY = 20;

        await Task.Delay(200);
        await OtpLabel.FadeToAsync(1, 600);
        await OtpLabel.TranslateToAsync(0, 0, 400, Easing.CubicOut);

        UpdateCode();
        StartTimer();
        StartRingAnimation();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
        _timer?.Dispose();
        _isAnimatingRing = false;
    }

    private async void StartRingAnimation()
    {
        _isAnimatingRing = true;
        while (_isAnimatingRing)
        {
            await OuterRing.ScaleToAsync(1.08, 1000, Easing.SinInOut);
            await OuterRing.ScaleToAsync(1.0, 1000, Easing.SinInOut);
        }
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

    private async void UpdateCode()
    {
        var code = TotpService.GetCurrentCode();
        var remaining = TotpService.GetRemainingSeconds();

        if (remaining == 29)
        {
            await OtpLabel.FadeToAsync(0, 150);
            OtpLabel.Text = $"{code[..3]} {code[3..]}";
            await OtpLabel.FadeToAsync(1, 300);
        }
        else
        {
            OtpLabel.Text = $"{code[..3]} {code[3..]}";
        }

        CountdownLabel.Text = remaining.ToString();
        TimerProgress.Progress = remaining / 30.0;

        if (remaining <= 5)
        {
            OtpLabel.TextColor = Color.FromArgb("#FF5252");
            TimerProgress.ProgressColor = Color.FromArgb("#FF5252");
            CountdownLabel.TextColor = Color.FromArgb("#FF5252");
            CopyBtn.BackgroundColor = Color.FromArgb("#FF5252");
            await OtpLabel.ScaleToAsync(1.05, 200);
            await OtpLabel.ScaleToAsync(1.0, 200);
        }
        else
        {
            OtpLabel.TextColor = Color.FromArgb("#4F8EF7");
            TimerProgress.ProgressColor = Color.FromArgb("#4F8EF7");
            CountdownLabel.TextColor = Color.FromArgb("#4F8EF7");
            CopyBtn.BackgroundColor = Color.FromArgb("#4F8EF7");
        }
    }

    private async void CopyBtn_Clicked(object sender, EventArgs e)
    {
        var code = TotpService.GetCurrentCode();
        await Clipboard.SetTextAsync(code);

        await CopyBtn.ScaleToAsync(0.95, 100);
        await CopyBtn.ScaleToAsync(1.0, 100);

        CopyBtn.Text = _isArabic ? "✓ تم النسخ!" : "✓ Copied!";
        CopiedLabel.IsVisible = true;

        await Task.Delay(2000);

        CopyBtn.Text = _isArabic ? "نسخ الرمز" : "Copy Code";
        CopiedLabel.IsVisible = false;
    }

    private void OnThemeToggle(object? sender, TappedEventArgs e)
    {
        _isDarkMode = !_isDarkMode;
        ApplyTheme();
    }

    private void ApplyTheme()
    {
        if (_isDarkMode)
        {
            BackgroundColor = DarkBg;
            InnerCircle.Fill = new SolidColorBrush(DarkCard);
            CardSection1.BackgroundColor = DarkCard;
            CardSection2.BackgroundColor = DarkCard;
            CardSection3.BackgroundColor = DarkCard;
            CodeBorder.BackgroundColor = DarkCodeBg;
            AppNameLabel.TextColor = Colors.White;
            ThemeBtn.Text = "🌙";
        }
        else
        {
            BackgroundColor = LightBg;
            InnerCircle.Fill = new SolidColorBrush(LightCard);
            CardSection1.BackgroundColor = LightCard;
            CardSection2.BackgroundColor = LightCard;
            CardSection3.BackgroundColor = LightCard;
            CodeBorder.BackgroundColor = LightCodeBg;
            AppNameLabel.TextColor = Color.FromArgb("#0A0E27");
            ThemeBtn.Text = "☀️";
        }
    }

    private void OnLanguageToggle(object? sender, TappedEventArgs e)
    {
        _isArabic = !_isArabic;
        ApplyLanguage();
    }

    private void ApplyLanguage()
    {
        if (_isArabic)
        {
            LangBtn.Text = "AR | EN";
            AppNameLabel.Text = "كاريير فيرست";
            AppSubtitleLabel.Text = "مولد رمز التحقق";
            SecureCodeLabel.Text = "رمز الوصول الآمن";
            RefreshesInLabel.Text = "ينتهي خلال";
            SecondsLabel.Text = "ثانية";
            CopyBtn.Text = "نسخ الرمز";
            CopiedLabel.Text = "✓ تم النسخ إلى الحافظة!";
            FooterLabel.Text = "مشفر بالكامل · لا يُشارك أبداً";
        }
        else
        {
            LangBtn.Text = "EN | AR";
            AppNameLabel.Text = "CareerFirst";
            AppSubtitleLabel.Text = "OTP GENERATOR";
            SecureCodeLabel.Text = "SECURE ACCESS CODE";
            RefreshesInLabel.Text = "Refreshes in";
            SecondsLabel.Text = "seconds";
            CopyBtn.Text = "Copy Code";
            CopiedLabel.Text = "✓ Code copied to clipboard!";
            FooterLabel.Text = "End-to-end encrypted · Never shared";
        }
    }
}
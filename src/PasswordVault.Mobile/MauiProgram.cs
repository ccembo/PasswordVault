using Microsoft.Extensions.Logging;
using PasswordVault.Mobile.ViewModel;
using Plugin.Maui.Biometric;

namespace PasswordVault.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<DetailsPage>();
            builder.Services.AddTransient<DetailViewModel>();

            builder.Services.AddSingleton<IBiometric>(
                BiometricAuthenticationService.Default);

            return builder.Build();
        }
    }
}

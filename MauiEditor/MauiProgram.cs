using MauiEditor.View;
using MauiEditor.ViewModel;
using Microsoft.Extensions.Logging;

namespace MauiEditor
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
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<InsertDataView>();
            builder.Services.AddTransient<InsertViewModel>();

            builder.Services.AddTransient<KommuneManagerView>();
            builder.Services.AddTransient<KommuneViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

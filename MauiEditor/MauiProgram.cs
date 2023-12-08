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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // Jesper
            // "creates" page when opened and "destroy" when closed
            builder.Services.AddTransient<UpdateDataView>();  
            // builder.Services.AddTransient<UpdateDataViewModel>();

            return builder.Build();
        }
    }
}

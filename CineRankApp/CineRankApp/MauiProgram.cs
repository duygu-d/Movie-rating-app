using CineRankApp.Services;
using CineRankApp.View;
using CineRankApp.ViewModel;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace CineRankApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
                    fonts.AddFont("Epilogue-Medium.ttf", "Epilogue");
                    fonts.AddFont("fontello.ttf", "Icons");
                });

            builder.Services.AddSingleton<MovieService>(); //registering the service once

            builder.Services.AddSingleton<MoviesViewModel>();
            builder.Services.AddSingleton<FavouriteMoviesViewModel>();
            builder.Services.AddTransient<MovieDetailsViewModel>(); //creating a new VM for every single CinemaMovie object

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<FavouriteMovies>();
            builder.Services.AddTransient<MovieDetailsPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
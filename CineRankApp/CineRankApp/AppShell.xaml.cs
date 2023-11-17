using CineRankApp.View;

namespace CineRankApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MovieDetailsPage), typeof(MovieDetailsPage));
            Routing.RegisterRoute(nameof(FavouriteMovies), typeof(FavouriteMovies));
        }
    }
}
using CineRankApp.ViewModel;

namespace CineRankApp.View;

public partial class FavouriteMovies : ContentPage
{
	public FavouriteMovies(FavouriteMoviesViewModel favouriteMovies)
	{
		InitializeComponent();
		BindingContext = favouriteMovies;
	}
}
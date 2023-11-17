using CineRankApp.Controls;
using CineRankApp.Model;
using CineRankApp.ViewModel;

namespace CineRankApp.View;

public partial class MovieDetailsPage : ContentPage
{
    public MovieDetailsPage(MovieDetailsViewModel movieViewModel)
	{
        InitializeComponent();
        BindingContext = movieViewModel;
    }
}
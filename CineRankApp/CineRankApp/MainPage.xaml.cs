using CineRankApp.View;
using CineRankApp.ViewModel;

namespace CineRankApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MoviesViewModel moviesViewModel)
        {
            InitializeComponent();
            BindingContext = moviesViewModel; //initializing the context for this page
        }
    }
}
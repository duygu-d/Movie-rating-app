using CommunityToolkit.Mvvm.ComponentModel;
using CineRankApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineRankApp.Services;
using CommunityToolkit.Mvvm.Input;
using CineRankApp.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CineRankApp.View;

namespace CineRankApp.ViewModel
{
    [QueryProperty("CinemaMovie", "CinemaMovie")]
    public partial class MovieDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private CinemaMovie _cinemaMovie;

        private MovieService _movieService;

        private List<CinemaMovie> _favMovies = new List<CinemaMovie>();

        public MovieDetailsViewModel(MovieService movieService)
        {
            _movieService = movieService;
        }

        [RelayCommand]
        public async Task GoToHomePage()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task GoToFavouriteMovies()
        {
            await Shell.Current.GoToAsync(nameof(FavouriteMovies));
        }

        [RelayCommand]
        public async Task OnRatingStarsTapped (int rating)
        {
            CinemaMovie.UserRating = rating;

            _favMovies = await _movieService.GetFavMovies();

            if(CinemaMovie.UserRating >= 4)
            {
                _favMovies.Add(CinemaMovie);
                await _movieService.SaveToFavMovies(_favMovies);
            }
        }

    }
}

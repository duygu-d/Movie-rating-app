using CineRankApp.Model;
using CineRankApp.Services;
using CineRankApp.View;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineRankApp.ViewModel
{
    public partial class FavouriteMoviesViewModel : BaseViewModel
    {
        public ObservableCollection<CinemaMovie> FavouriteMovies { get; } = new();
        private MovieService _movieService;

        public FavouriteMoviesViewModel(MovieService movieService)
        {
            _movieService = movieService;
            GetMoviesAsync();
        }

        public async void GetMoviesAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var movies = await _movieService.GetFavMovies();

                if (FavouriteMovies.Count != 0)
                {
                    FavouriteMovies.Clear();
                }


                foreach (var movie in movies) //observable range collection
                {
                    FavouriteMovies.Add(movie);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", "Unable to get the movies!", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task GoToMovieDetails(CinemaMovie movie)
        {
            if (movie == null)
            {
                return;
            }
            CinemaMovie cinemaMovie = await _movieService.GetMovieDetails(movie.Id);
            await Shell.Current.GoToAsync($"{nameof(MovieDetailsPage)}", true,
                new Dictionary<string, object>
                {
                    {"CinemaMovie", cinemaMovie }
                });
        }
    }
}

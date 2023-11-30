using CineRankApp.Model;
using CineRankApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using CineRankApp.View;

namespace CineRankApp.ViewModel
{
    public partial class MoviesViewModel : BaseViewModel
    {
       private MovieService _movieService;
       public ObservableCollection<CinemaMovie> Movies { get; } = new();
       public ObservableCollection<CinemaMovie> TrendingMovies { get; } = new();

        public MoviesViewModel(MovieService movieService)
        {
            Title = "CineRank";
            _movieService = movieService;
           InitializeAsync();

        }

        [RelayCommand]
        [Obsolete]
        public async Task GoToMovieDetails(CinemaMovie movie)
        {
            if(movie == null)
            {
                return;
            }
            CinemaMovie cinemaMovie = await _movieService.GetMovieDetails(movie.Id);
            cinemaMovie.FormatedGenres = movie.FormatedGenres;
            cinemaMovie.PosterPath = movie.PosterPath;
            SetUserRating(cinemaMovie);
            await Shell.Current.GoToAsync($"{nameof(MovieDetailsPage)}",true,
                new Dictionary<string, object>
                {
                    {"CinemaMovie", cinemaMovie }
                });
        }

        [Obsolete]
        private async void SetUserRating(CinemaMovie movie)
        {
            var favMovies = await _movieService.GetFavMovies();
            int rating = favMovies.Where(m => m.Title.Equals(movie.Title)).Select(m => m.UserRating).FirstOrDefault();

            if(rating > 0)
            {
                movie.UserRating = rating;
                MessagingCenter.Send<object, int>(this, "UserRatingChanged", movie.UserRating);
            }
        }

        [RelayCommand]
        public async Task GoToFavouriteMovies()
        {
            await Shell.Current.GoToAsync(nameof(FavouriteMovies));
        }
        private async void InitializeAsync()
        {
            try
            {
                await GetTrendingMoviesAsync();
                await GetMoviesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", "Unable to get the movies!", "OK");
            }
        }
        public async Task GetMoviesAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var movies = await _movieService.GetAllMovies();

                if (Movies.Count != 0)
                {
                    Movies.Clear();
                }


                foreach (var movie in movies) //observable range collection
                {
                    Movies.Add(movie);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", "Unable to get the movies!","OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetTrendingMoviesAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var trendingMovies = await _movieService.GetTrendingMovies();

                if (TrendingMovies.Count != 0)
                {
                    TrendingMovies.Clear();
                }


                foreach (var movie in trendingMovies) //observable range collection
                {
                    TrendingMovies.Add(movie);
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
    }
}

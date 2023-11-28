using CineRankApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CineRankApp.Services
{
    public class MovieService
    {
        private const string _apiKey = "eba798dfef161dcf10e59c070e3194b5";
        private static readonly Uri _basesUrl = new Uri("https://api.themoviedb.org/3/");
        private List<CinemaMovie> _movies = new List<CinemaMovie>();
        private List<CinemaMovie> _favMovies = new List<CinemaMovie>();
        private List<CinemaMovie> _trendingMovies = new List<CinemaMovie>();
        private CinemaMovie _movie = new CinemaMovie();
        private List<Genre> _allGenres = new List<Genre>();
        private List<Cast> _cast = new List<Cast>();
        private string _key = "";
        private HttpClient _httpClient;


        public MovieService()
        {
            _httpClient = new HttpClient() { BaseAddress = _basesUrl };
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

        }

        private async Task<string> GetTrailerKey(string movieId)
        {
            var trailerKeyJson = await GetInfoJSON($"movie/{movieId}/videos?api_key={_apiKey}");
            _key = JObject.Parse(trailerKeyJson)["results"]
                                .Where(x => x["name"]?.ToString().ToLower() == "official trailer")
                                .Select(x => x["key"])
                                .FirstOrDefault()?
                                .ToString();

            return _key;

        }

        private async Task<List<Cast>> GetMovieCast(string movieId)
        {
            var castJson = await GetInfoJSON($"movie/{movieId}/credits?api_key={_apiKey}");
            _cast = JObject.Parse(castJson)["cast"].ToObject<List<Cast>>();

            return _cast;
        }

        private async Task FilterActors(CinemaMovie movie)
        {
            List<Cast> cast = await GetMovieCast(movie.Id);
            movie.Actors = cast.Where(c => c.Department.ToLower() == "acting" && !(string.IsNullOrEmpty(c.ProfilePath))).Take(15).ToList();
        }

        public async Task<string> GetInfoJSON(string path)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(path))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

        }

        private async Task SetMovieGenres(List<CinemaMovie> movies)
        {
            if (_allGenres?.Count <= 0)
            {
                _allGenres = await GetAllGenres();
            }
            foreach (var movie in movies)
            {
                if (movie.Genres == null)
                {
                    movie.Genres = new List<Genre>();
                }

                foreach (var id in movie.GenreIds)
                {
                    Genre matchingGenre = _allGenres.FirstOrDefault(genre => genre.Id == id);
                    movie.Genres.Add(matchingGenre);
                }
            }
        }

        private void SetPosterPath(List<CinemaMovie> movies)
        {
            foreach (var movie in movies)
            {
                var posterKey = movie.PosterPath;
                movie.PosterPath = $"https://image.tmdb.org/t/p/original{posterKey}";
            }
        }

        public async Task<List<CinemaMovie>> GetAllMovies()
        {
            if (_movies?.Count > 0)
            {
                return _movies;
            }
            var moviesJSON = await GetInfoJSON($"discover/movie?api_key={_apiKey}");
            _movies = JObject.Parse(moviesJSON)["results"].ToObject<List<CinemaMovie>>();
            SetPosterPath(_movies);
            await SetMovieGenres(_movies);
            GenresFormatter(_movies);

            return _movies;
        }

        public async Task<List<CinemaMovie>> GetTrendingMovies()
        {
            if (_trendingMovies?.Count > 0)
            {
                return _trendingMovies;
            }
            var trendingMoviesJSON = await GetInfoJSON($"trending/movie/day?api_key={_apiKey}");
            _trendingMovies = JObject.Parse(trendingMoviesJSON)["results"].ToObject<List<CinemaMovie>>();
            SetPosterPath(_trendingMovies);
            await SetMovieGenres(_trendingMovies);
            GenresFormatter(_trendingMovies);

            return _trendingMovies;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            if (_allGenres?.Count > 0)
            {
                return _allGenres;
            }
            var allGenresKvpJSON = await GetInfoJSON($"genre/movie/list?api_key={_apiKey}");
            _allGenres = JObject.Parse(allGenresKvpJSON)["genres"].ToObject<List<Genre>>();
            return _allGenres;
        }

        public async Task<CinemaMovie> GetMovieDetails(string movieId)
        {
            var movieJson = await GetInfoJSON($"movie/{movieId}?api_key={_apiKey}");
            _movie = JsonConvert.DeserializeObject<CinemaMovie>(movieJson);

            string key = await GetTrailerKey(movieId);
            _movie.TrailerPath = $"https://www.youtube.com/embed/{key}";
            await FilterActors(_movie);

            return _movie;
        }

        public async Task SaveToFavMovies(List<CinemaMovie> favMovies)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(folderPath, "favouriteData.json");

            string jsonString = System.Text.Json.JsonSerializer.Serialize(favMovies);
            await File.WriteAllTextAsync(filePath, jsonString);
            string fileContents = File.ReadAllText(filePath);
            Console.WriteLine(fileContents);
        }

        public async Task<List<CinemaMovie>> GetFavMovies()
        {
            if (_favMovies.Count > 0)
            {
                return _favMovies;
            }
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(folderPath, "favouriteData.json");
            if (File.Exists(filePath))
            {
                string jsonFromFile = await File.ReadAllTextAsync(filePath);
                _favMovies = System.Text.Json.JsonSerializer.Deserialize<List<CinemaMovie>>(jsonFromFile);
            }
            return _favMovies;
        }

        private void GenresFormatter(List<CinemaMovie> movies)
        {
            foreach (var movie in movies)
            {
                movie.FormatedGenres = string.Join(" | ", movie.Genres);

            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineRankApp.Model
{
    public class CinemaMovie
    {
        //private const string _basePath = "https://image.tmdb.org/t/p/original";

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("genre_ids")]
        public List<int> GenreIds { get; set; }

        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }

        [JsonIgnore]
        public List<Cast> Actors { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("release_date")]
        public DateOnly ReleaseDate { get; set; }

        [JsonProperty("runtime")]
        public string Runtime { get; set; }

        
        //private string _posterPath;

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }


        [JsonIgnore]
        public int UserRating { get; set; } = 0;

        [JsonIgnore]
        public string FormatedGenres { get; set; }

        [JsonProperty("key")]
        public string TrailerPath { get; set; }

    }
}

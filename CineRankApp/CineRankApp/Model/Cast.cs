using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineRankApp.Model
{
    public class Cast
    {
        private const string _basePath = "https://image.tmdb.org/t/p/original";

        [JsonProperty("known_for_department")]
        public string Department { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("character")]
        public string CharacterName { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }
        public string Image
        {
            get => $"{_basePath}{ProfilePath}";
        }
    }
}

using System;
using System.Text.Json.Serialization;

namespace StarWarsAPI.Model
{
    public class People
    {
        public string ImgSrc { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("height")]
        public string Height { get; set; }

        [JsonPropertyName("mass")]
        public string Mass { get; set; }

        [JsonPropertyName("hair_color")]
        public string HairColor { get; set; }

        [JsonPropertyName("skin_color")]
        public string SkinColor { get; set; }

        [JsonPropertyName("eye_color")]
        public string EyeColor { get; set; }

        [JsonPropertyName("birth_year")]
        public string BirthYear { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("homeworld")]
        public string HomeworldUrl { get; set; }

        [JsonPropertyName("species")]
        public List<string> SpeciesUrls { get; set; } = new List<string>();

        public Planet Planet { get; set; }
        public List<Species> Species { get; set; } = new List<Species>();
    }

    public class PeopleWrapper
    {
        [JsonPropertyName("results")]
        public List<People> Results { get; set; }

        [JsonPropertyName("next")]
        public string NextPage { get; set; }
    }
}

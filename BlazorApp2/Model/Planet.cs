using System.Text.Json.Serialization;

namespace StarWarsAPI.Model
{
    public class Planet
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("population")]
        public string Population { get; set; }
        [JsonPropertyName("climate")]

        public string Climate { get; set; }
        [JsonPropertyName("gravity")]

        public string Gravity { get; set; }

        [JsonPropertyName("terrain")]
        public string Terrain { get; set; }

    }
}

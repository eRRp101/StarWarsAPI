using System.Text.Json.Serialization;

namespace StarWarsAPI.Model
{
    public class Species
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("classification")]
        public string Classification { get; set; }

        [JsonPropertyName("average_lifespan")]
        public string AverageLifespan { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}

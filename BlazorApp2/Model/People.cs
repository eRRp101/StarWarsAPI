using System;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace BlazorApp2.Model
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
    }

    public class PersonWrapper : IWrapper
    {
        [JsonPropertyName("results")]
        public List<People> PersonList { get; set; }
    }
}

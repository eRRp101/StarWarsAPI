using System.Text.Json.Serialization;

namespace BlazorApp2.Model
{
    public class SWAPIpage
    {
        [JsonPropertyName("next")]
        public string NextPage { get; set; }

        [JsonPropertyName("previous")]
        public string PreviousPage { get; set; }
    }
}

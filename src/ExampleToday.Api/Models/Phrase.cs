using System.Text.Json.Serialization;

namespace ExampleToday.Api.Models
{
    public class Phrase
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("autor")]
        public string Author { get; set; } = string.Empty!;

        [JsonPropertyName("frase")]
        public string Content { get; set; } = string.Empty!;
    }
}
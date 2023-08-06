namespace ExampleToday.Api.Models
{
    public class Phrase
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty!;
        public string Content { get; set; } = string.Empty!;
    }
}

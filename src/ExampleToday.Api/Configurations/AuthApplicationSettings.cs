namespace ExampleToday.Api.Configurations
{
    public class AuthApplicationSettings
    {
        public string Key { get; set; } = string.Empty!;
        public string Issuer { get; set; } = string.Empty!;
        public string Audience { get; set; } = string.Empty!;
    }
}
namespace ExampleToday.Api.ViewObjects
{
    public class FailResponse
    {
        public FailResponse()
        {
            Messages = new List<string>();
        }

        public FailResponse(IEnumerable<string> messages)
        {
            Messages = messages;
        }

        public FailResponse(string message)
        {
            Messages = new string[] { message };
        }

        public IEnumerable<string> Messages { get; }
    }
}
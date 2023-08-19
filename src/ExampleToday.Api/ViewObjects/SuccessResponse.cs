namespace ExampleToday.Api.ViewObjects
{
    public class SuccessResponse<T>
    {
        public SuccessResponse(T results)
        {
            Results = results;
        }

        public T Results { get; }

    }
}
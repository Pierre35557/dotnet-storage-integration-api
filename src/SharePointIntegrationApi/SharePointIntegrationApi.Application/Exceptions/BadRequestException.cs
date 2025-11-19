namespace SharePointIntegrationApi.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs when one or more validation rules fail.
    /// </summary>
    public class BadRequestException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public BadRequestException(IEnumerable<string> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}

namespace StorageIntegrationApi.Application.Exceptions
{
    /// <summary>
    /// Represents an error that occurs when one or more required request headers are missing.
    /// </summary>
    public class MissingHeadersException : Exception
    {
        public IReadOnlyList<string> MissingHeaders { get; }

        public MissingHeadersException()
            : base("One or more required headers are missing.")
        {
            MissingHeaders = Array.Empty<string>();
        }

        public MissingHeadersException(IEnumerable<string> missingHeaders)
            : base($"Missing required header(s): {string.Join(", ", missingHeaders)}")
        {
            MissingHeaders = missingHeaders.ToList();
        }

        public MissingHeadersException(string message, Exception? innerException)
            : base(message, innerException)
        {
            MissingHeaders = Array.Empty<string>();
        }
    }
}

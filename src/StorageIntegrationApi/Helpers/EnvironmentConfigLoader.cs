namespace StorageIntegrationApi.Api.Helpers
{
    /// <summary>
    /// Utility class for loading environment variables from a file.
    /// </summary>
    public class EnvironmentConfigLoader
    {
        /// <summary>
        /// Loads environment variables from a file and sets them in the current process.
        /// </summary>
        /// <param name="filePath">The path to the environment variable file.</param>
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var rawLine in File.ReadAllLines(filePath))
            {
                var line = rawLine.Trim();

                // Skip empty lines and comments
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var separatorIndex = line.IndexOf('=');
                if (separatorIndex == -1)
                    continue;

                var key = line.Substring(0, separatorIndex).Trim();
                var value = line.Substring(separatorIndex + 1).Trim();

                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}

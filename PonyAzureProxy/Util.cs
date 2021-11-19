namespace PonyAzureProxy
{
    public class Util
    {

        public static string GetEnv(string name, string defaultVal = null)
        {
            string? YARPFORWARD_PREFIX = Environment.GetEnvironmentVariable(name);
            if (!string.IsNullOrEmpty(YARPFORWARD_PREFIX) && YARPFORWARD_PREFIX.Trim().Length > 0)
            {
                return YARPFORWARD_PREFIX.Trim();

            }

            return defaultVal;
        }

    }
}

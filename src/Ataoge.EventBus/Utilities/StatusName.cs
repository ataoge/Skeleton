namespace Ataoge.EventBus.Utilities
{
    /// <summary>
    /// The message status name.
    /// </summary>
    public struct StatusName
    {
        public const string Scheduled = nameof(Scheduled);
        public const string Succeeded = nameof(Succeeded);
        public const string Failed = nameof(Failed);

        public static string Standardized(string input)
        {
            foreach (var item in typeof(StatusName).GetFields())
            {
                if (item.Name.ToLower() == input.ToLower())
                {
                    return item.Name;
                }
            }
            return string.Empty;
        }
    }
}
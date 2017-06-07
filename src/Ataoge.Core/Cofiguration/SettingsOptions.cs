namespace Ataoge.Configuration
{
    public class SettingsOptions
    {
        public string UploadRootPath {get; set;}

        public bool UseAbsoluteUrl {get; set;}

        public string AndroidBaseScheme {get; set;} = "dciWeb://";

        public string IosBaseScheme {get; set;} = "dciWeb://";

        public string ApiBasePath {get; set;}

        public string AdminBasePath {get; set;}

        public string WebBasePath {get; set;}
    }
}
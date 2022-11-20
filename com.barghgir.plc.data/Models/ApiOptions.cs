namespace com.barghgir.plc.api.Data
{
    public sealed class ApiOptions
    {
        public LoggingOptions Logging { get; set; }
        public string AllowedHosts { get; set; }
        public ImagesOptions Images { get; set; }
    }
}
public class ImagesOptions
{
    public ImageSize DetailBackgroundSize { get; set; }
    public ImageSize ListBackgroundSize { get; set; }
    public string SourceUrl { get; set; }
}
public class ImageSize
{
    public int? HeightPx { get; set; }
    public int? WidthPx { get; set; }
}
public class LoggingOptions
{
    public LogLevelOptions LogLevel { get; set; }
}
public class LogLevelOptions
{
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
}

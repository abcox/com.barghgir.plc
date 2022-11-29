namespace com.barghgir.plc.data.Models;

public class Configuration
{
    public string? SelectedEnvironmentName { get; set; }
    public Environment[]? Environments { get; set; }

    public class Environment
    {
        public string? Name { get; set; }
        public Options? Options { get; set; }
    }

    public class Options
    {
        public string? BaseServiceEndpoint { get; set; }
        public string? ImageUrl { get; set; }
    }
}
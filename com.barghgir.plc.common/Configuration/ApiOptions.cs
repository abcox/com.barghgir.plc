namespace com.barghgir.plc.common.Configuration;

public sealed class ApiOptions
{
    public ConnectionStrings? ConnectionStrings { get; set; }
    public LoggingOptions? Logging { get; set; }
    public string? AllowedHosts { get; set; }
    public ImagesOptions? Images { get; set; }
    public SecurityOptions? Security { get; set; }
    public AppInfo? AppInfo { get; set; }
    public AzureOptions? Azure { get; set; }
}

public class ConnectionStrings
{
    public string? AppDbContext { get; set; }
}

public class AzureOptions
{
    public AzureEnvironmentOptions? Environment { get; set; }
    public StorageOptions? Storage { get; set; }
    public KeyVaultOptions? KeyVault { get; set; }
}

public class AzureEnvironmentOptions
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? SubscriptionName { get; set; }
    public string? TenantId { get; set; }
}

public class KeyVaultOptions
{
    public string? Name { get; set; }
    public string? Prefix { get; set; }
    public string? Url { get; set; }
}

public class StorageOptions
{
    public string? Url { get; set; }
}

public class AppInfo
{
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public string? Environment { get; set; }
}

public class SecurityOptions
{
    public string? AesEncryptionKey { get; set; }
    public string? AesEncryptionIVector { get; set; } // Initialization Vector
    public int FailedSignInCountMaxLimit { get; set; }
}

public class ImagesOptions
{
    public ImageSize? DetailBackgroundSize { get; set; }
    public ImageSize? ListBackgroundSize { get; set; }
    public string SourceUrl { get; set; }
}

public class ImageSize
{
    public int? HeightPx { get; set; }
    public int? WidthPx { get; set; }
}

public class LoggingOptions
{
    public LogLevelOptions? LogLevel { get; set; }
}

public class LogLevelOptions
{
    public string? Default { get; set; }
    public string? MicrosoftAspNetCore { get; set; }
}
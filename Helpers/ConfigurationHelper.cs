using Microsoft.Extensions.Configuration;

namespace Helpers;

public class ConfigurationHelper
{
    private IConfiguration Configuration { get; }

    public ConfigurationHelper(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public string[] GetStringArray(string key)
    {
        string? value = Configuration[key];

        if (value == null)
        {
            throw new ArgumentException($"key: {key} not found in configuration.", nameof(key));
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        return value.Split(',');
    }
}
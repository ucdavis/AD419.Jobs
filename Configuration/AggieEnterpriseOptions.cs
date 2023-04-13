namespace AD419Functions.Configuration;

public class AggieEnterpriseOptions
{
    public string ApiUrl { get; set; } = "";
    public string ConsumerKey { get; set; } = "";
    public string ConsumerSecret { get; set; } = "";
    public string TokenEndpoint { get; set; } = "";
    public string ScopeApp { get; set; } = "";
    public string ScopeEnv { get; set; } = "";
    public int BatchSize { get; set; } = 1000;
}

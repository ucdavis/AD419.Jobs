using System;
using Serilog;

using AggieEnterpriseApi;
using AggieEnterpriseApi.Extensions;
using Microsoft.Extensions.Options;
using AD419Functions.Configuration;

namespace AD419Functions.Services;

public class AggieEnterpriseService
{
    private readonly AggieEnterpriseOptions _options;
    private readonly IAggieEnterpriseClient _aggieClient;

    public AggieEnterpriseService(IOptions<AggieEnterpriseOptions> options)
    {
        _options = options.Value;
        _aggieClient = GraphQlClient.Get(
            _options.ApiUrl,
            _options.TokenEndpoint,
            _options.ConsumerKey,
            _options.ConsumerSecret,
            $"{_options.ScopeApp}-{_options.ScopeEnv}");
    }

    public async Task Test()
    {
        var result = await _aggieClient.GlValidateChartstring.ExecuteAsync("3110-13U20-ADNO003-238533-00-000-0000000000-000000-0000-000000-000000", true);

        var data = result.ReadData();

        Log.Information("CCOA string is valid: {IsValid}", data.GlValidateChartstring.ValidationResponse.Valid);
    }
}
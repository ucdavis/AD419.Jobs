using System;
using Serilog;

using AggieEnterpriseApi;
using AggieEnterpriseApi.Extensions;
using Microsoft.Extensions.Options;
using AD419Functions.Configuration;
using System.Collections.ObjectModel;

namespace AD419Functions.Services;

public class AggieEnterpriseService
{
    private readonly AggieEnterpriseOptions _options;
    private readonly IAggieEnterpriseClient _apiClient;

    public AggieEnterpriseService(IOptions<AggieEnterpriseOptions> options)
    {
        _options = options.Value;
        _apiClient = GraphQlClient.Get(
            _options.ApiUrl,
            _options.TokenEndpoint,
            _options.ConsumerKey,
            _options.ConsumerSecret,
            $"{_options.ScopeApp}-{_options.ScopeEnv}");
    }

    public async IAsyncEnumerable<IErpDepartmentAllPaged_ErpFinancialDepartmentSearch_Data> GetFinancialDepartmentValues()
    {
        var startIndex = 0;

        while (startIndex > -1)
        {
            var result = await _apiClient.ErpDepartmentAllPaged.ExecuteAsync(new ErpFinancialDepartmentFilterInput
            {
                SearchCommon = new SearchCommonInputs
                {
                    Limit = _options.BatchSize,
                    StartIndex = startIndex,
                    IncludeTotalResultCount = true,
                },
                Enabled = new BooleanFilterInput
                {
                    Eq = true
                },
            });

            var data = result.ReadData();
            startIndex = data.ErpFinancialDepartmentSearch.Data.Count > 0 
                ? startIndex + data.ErpFinancialDepartmentSearch.Data.Count
                : -1;

            foreach (var deptData in data.ErpFinancialDepartmentSearch.Data)
            {
                yield return deptData;
            }
        }
    }

    public async Task Test()
    {
        var result = await _apiClient.GlValidateChartstring.ExecuteAsync("3110-13U20-ADNO003-238533-00-000-0000000000-000000-0000-000000-000000", true);

        var data = result.ReadData();

        Log.Information("CCOA string is valid: {IsValid}", data.GlValidateChartstring.ValidationResponse.Valid);
    }
}
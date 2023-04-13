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

    public async IAsyncEnumerable<IErpDepartmentSearch2_ErpFinancialDepartmentSearch_Data> GetFinancialDepartmentValues(int batchSize = 100)
    {
        var startIndex = 0;

        while (startIndex > -1)
        {
            var result = await _apiClient.ErpDepartmentSearch2.ExecuteAsync(new ErpFinancialDepartmentFilterInput
            {
                SearchCommon = new SearchCommonInputs
                {
                    Limit = batchSize,
                    StartIndex = startIndex,
                    IncludeTotalResultCount = true,
                    Sort = new string[] { "parentCode", "code" },
                },
                Enabled = new BooleanFilterInput
                {
                    Eq = true
                },
            });

            var data = result.ReadData();

            foreach (var deptData in data.ErpFinancialDepartmentSearch.Data)
            {
                //Log.Information("Parent: {ParentCode} Dept: {DeptCode} - {DeptName}", deptData.ParentCode, deptData.Code, deptData.Name);
                yield return deptData;
            }
            startIndex = data.ErpFinancialDepartmentSearch.Metadata.NextStartIndex ?? -1;
        }
    }

    public async Task Test()
    {
        var result = await _apiClient.GlValidateChartstring.ExecuteAsync("3110-13U20-ADNO003-238533-00-000-0000000000-000000-0000-000000-000000", true);

        var data = result.ReadData();

        Log.Information("CCOA string is valid: {IsValid}", data.GlValidateChartstring.ValidationResponse.Valid);
    }
}
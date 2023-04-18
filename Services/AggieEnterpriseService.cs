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

            foreach (var item in data.ErpFinancialDepartmentSearch.Data)
            {
                yield return item;
            }
        }
    }

    public async IAsyncEnumerable<IErpFundAllPaged_ErpFundSearch_Data> GetFundValues()
    {
        var startIndex = 0;

        while (startIndex > -1)
        {
            var result = await _apiClient.ErpFundAllPaged.ExecuteAsync(new ErpFundFilterInput
            {
                SearchCommon = new SearchCommonInputs
                {
                    Limit = _options.BatchSize,
                    StartIndex = startIndex,
                },
                Enabled = new BooleanFilterInput
                {
                    Eq = true
                },
                // TODO: this is a workaround for skipping record with invalid name. remove this when data gets corrected
                Code = new StringFilterInput
                {
                    Ne = "37542"
                },
            });

            var data = result.ReadData();
            startIndex = data.ErpFundSearch.Data.Count > 0 
                ? startIndex + data.ErpFundSearch.Data.Count
                : -1;

            foreach (var item in data.ErpFundSearch.Data)
            {
                yield return item;
            }
        }
    }

    public async IAsyncEnumerable<IErpAccountAllPaged_ErpAccountSearch_Data> GetAccountValues()
    {
        var startIndex = 0;

        while (startIndex > -1)
        {
            var result = await _apiClient.ErpAccountAllPaged.ExecuteAsync(new ErpAccountFilterInput
            {
                SearchCommon = new SearchCommonInputs
                {
                    Limit = _options.BatchSize,
                    StartIndex = startIndex,
                },
                Enabled = new BooleanFilterInput
                {
                    Eq = true
                }
            });

            var data = result.ReadData();
            startIndex = data.ErpAccountSearch.Data.Count > 0 
                ? startIndex + data.ErpAccountSearch.Data.Count
                : -1;

            foreach (var item in data.ErpAccountSearch.Data)
            {
                yield return item;
            }
        }
    }

    public async IAsyncEnumerable<IErpProjectAllPaged_ErpProjectSearch_Data> GetProjectValues()
    {
        var startIndex = 0;

        while (startIndex > -1)
        {
            var result = await _apiClient.ErpProjectAllPaged.ExecuteAsync(new ErpProjectFilterInput
            {
                SearchCommon = new SearchCommonInputs
                {
                    Limit = _options.BatchSize,
                    StartIndex = startIndex,
                },
                Enabled = new BooleanFilterInput
                {
                    Eq = true
                },
            });

            var data = result.ReadData();
            startIndex = data.ErpProjectSearch.Data.Count > 0 
                ? startIndex + data.ErpProjectSearch.Data.Count
                : -1;

            foreach (var item in data.ErpProjectSearch.Data)
            {
                yield return item;
            }
        }
    }

}
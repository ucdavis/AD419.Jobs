using System;
using Serilog;

using AggieEnterpriseApi;
using AggieEnterpriseApi.Extensions;
using Microsoft.Extensions.Options;
using AD419.Jobs.Configuration;

namespace AD419.Jobs.Services;

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

    public async IAsyncEnumerable<IErpDepartmentSearch_ErpFinancialDepartmentSearch_Data> GetFinancialDepartmentValues()
    {
        var startIndex = 0;
        IErpDepartmentSearchResult? data = null;

        while (startIndex > -1)
        {
            try
            {
                var result = await _apiClient.ErpDepartmentSearch.ExecuteAsync(new ErpFinancialDepartmentFilterInput
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
                },
                "A"); // Just a placeholder. We're not interested in this portion of the response.

                data = result.ReadData();
                startIndex = data.ErpFinancialDepartmentSearch.Data.Count > 0
                    ? startIndex + data.ErpFinancialDepartmentSearch.Data.Count
                    : -1;
            }
            catch (Exception ex)
            {
#if DEBUG
                Log.Error(ex, "Error getting department values, skipping batch");
                startIndex += _options.BatchSize;
                continue;
#else
                throw;
#endif
            }

            foreach (var item in data.ErpFinancialDepartmentSearch.Data)
            {
                yield return item;
            }
        }
    }

    public async IAsyncEnumerable<IErpFundSearch_ErpFundSearch_Data> GetFundValues()
    {
        var startIndex = 0;
        IErpFundSearchResult? data = null;

        while (startIndex > -1)
        {
            try
            {
                var result = await _apiClient.ErpFundSearch.ExecuteAsync(new ErpFundFilterInput
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
                },
                "A"); // Just a placeholder. We're not interested in this portion of the response.

                data = result.ReadData();
                startIndex = data.ErpFundSearch.Data.Count > 0
                    ? startIndex + data.ErpFundSearch.Data.Count
                    : -1;
            }
            catch (Exception ex)
            {
#if DEBUG
                Log.Error(ex, "Error getting fund values, skipping batch");
                startIndex += _options.BatchSize;
                continue;
#else
                throw;
#endif
            }

            foreach (var item in data.ErpFundSearch.Data)
            {
                yield return item;
            }
        }
    }

    public async IAsyncEnumerable<IErpAccountSearch_ErpAccountSearch_Data> GetAccountValues()
    {
        var startIndex = 0;
        IErpAccountSearchResult? data = null;

        while (startIndex > -1)
        {
            try
            {
                var result = await _apiClient.ErpAccountSearch.ExecuteAsync(new ErpAccountFilterInput
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
                },
                "A"); // Just a placeholder. We're not interested in this portion of the response.

                data = result.ReadData();
                startIndex = data.ErpAccountSearch.Data.Count > 0
                    ? startIndex + data.ErpAccountSearch.Data.Count
                    : -1;
            }
            catch (Exception ex)
            {
#if DEBUG
                Log.Error(ex, "Error getting account values, skipping batch");
                startIndex += _options.BatchSize;
                continue;
#else
                throw;
#endif
            }

            foreach (var item in data.ErpAccountSearch.Data)
            {
                yield return item;
            }

        }
    }

    public async IAsyncEnumerable<IErpProjectSearch_ErpProjectSearch_Data> GetProjectValues()
    {
        var startIndex = 0;
        IErpProjectSearchResult? data = null;

        while (startIndex > -1)
        {
            try
            {
                var result = await _apiClient.ErpProjectSearch.ExecuteAsync(new ErpProjectFilterInput
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
                },
                "A"); // Just a placeholder. We're not interested in this portion of the response.

                data = result.ReadData();
                startIndex = data.ErpProjectSearch.Data.Count > 0
                    ? startIndex + data.ErpProjectSearch.Data.Count
                    : -1;
            }
            catch (Exception ex)
            {
#if DEBUG
                Log.Error(ex, "Error getting project values, skipping batch");
                startIndex += _options.BatchSize;
                continue;
#else
                throw;
#endif
            }

            foreach (var item in data.ErpProjectSearch.Data)
            {
                yield return item;
            }
        }
    }

}
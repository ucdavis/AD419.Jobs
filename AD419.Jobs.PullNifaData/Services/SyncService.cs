using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using AD419.Jobs.Configuration;
using Serilog;
using AD419.Jobs.Core.Utilities;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Text.RegularExpressions;
using AD419.Jobs.PullNifaData.Models;
using AD419.Jobs.PullNifaData.Extensions;
using AD419.Jobs.PullNifaData.Utilities;
using Razor.Templating.Core;
using AD419.Jobs.Core.Configuration;
using AD419.Jobs.Core.Services;
using AD419.Jobs.Core.Extensions;

namespace AD419.Jobs.PullNifaData.Services;

public class SyncService
{
    ISqlDataContext _sqlDataContext;
    private readonly SyncOptions _syncOptions;
    private readonly ISshService _sshService;

    public SyncService(ISqlDataContext sqlDataContext, IOptions<SyncOptions> syncOptions, ISshService sshService)
    {
        _sqlDataContext = sqlDataContext;
        _syncOptions = syncOptions.Value;
        _sshService = sshService;
    }

    public async Task Run()
    {
        Log.Information("Starting sync");

        var filePaths = _sshService.ListFiles("/")
            .Where(f => Regex.IsMatch(Path.GetFileName(f), $@"NIFA_(GL|PGM_(AWARD|EMPLOYEE|EXPENDITURE|PROJECT))_Incremental_[0-9]{{8}}_[0-9]{{6}}\.csv"))
            .OrderBy(x => x);

        foreach (var filePath in filePaths)
        {
            Log.Information("Processing file {FileName}", filePath);
            await _sqlDataContext.BeginTransaction();
            try
            {
                var stream = _sshService.DownloadFile(filePath);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = "|",
                });

                switch (Path.GetFileName(filePath))
                {
                    case string s when Regex.IsMatch(s, @"NIFA_GL_.*?\.csv"):
                        await SyncData<NifaGlModel>(csv);
                        break;
                    case string s when Regex.IsMatch(s, @"NIFA_PGM_AWARD_.*?\.csv"):
                        await SyncData<NifaPgmAwardModel>(csv);
                        break;
                    case string s when Regex.IsMatch(s, @"NIFA_PGM_EMPLOYEE_.*?\.csv"):
                        await SyncData<NifaPgmEmployeeModel>(csv);
                        break;
                    case string s when Regex.IsMatch(s, @"NIFA_PGM_EXPENDITURE_.*?\.csv"):
                        await SyncData<NifaPgmExpenditureModel>(csv);
                        break;
                    case string s when Regex.IsMatch(s, @"NIFA_PGM_PROJECT_.*?\.csv"):
                        await SyncData<NifaPgmProjectModel>(csv);
                        break;
                    default:
                        throw new Exception($"Unknown file type {Path.GetFileName(filePath)}");
                }

                _sshService.MoveFile(filePath, $"{_syncOptions.ProcessedFileLocation}/{Path.GetFileName(filePath)}");

                await _sqlDataContext.CommitTransaction();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error syncing");
                try
                {
                    await _sqlDataContext.RollbackTransaction();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error rolling back transaction");
                }
                throw;
            }
        }
    }

    private async Task SyncData<T>(CsvReader csv)
        where T : class
    {
        var dataTable = TableHelper.CreateDataTable<T>();
        var tableModel = TableHelper.CreateTableModel<T>();
        var initTempTableScript = await RazorTemplateEngine.RenderAsync("/Views/Scripts/InitTempTable.sql.cshtml", tableModel);
        var mergeTempDataScript = await RazorTemplateEngine.RenderAsync("/Views/Scripts/MergeTempData.sql.cshtml", tableModel);
        Log.Information("Reading {TableName} csv data", tableModel.Name);
        var records = csv.GetRecords<T>();
        foreach (var record in records)
        {
            dataTable.AddModelData(record);
        }
        Log.Information("Creating temp table {TableName}", tableModel.Name);
        await _sqlDataContext.ExecuteScriptFromString(initTempTableScript);
        Log.Information("Writing {TableName} data to temp table", tableModel.Name);
        await _sqlDataContext.BulkCopy(dataTable, $"#{tableModel.Name}", _syncOptions.BulkCopyBatchSize);
        Log.Information("Merging {TableName} data", tableModel.Name);
        await _sqlDataContext.ExecuteScriptFromString(mergeTempDataScript);
    }





}


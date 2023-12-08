using System.Data;
using AggieEnterpriseApi;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using AD419.Jobs.Configuration;
using Serilog;
using AD419.Jobs.Core.Utilities;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Text.RegularExpressions;
using AD419.Jobs.Models;
using AD419.Jobs.Core.Extensions;

namespace AD419.Jobs.PullNifaData.Services;

public class SyncService
{
    private readonly ConnectionStrings _connectionStrings;
    private readonly SyncOptions _syncOptions;
    private readonly ISshService _sshService;
    private readonly DataTable _nifaGlDataTable;

    public SyncService(IOptions<ConnectionStrings> connectionStrings, IOptions<SyncOptions> syncOptions, ISshService sshService)
    {
        _connectionStrings = connectionStrings.Value;
        _syncOptions = syncOptions.Value;
        _sshService = sshService;
        _nifaGlDataTable = CreateNifaGlDataTable();
    }

    public async Task Run()
    {
        Log.Information("Starting sync");
        using var connection = new SqlConnection(_connectionStrings.DefaultConnection);
        await connection.OpenAsync();
        Log.Information("Creating temp tables");
        await SqlHelper.ExecuteScript("Scripts/Init_Temp_Tables.sql", connection);

        var filePaths = _sshService.ListFiles("/")
            .Where(f => Path.GetFileName(f).StartsWith("NIFA_GL"))
            .OrderBy(x => x);

        foreach (var filePath in filePaths)
        {
            Log.Information("Processing file {FileName}", filePath);
            using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
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
                        await SyncNifaGlData(connection, transaction, csv);
                        break;
                    default:
                        throw new Exception($"Unknown file type {Path.GetFileName(filePath)}");
                }

                _sshService.MoveFile(filePath, $"{_syncOptions.ProcessedFileLocation}/{Path.GetFileName(filePath)}");

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error syncing");
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error rolling back transaction");
                }
                throw;
            }
        }
    }

    private async Task SyncNifaGlData(SqlConnection connection, SqlTransaction transaction, CsvReader csv)
    {
        Log.Information("Reading NIFA_GL data");
        var records = csv.GetRecords<NifaGlModel>();
        foreach (var record in records)
        {
            _nifaGlDataTable.AddNifaGlModel(record);
        }
        Log.Information("Writing NIFA_GL data to temp table");
        await SyncData(connection, transaction, _nifaGlDataTable, "#UCD_NIFA_GL");
        Log.Information("Merging NIFA_GL data");
        await SqlHelper.ExecuteScript("Scripts/NIFA_GL_Merge_Temp_Data.sql", connection, transaction);
    }

    private async Task SyncData(SqlConnection connection, SqlTransaction transaction, DataTable dataTable, string tableName)
    {
        var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction)
        {
            DestinationTableName = tableName,
            BatchSize = _syncOptions.BulkCopyBatchSize
        };
        await bulkCopy.WriteToServerAsync(dataTable);
    }

    private static DataTable CreateNifaGlDataTable()
    {
        DataTable dataTable = new DataTable();

        dataTable.Columns.Add("AccountingYear", typeof(int));
        dataTable.Columns.Add("PostedYear", typeof(int));
        dataTable.Columns.Add("Period", typeof(string));
        dataTable.Columns.Add("AccountingDate", typeof(DateTime));
        dataTable.Columns.Add("PostedDate", typeof(DateTime));
        dataTable.Columns.Add("JournalCategory", typeof(string));
        dataTable.Columns.Add("JournalSource", typeof(string));
        dataTable.Columns.Add("AccountCombination", typeof(string));
        dataTable.Columns.Add("Entity", typeof(string));
        dataTable.Columns.Add("EntityDesctiption", typeof(string));
        dataTable.Columns.Add("Fund", typeof(string));
        dataTable.Columns.Add("FundDescription", typeof(string));
        dataTable.Columns.Add("ParentFund", typeof(string));
        dataTable.Columns.Add("ParentFundDescription", typeof(string));
        dataTable.Columns.Add("FinancialDepartmentParentC", typeof(string));
        dataTable.Columns.Add("FinancialDepartmentParentC_Description", typeof(string));
        dataTable.Columns.Add("FinancialDepartment", typeof(string));
        dataTable.Columns.Add("FinancialDepartmentDescription", typeof(string));
        dataTable.Columns.Add("NaturalAccount", typeof(string));
        dataTable.Columns.Add("NaturalAccountDescription", typeof(string));
        dataTable.Columns.Add("NatruralAccountType", typeof(string));
        dataTable.Columns.Add("Purpose", typeof(string));
        dataTable.Columns.Add("PurposeDescription", typeof(string));
        dataTable.Columns.Add("Program", typeof(string));
        dataTable.Columns.Add("ProgramDescription", typeof(string));
        dataTable.Columns.Add("Project", typeof(string));
        dataTable.Columns.Add("ProjectDescription", typeof(string));
        dataTable.Columns.Add("Activity", typeof(string));
        dataTable.Columns.Add("ActivityDescription", typeof(string));
        dataTable.Columns.Add("InterEntity", typeof(string));
        dataTable.Columns.Add("GL_Future1", typeof(string));
        dataTable.Columns.Add("GL_Future2", typeof(string));
        dataTable.Columns.Add("DebitAmount", typeof(decimal));
        dataTable.Columns.Add("CreditAmount", typeof(decimal));

        return dataTable;
    }
}




public static class DataTableExtensions
{
    public static void AddNifaGlModel(this DataTable dataTable, NifaGlModel nifaGlModel)
    {
        var row = dataTable.NewRow();
        row["AccountingYear"] = nifaGlModel.AccountingYear;
        row["PostedYear"] = nifaGlModel.PostedYear as object ?? DBNull.Value;
        row["Period"] = nifaGlModel.Period;
        row["AccountingDate"] = nifaGlModel.AccountingDate as object ?? DBNull.Value;
        row["PostedDate"] = nifaGlModel.PostedDate as object ?? DBNull.Value;
        row["JournalCategory"] = nifaGlModel.JournalCategory;
        row["JournalSource"] = nifaGlModel.JournalSource;
        row["AccountCombination"] = nifaGlModel.AccountCombination;
        row["Entity"] = nifaGlModel.Entity;
        row["EntityDesctiption"] = nifaGlModel.EntityDesctiption;
        row["Fund"] = nifaGlModel.Fund;
        row["FundDescription"] = nifaGlModel.FundDescription;
        row["ParentFund"] = nifaGlModel.ParentFund;
        row["ParentFundDescription"] = nifaGlModel.ParentFundDescription;
        row["FinancialDepartmentParentC"] = nifaGlModel.FinancialDepartmentParentC;
        row["FinancialDepartmentParentC_Description"] = nifaGlModel.FinancialDepartmentParentC_Description;
        row["FinancialDepartment"] = nifaGlModel.FinancialDepartment;
        row["FinancialDepartmentDescription"] = nifaGlModel.FinancialDepartmentDescription;
        row["NaturalAccount"] = nifaGlModel.NaturalAccount;
        row["NaturalAccountDescription"] = nifaGlModel.NaturalAccountDescription;
        row["NatruralAccountType"] = nifaGlModel.NatruralAccountType;
        row["Purpose"] = nifaGlModel.Purpose;
        row["PurposeDescription"] = nifaGlModel.PurposeDescription;
        row["Program"] = nifaGlModel.Program;
        row["ProgramDescription"] = nifaGlModel.ProgramDescription;
        row["Project"] = nifaGlModel.Project;
        row["ProjectDescription"] = nifaGlModel.ProjectDescription;
        row["Activity"] = nifaGlModel.Activity;
        row["ActivityDescription"] = nifaGlModel.ActivityDescription;
        row["InterEntity"] = nifaGlModel.InterEntity;
        row["GL_Future1"] = nifaGlModel.GL_Future1;
        row["GL_Future2"] = nifaGlModel.GL_Future2;
        row["DebitAmount"] = nifaGlModel.DebitAmount as object ?? DBNull.Value;
        row["CreditAmount"] = nifaGlModel.CreditAmount as object ?? DBNull.Value;
        dataTable.Rows.Add(row);
    }
}
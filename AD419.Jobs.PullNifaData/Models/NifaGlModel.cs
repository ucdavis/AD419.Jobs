using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.PullNifaData.Models;

[Table("UCD_NIFA_GL")]
public class NifaGlModel
{
    [Name("ACCOUNTING YEAR")]
    [ColumnOrder]
    [Required]
    public int AccountingYear { get; set; }
    [Name("POSTED YEAR")]
    [ColumnOrder]
    public int? PostedYear { get; set; }
    [Name("PERIOD")]
    [ColumnOrder]
    [Required]
    public string Period { get; set; } = "";
    [Name("ACCOUNTING DATE")]
    [ColumnOrder]
    [KeyOrder(0)]
    public DateTime? AccountingDate { get; set; }
    [Name("POSTED DATE")]
    [ColumnOrder]
    [KeyOrder(1)]
    public DateTime? PostedDate { get; set; }
    [Name("JOURNAL CATEGORY")]
    [ColumnOrder]
    [KeyOrder(3)]
    public string JournalCategory { get; set; } = "";
    [Name("JOURNAL SOURCE")]
    [ColumnOrder]
    [KeyOrder(2)]
    public string JournalSource { get; set; } = "";
    [Name("ACCOUNT COMBINATION")]
    [ColumnOrder]
    [KeyOrder(4)]
    [Required]
    public string AccountCombination { get; set; } = "";
    [Name("ENTITY")]
    [ColumnOrder]
    public string Entity { get; set; } = "";
    [Name("ENTITY DESCRIPTION")]
    [ColumnOrder]
    public string EntityDesctiption { get; set; } = "";
    [Name("FUND")]
    [ColumnOrder]
    public string Fund { get; set; } = "";
    [Name("FUND DESCRIPTION")]
    [ColumnOrder]
    public string FundDescription { get; set; } = "";
    [Name("PARENT FUND")]
    [ColumnOrder]
    public string ParentFund { get; set; } = "";
    [Name("PARENT FUND DESCRIPTION")]
    [ColumnOrder]
    public string ParentFundDescription { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT PARENT C")]
    [ColumnOrder]
    public string FinancialDepartmentParentC { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT PARENT C DESCRIPTION")]
    [ColumnOrder]
    public string FinancialDepartmentParentC_Description { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT")]
    [ColumnOrder]
    public string FinancialDepartment { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT DESCRIPTION")]
    [ColumnOrder]
    public string FinancialDepartmentDescription { get; set; } = "";
    [Name("NATURAL ACCOUNT")]
    [ColumnOrder]
    public string NaturalAccount { get; set; } = "";
    [Name("NATURAL ACCOUNT DESCRIPTION")]
    [ColumnOrder]
    public string NaturalAccountDescription { get; set; } = "";
    [Name("NATURAL ACCOUNT TYPE")]
    [ColumnOrder]
    public string NatruralAccountType { get; set; } = "";
    [Name("PURPOSE")]
    [ColumnOrder]
    public string Purpose { get; set; } = "";
    [Name("PURPOSE DESCRIPTION")]
    [ColumnOrder]
    public string PurposeDescription { get; set; } = "";
    [Name("PROGRAM")]
    [ColumnOrder]
    public string Program { get; set; } = "";
    [Name("PROGRAM DESCRIPTION")]
    [ColumnOrder]
    public string ProgramDescription { get; set; } = "";
    [Name("PROJECT")]
    [ColumnOrder]
    public string Project { get; set; } = "";
    [Name("PROJECT DESCRIPTION")]
    [ColumnOrder]
    public string ProjectDescription { get; set; } = "";
    [Name("ACTIVITY")]
    [ColumnOrder]
    public string Activity { get; set; } = "";
    [Name("ACTIVITY DESCRIPTION")]
    [ColumnOrder]
    public string ActivityDescription { get; set; } = "";
    [Name("INTER ENTITY")]
    [ColumnOrder]
    public string InterEntity { get; set; } = "";
    [Name("GL FUTURE 1")]
    [ColumnOrder]
    public string GL_Future1 { get; set; } = "";
    [Name("GL FUTURE 2")]
    [ColumnOrder]
    public string GL_Future2 { get; set; } = "";
    [Name("DEBIT AMOUNT")]
    [ColumnOrder]
    [KeyOrder(5)]
    public decimal? DebitAmount { get; set; }
    [Name("CREDIT AMOUNT")]
    [ColumnOrder]
    [KeyOrder(6)]
    public decimal? CreditAmount { get; set; }
}

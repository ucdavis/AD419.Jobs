using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.Models;

public class NifaGlModel
{
    [Name("ACCOUNTING YEAR")]
    [Order]
    public int AccountingYear { get; set; }
    [Name("POSTED YEAR")]
    [Order]
    public int? PostedYear { get; set; }
    [Name("PERIOD")]
    [Order]
    public string Period { get; set; } = "";
    [Name("ACCOUNTING DATE")]
    [Order]
    public DateTime? AccountingDate { get; set; }
    [Name("POSTED DATE")]
    [Order]
    public DateTime? PostedDate { get; set; }
    [Name("JOURNAL CATEGORY")]
    [Order]
    public string JournalCategory { get; set; } = "";
    [Name("JOURNAL SOURCE")]
    [Order]
    public string JournalSource { get; set; } = "";
    [Name("ACCOUNT COMBINATION")]
    [Order]
    public string AccountCombination { get; set; } = "";
    [Name("ENTITY")]
    [Order]
    public string Entity { get; set; } = "";
    [Name("ENTITY DESCRIPTION")]
    [Order]
    public string EntityDesctiption { get; set; } = "";
    [Name("FUND")]
    [Order]
    public string Fund { get; set; } = "";
    [Name("FUND DESCRIPTION")]
    [Order]
    public string FundDescription { get; set; } = "";
    [Name("PARENT FUND")]
    [Order]
    public string ParentFund { get; set; } = "";
    [Name("PARENT FUND DESCRIPTION")]
    [Order]
    public string ParentFundDescription { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT PARENT C")]
    [Order]
    public string FinancialDepartmentParentC { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT PARENT C DESCRIPTION")]
    [Order]
    public string FinancialDepartmentParentC_Description { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT")]
    [Order]
    public string FinancialDepartment { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT DESCRIPTION")]
    [Order]
    public string FinancialDepartmentDescription { get; set; } = "";
    [Name("NATURAL ACCOUNT")]
    [Order]
    public string NaturalAccount { get; set; } = "";
    [Name("NATURAL ACCOUNT DESCRIPTION")]
    [Order]
    public string NaturalAccountDescription { get; set; } = "";
    [Name("NATURAL ACCOUNT TYPE")]
    [Order]
    public string NatruralAccountType { get; set; } = "";
    [Name("PURPOSE")]
    [Order]
    public string Purpose { get; set; } = "";
    [Name("PURPOSE DESCRIPTION")]
    [Order]
    public string PurposeDescription { get; set; } = "";
    [Name("PROGRAM")]
    [Order]
    public string Program { get; set; } = "";
    [Name("PROGRAM DESCRIPTION")]
    [Order]
    public string ProgramDescription { get; set; } = "";
    [Name("PROJECT")]
    [Order]
    public string Project { get; set; } = "";
    [Name("PROJECT DESCRIPTION")]
    [Order]
    public string ProjectDescription { get; set; } = "";
    [Name("ACTIVITY")]
    [Order]
    public string Activity { get; set; } = "";
    [Name("ACTIVITY DESCRIPTION")]
    [Order]
    public string ActivityDescription { get; set; } = "";
    [Name("INTER ENTITY")]
    [Order]
    public string InterEntity { get; set; } = "";
    [Name("GL FUTURE 1")]
    [Order]
    public string GL_Future1 { get; set; } = "";
    [Name("GL FUTURE 2")]
    [Order]
    public string GL_Future2 { get; set; } = "";
    [Name("DEBIT AMOUNT")]
    [Order]
    public decimal? DebitAmount { get; set; }
    [Name("CREDIT AMOUNT")]
    [Order]
    public decimal? CreditAmount { get; set; }
}

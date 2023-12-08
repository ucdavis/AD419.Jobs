using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.Models;

public class NifaGlModel
{
    [Name("ACCOUNTING YEAR")]
    public int AccountingYear { get; set; }
    [Name("POSTED YEAR")]
    public int? PostedYear { get; set; }
    [Name("PERIOD")]
    public string Period { get; set; } = "";
    [Name("ACCOUNTING DATE")]
    public DateTime? AccountingDate { get; set; }
    [Name("POSTED DATE")]
    public DateTime? PostedDate { get; set; }
    [Name("JOURNAL CATEGORY")]
    public string JournalCategory { get; set; } = "";
    [Name("JOURNAL SOURCE")]
    public string JournalSource { get; set; } = "";
    [Name("ACCOUNT COMBINATION")]
    public string AccountCombination { get; set; } = "";
    [Name("ENTITY")]
    public string Entity { get; set; } = "";
    [Name("ENTITY DESCRIPTION")]
    public string EntityDesctiption { get; set; } = "";
    [Name("FUND")]
    public string Fund { get; set; } = "";
    [Name("FUND DESCRIPTION")]
    public string FundDescription { get; set; } = "";
    [Name("PARENT FUND")]
    public string ParentFund { get; set; } = "";
    [Name("PARENT FUND DESCRIPTION")]
    public string ParentFundDescription { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT PARENT C")]
    public string FinancialDepartmentParentC { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT PARENT C DESCRIPTION")]
    public string FinancialDepartmentParentC_Description { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT")]
    public string FinancialDepartment { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT DESCRIPTION")]
    public string FinancialDepartmentDescription { get; set; } = "";
    [Name("NATURAL ACCOUNT")]
    public string NaturalAccount { get; set; } = "";
    [Name("NATURAL ACCOUNT DESCRIPTION")]
    public string NaturalAccountDescription { get; set; } = "";
    [Name("NATURAL ACCOUNT TYPE")]
    public string NatruralAccountType { get; set; } = "";
    [Name("PURPOSE")]
    public string Purpose { get; set; } = "";
    [Name("PURPOSE DESCRIPTION")]
    public string PurposeDescription { get; set; } = "";
    [Name("PROGRAM")]
    public string Program { get; set; } = "";
    [Name("PROGRAM DESCRIPTION")]
    public string ProgramDescription { get; set; } = "";
    [Name("PROJECT")]
    public string Project { get; set; } = "";
    [Name("PROJECT DESCRIPTION")]
    public string ProjectDescription { get; set; } = "";
    [Name("ACTIVITY")]
    public string Activity { get; set; } = "";
    [Name("ACTIVITY DESCRIPTION")]
    public string ActivityDescription { get; set; } = "";
    [Name("INTER ENTITY")]
    public string InterEntity { get; set; } = "";
    [Name("GL FUTURE 1")]
    public string GL_Future1 { get; set; } = "";
    [Name("GL FUTURE 2")]
    public string GL_Future2 { get; set; } = "";
    [Name("DEBIT AMOUNT")]
    public decimal? DebitAmount { get; set; }
    [Name("CREDIT AMOUNT")]
    public decimal? CreditAmount { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.PullNifaData.Models;

[Table("UCD_NIFA_GL")]
public class NifaGlModel
{
    [Name("ACCOUNTING YEAR")]
    [DbColumnOrder]
    [Required]
    public int AccountingYear { get; set; }
    [Name("POSTED YEAR")]
    [DbColumnOrder]
    public int? PostedYear { get; set; }
    [Name("PERIOD")]
    [DbColumnOrder]
    [Required]
    public string Period { get; set; } = "";
    [Name("ACCOUNTING DATE")]
    [DbColumnOrder]
    [DbKeyOrder(0)]
    public DateTime? AccountingDate { get; set; }
    [Name("POSTED DATE")]
    [DbColumnOrder]
    [DbKeyOrder(1)]
    public DateTime? PostedDate { get; set; }
    [Name("JOURNAL CATEGORY")]
    [DbColumnOrder]
    [DbKeyOrder(3)]
    [MaxLength(250)]
    public string JournalCategory { get; set; } = "";
    [Name("JOURNAL SOURCE")]
    [DbColumnOrder]
    [DbKeyOrder(2)]
    [MaxLength(150)]
    public string JournalSource { get; set; } = "";
    [Name("ACCOUNT COMBINATION")]
    [DbColumnOrder]
    [DbKeyOrder(4)]
    [Required]
    [MaxLength(150)]
    public string AccountCombination { get; set; } = "";
    [Name("ENTITY")]
    [DbColumnOrder]
    public string Entity { get; set; } = "";
    [Name("ENTITY DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string EntityDesctiption { get; set; } = "";
    [Name("FUND")]
    [DbColumnOrder]
    public string Fund { get; set; } = "";
    [Name("FUND DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string FundDescription { get; set; } = "";
    [Name("PARENT FUND")]
    [DbColumnOrder]
    public string ParentFund { get; set; } = "";
    [Name("PARENT FUND DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string ParentFundDescription { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT PARENT C")]
    [DbColumnOrder]
    public string FinancialDepartmentParentC { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT PARENT C DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string FinancialDepartmentParentC_Description { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string FinancialDepartment { get; set; } = "";
    [Name("FINANCIAL DEPARTMENT DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string FinancialDepartmentDescription { get; set; } = "";
    [Name("NATURAL ACCOUNT")]
    [DbColumnOrder]
    public string NaturalAccount { get; set; } = "";
    [Name("NATURAL ACCOUNT DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string NaturalAccountDescription { get; set; } = "";
    [Name("NATURAL ACCOUNT TYPE")]
    [DbColumnOrder]
    [MaxLength(150)]
    public string NatruralAccountType { get; set; } = "";
    [Name("PURPOSE")]
    [DbColumnOrder]
    public string Purpose { get; set; } = "";
    [Name("PURPOSE DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string PurposeDescription { get; set; } = "";
    [Name("PROGRAM")]
    [DbColumnOrder]
    public string Program { get; set; } = "";
    [Name("PROGRAM DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string ProgramDescription { get; set; } = "";
    [Name("PROJECT")]
    [DbColumnOrder]
    public string Project { get; set; } = "";
    [Name("PROJECT DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string ProjectDescription { get; set; } = "";
    [Name("ACTIVITY")]
    [DbColumnOrder]
    public string Activity { get; set; } = "";
    [Name("ACTIVITY DESCRIPTION")]
    [DbColumnOrder]
    [MaxLength(250)]
    public string ActivityDescription { get; set; } = "";
    [Name("INTER ENTITY")]
    [DbColumnOrder]
    public string InterEntity { get; set; } = "";
    [Name("GL FUTURE 1")]
    [DbColumnOrder]
    public string GL_Future1 { get; set; } = "";
    [Name("GL FUTURE 2")]
    [DbColumnOrder]
    public string GL_Future2 { get; set; } = "";
    [Name("DEBIT AMOUNT")]
    [DbColumnOrder]
    [DbKeyOrder(5)]
    public decimal? DebitAmount { get; set; }
    [Name("CREDIT AMOUNT")]
    [DbColumnOrder]
    [DbKeyOrder(6)]
    public decimal? CreditAmount { get; set; }
    [Name("RUN DATE")]
    [DbColumnOrder]
    public DateTime RunDate { get; set; }
    [Name("EXPENSE TYPE")]
    [DbColumnOrder]
    [DbKeyOrder(7)]
    public string ExpenseType { get; set; } = "";
}

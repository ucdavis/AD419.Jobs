using System.ComponentModel.DataAnnotations.Schema;
using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.PullNifaData.Models;

[Table("UCD_NIFA_PGM_EXPENDITURE")]
public class NifaPgmExpenditureModel
{
    [Name("Run Date")]
    [ColumnOrder]
    public DateTime RunDate { get; set; }

    [Name("Expenditure Project ID")]
    [ColumnOrder]
    [KeyOrder(0)]
    public string ExpenditureProjectID { get; set; } = "";

    [Name("Expenditure Award ID")]
    [ColumnOrder]
    [KeyOrder(1)]
    public string ExpenditureAwardID { get; set; } = "";

    [Name("Expenditure Item Date")]
    [ColumnOrder]
    [KeyOrder(2)]
    public DateTime? ExpenditureItemDate { get; set; }

    [Name("Expenditure Type")]
    [ColumnOrder]
    public string ExpenditureType { get; set; } = "";

    [Name("Expenditure Category")]
    [ColumnOrder]
    public string ExpenditureCategory { get; set; } = "";

    [Name("Burdened Cost Amount")]
    [ColumnOrder]
    public decimal BurdenedCostAmount { get; set; }

    [Name("Task Number")]
    [ColumnOrder]
    public string TaskNumber { get; set; } = "";

    [Name("Task Name")]
    [ColumnOrder]
    public string TaskName { get; set; } = "";

    [Name("Budget Period - Start Date")]
    [ColumnOrder]
    public DateTime? BudgetPeriodStartDate { get; set; }

    [Name("Budget Period - End Date")]
    [ColumnOrder]
    public DateTime? BudgetPeriodEndDate { get; set; }

    [Name("Project Burden Structure base (ie, MTDC)")]
    [ColumnOrder]
    public string ProjectBurdenStructureBase { get; set; } = "";

    [Name("Project Burden Cost Rate")]
    [ColumnOrder]
    public decimal? ProjectBurdenCostRate { get; set; }

    [Name("Project Task Fund DFF")]
    [ColumnOrder]
    public string ProjectTaskFundDFF { get; set; } = "";

    [Name("Project Task Purpose DFF")]
    [ColumnOrder]
    public string ProjectTaskPurposeDFF { get; set; } = "";

    [Name("Project Task Program DFF")]
    [ColumnOrder]
    public string ProjectTaskProgramDFF { get; set; } = "";

    [Name("Project Task Activity DFF")]
    [ColumnOrder]
    public string ProjectTaskActivityDFF { get; set; } = "";
}

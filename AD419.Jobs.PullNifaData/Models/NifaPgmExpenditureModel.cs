using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.Models;

public class NifaPgmExpenditureModel
{
    [Name("Run Date")]
    [Order]
    public DateTime RunDate { get; set; }

    [Name("Expenditure Project ID")]
    [Order]
    public string ExpenditureProjectID { get; set; } = "";

    [Name("Expenditure Award ID")]
    [Order]
    public string ExpenditureAwardID { get; set; } = "";

    [Name("Expenditure Item Date")]
    [Order]
    public DateTime? ExpenditureItemDate { get; set; }

    [Name("Expenditure Type")]
    [Order]
    public string ExpenditureType { get; set; } = "";

    [Name("Expenditure Category")]
    [Order]
    public string ExpenditureCategory { get; set; } = "";

    [Name("Burdened Cost Amount")]
    [Order]
    public decimal BurdenedCostAmount { get; set; }

    [Name("Task Number")]
    [Order]
    public string TaskNumber { get; set; } = "";

    [Name("Task Name")]
    [Order]
    public string TaskName { get; set; } = "";

    [Name("Budget Period - Start Date")]
    [Order]
    public DateTime? BudgetPeriodStartDate { get; set; }

    [Name("Budget Period - End Date")]
    [Order]
    public DateTime? BudgetPeriodEndDate { get; set; }

    [Name("Project Burden Structure base (ie, MTDC)")]
    [Order]
    public string ProjectBurdenStructureBase { get; set; } = "";

    [Name("Project Burden Cost Rate")]
    [Order]
    public decimal ProjectBurdenCostRate { get; set; }

    [Name("Project Task Fund DFF")]
    [Order]
    public string ProjectTaskFundDFF { get; set; } = "";

    [Name("Project Task Purpose DFF")]
    [Order]
    public string ProjectTaskPurposeDFF { get; set; } = "";

    [Name("Project Task Program DFF")]
    [Order]
    public string ProjectTaskProgramDFF { get; set; } = "";

    [Name("Project Task Activity DFF")]
    [Order]
    public string ProjectTaskActivityDFF { get; set; } = "";
}

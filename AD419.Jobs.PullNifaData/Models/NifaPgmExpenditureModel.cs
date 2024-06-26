using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.PullNifaData.Models;

[Table("UCD_NIFA_PGM_EXPENDITURE")]
public class NifaPgmExpenditureModel
{
    [Name("Run Date")]
    [DbColumnOrder]
    public DateTime RunDate { get; set; }

    [Name("Expenditure Project ID")]
    [DbColumnOrder]
    [DbKeyOrder(0)]
    public string ExpenditureProjectID { get; set; } = "";

    [Name("Expenditure Award ID")]
    [DbColumnOrder]
    [DbKeyOrder(1)]
    public string ExpenditureAwardID { get; set; } = "";

    [Name("Expenditure Item Date")]
    [DbColumnOrder]
    [DbKeyOrder(6)]
    public DateTime? ExpenditureItemDate { get; set; }

    [Name("Expenditure Type")]
    [DbColumnOrder]
    [MaxLength(300)]
    [DbKeyOrder(2)]
    public string ExpenditureType { get; set; } = "";

    [Name("Expenditure Category")]
    [DbColumnOrder]
    [MaxLength(300)]
    [DbKeyOrder(3)]
    public string ExpenditureCategory { get; set; } = "";

    [Name("Burdened Cost Amount")]
    [DbColumnOrder]
    [TypeConverter(typeof(DecimalTypeConverter))]
    [DbKeyOrder(4)]
    public decimal BurdenedCostAmount { get; set; }

    [Name("Task Number")]
    [DbColumnOrder]
    [DbKeyOrder(5)]
    public string TaskNumber { get; set; } = "";

    [Name("Task Name")]
    [DbColumnOrder]
    [MaxLength(300)]
    public string TaskName { get; set; } = "";

    [Name("Budget Period - Start Date")]
    [DbColumnOrder]
    public DateTime? BudgetPeriodStartDate { get; set; }

    [Name("Budget Period - End Date")]
    [DbColumnOrder]
    public DateTime? BudgetPeriodEndDate { get; set; }

    [Name("Project Burden Structure base (ie, MTDC)")]
    [DbColumnOrder]
    [DbKeyOrder(7)]
    public string ProjectBurdenStructureBase { get; set; } = "";

    [Name("Project Burden Cost Rate")]
    [DbColumnOrder]
    [DbKeyOrder(8)]
    public decimal? ProjectBurdenCostRate { get; set; }

    [Name("Project Task Fund DFF")]
    [DbColumnOrder]
    public string ProjectTaskFundDFF { get; set; } = "";

    [Name("Project Task Purpose DFF")]
    [DbColumnOrder]
    public string ProjectTaskPurposeDFF { get; set; } = "";

    [Name("Project Task Program DFF")]
    [DbColumnOrder]
    public string ProjectTaskProgramDFF { get; set; } = "";

    [Name("Project Task Activity DFF")]
    [DbColumnOrder]
    public string ProjectTaskActivityDFF { get; set; } = "";
}

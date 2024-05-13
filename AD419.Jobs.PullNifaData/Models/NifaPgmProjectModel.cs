using System.ComponentModel.DataAnnotations.Schema;
using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.PullNifaData.Models;

[Table("UCD_NIFA_PGM_PROJECT")]
public class NifaPgmProjectModel
{
    [Name("Run Date")]
    [ColumnOrder]
    public DateTime RunDate { get; set; }

    [Name("Project ID")]
    [ColumnOrder]
    [KeyOrder(0)]
    public string ProjectID { get; set; } = "";

    [Name("Award ID")]
    [ColumnOrder]
    public string AwardID { get; set; } = "";

    [Name("Project Legal Entity")]
    [ColumnOrder]
    public string ProjectLegalEntity { get; set; } = "";

    [Name("Project Owning Organization")]
    [ColumnOrder]
    public string ProjectOwningOrganization { get; set; } = "";

    [Name("Project Number")]
    [ColumnOrder]
    public string ProjectNumber { get; set; } = "";

    [Name("Project Name")]
    [ColumnOrder]
    public string ProjectName { get; set; } = "";

    [Name("Project Type")]
    [ColumnOrder]
    public string ProjectType { get; set; } = "";

    [Name("Project Start Date")]
    [ColumnOrder]
    public DateTime? ProjectStartDate { get; set; }

    [Name("Project End Date")]
    [ColumnOrder]
    public DateTime? ProjectEndDate { get; set; }

    [Name("Project Closed Date")]
    [ColumnOrder]
    public DateTime? ProjectClosedDate { get; set; }

    [Name("Project Creation Date")]
    [ColumnOrder]
    public DateTime? ProjectCreationDate { get; set; }

    [Name("Project Last Update Date")]
    [ColumnOrder]
    public DateTime? ProjectLastUpdateDate { get; set; }
}

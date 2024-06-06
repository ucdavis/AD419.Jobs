using System.ComponentModel.DataAnnotations.Schema;
using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.PullNifaData.Models;

[Table("UCD_NIFA_PGM_PROJECT")]
public class NifaPgmProjectModel
{
    [Name("Run Date")]
    [DbColumnOrder]
    public DateTime RunDate { get; set; }

    [Name("Project ID")]
    [DbColumnOrder]
    [DbKeyOrder(0)]
    public string ProjectID { get; set; } = "";

    [Name("Award ID")]
    [DbColumnOrder]
    public string AwardID { get; set; } = "";

    [Name("Project Legal Entity")]
    [DbColumnOrder]
    public string ProjectLegalEntity { get; set; } = "";

    [Name("Project Owning Organization")]
    [DbColumnOrder]
    public string ProjectOwningOrganization { get; set; } = "";

    [Name("Project Number")]
    [DbColumnOrder]
    public string ProjectNumber { get; set; } = "";

    [Name("Project Name")]
    [DbColumnOrder]
    public string ProjectName { get; set; } = "";

    [Name("Project Type")]
    [DbColumnOrder]
    public string ProjectType { get; set; } = "";

    [Name("Project Start Date")]
    [DbColumnOrder]
    public DateTime? ProjectStartDate { get; set; }

    [Name("Project End Date")]
    [DbColumnOrder]
    public DateTime? ProjectEndDate { get; set; }

    [Name("Project Closed Date")]
    [DbColumnOrder]
    public DateTime? ProjectClosedDate { get; set; }

    [Name("Project Creation Date")]
    [DbColumnOrder]
    public DateTime? ProjectCreationDate { get; set; }

    [Name("Project Last Update Date")]
    [DbColumnOrder]
    public DateTime? ProjectLastUpdateDate { get; set; }

    [Name("Project Fund")]
    [DbColumnOrder]
    public string ProjectFund { get; set; } = "";
}

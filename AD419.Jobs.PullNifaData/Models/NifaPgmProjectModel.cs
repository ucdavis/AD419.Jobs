using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.Models;

public class NifaPgmProjectModel
{
    [Name("Run Date")]
    [Order]
    public DateTime RunDate { get; set; }

    [Name("Project ID")]
    [Order]
    public string ProjectID { get; set; } = "";

    [Name("Award ID")]
    [Order]
    public string AwardID { get; set; } = "";

    [Name("Project Legal Entity")]
    [Order]
    public string ProjectLegalEntity { get; set; } = "";

    [Name("Project Owning Organization")]
    [Order]
    public string ProjectOwningOrganization { get; set; } = "";

    [Name("Project Number")]
    [Order]
    public string ProjectNumber { get; set; } = "";

    [Name("Project Name")]
    [Order]
    public string ProjectName { get; set; } = "";

    [Name("Project Type")]
    [Order]
    public string ProjectType { get; set; } = "";

    [Name("Project Start Date")]
    [Order]
    public DateTime? ProjectStartDate { get; set; }

    [Name("Project End Date")]
    [Order]
    public DateTime? ProjectEndDate { get; set; }

    [Name("Project Closed Date")]
    [Order]
    public DateTime? ProjectClosedDate { get; set; }

    [Name("Project Creation Date")]
    [Order]
    public DateTime? ProjectCreationDate { get; set; }

    [Name("Project Last Update Date")]
    [Order]
    public DateTime? ProjectLastUpdateDate { get; set; }
}

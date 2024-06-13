using System.ComponentModel.DataAnnotations.Schema;
using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.PullNifaData.Models;


[Table("UCD_NIFA_PGM_EMPLOYEE")]
public class NifaPgmEmployeeModel
{
    [Name("Run Date")]
    [DbColumnOrder]
    public DateTime RunDate { get; set; }

    [Name("Award ID")]
    [DbColumnOrder]
    [DbKeyOrder(0)]
    public string AwardID { get; set; } = "";

    [Name("Project ID")]
    [DbColumnOrder]
    [DbKeyOrder(1)]
    public string ProjectID { get; set; } = "";

    [Name("Person ID")]
    [DbColumnOrder]
    [DbKeyOrder(2)]
    public string PersonID { get; set; } = "";

    [Name("Person Name")]
    [DbColumnOrder]
    public string PersonName { get; set; } = "";

    [Name("Person Number")]
    [DbColumnOrder]
    public string PersonNumber { get; set; } = "";

    [Name("Role")]
    [DbColumnOrder]
    public string Role { get; set; } = "";

    [Name("Role Start Date")]
    [DbColumnOrder]
    public DateTime? RoleStartDate { get; set; }

    [Name("Role End Date")]
    [DbColumnOrder]
    public DateTime? RoleEndDate { get; set; }

    [Name("Person Start Date")]
    [DbColumnOrder]
    public DateTime? PersonStartDate { get; set; }

    [Name("Person End Date")]
    [DbColumnOrder]
    public DateTime? PersonEndDate { get; set; }

    [Name("Source")]
    [DbColumnOrder]
    public string Source { get; set; } = "";
}
using CsvHelper.Configuration.Attributes;
using AD419.Jobs.PullNifaData.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD419.Jobs.PullNifaData.Models;

[Table("UCD_NIFA_PGM_AWARD")]
public class NifaPgmAwardModel
{
    [Name("Run Date")]
    [ColumnOrder]
    public DateTime RunDate { get; set; }

    [Name("Award ID")]
    [ColumnOrder]
    [KeyOrder(0)]
    public string AwardID { get; set; } = "";

    [Name("Award Number")]
    [ColumnOrder]
    public string AwardNumber { get; set; } = "";

    [Name("Award Type")]
    [ColumnOrder]
    public string AwardType { get; set; } = "";

    [Name("Award Purpose")]
    [ColumnOrder]
    public string AwardPurpose { get; set; } = "";

    [Name("Award Start Date")]
    [ColumnOrder]
    [KeyOrder(1)]
    public DateTime? AwardStartDate { get; set; }

    [Name("Award End Date")]
    [ColumnOrder]
    public DateTime? AwardEndDate { get; set; }

    [Name("Award Funding Amount")]
    [ColumnOrder]
    public decimal AwardFundingAmount { get; set; }

    [Name("Sponsor / Customer Name")]
    [ColumnOrder]
    public string Sponsor_CustomerName { get; set; } = "";

    [Name("Sponsor / Customer Number")]
    [ColumnOrder]
    public string Sponsor_CustomerNumber { get; set; } = "";

    [Name("UCOP Sponsor Code")]
    [ColumnOrder]
    public int UCOPSponsorCode { get; set; }

    [Name("Assistance Listing Numbers")]
    [ColumnOrder]
    public string AssistanceListingNumbers { get; set; } = "";

    [Name("Customer Class")]
    [ColumnOrder]
    public string CustomerClass { get; set; } = "";

    [Name("Terms & Conditions-Cost Share")]
    [ColumnOrder]
    public string Terms_Conditions_CostShare { get; set; } = "";

    [Name("Terms & Conditions-NSF Field of Study")]
    [ColumnOrder]
    public string Terms_Conditions_NSFFieldOfStudy { get; set; } = "";

    [Name("Invoice/LOC (Award Document Number)")]
    [ColumnOrder]
    public string Invoice_LOC_AwardDocumentNumber { get; set; } = "";

    [Name("LOC Number(Sponsor LOC Number)")]
    [ColumnOrder]
    public string LOCNumber_SponsorLOCNumber { get; set; } = "";

    [Name("Federal Prime Name")]
    [ColumnOrder]
    public string FederalPrimeName { get; set; } = "";

    [Name("Federal Prime Number")]
    [ColumnOrder]
    public string FederalPrimeNumber { get; set; } = "";

    [Name("NIFA Accession Number")]
    [ColumnOrder]
    public string NIFAAccessionNumber { get; set; } = "";

    [Name("NIFA Project Number")]
    [ColumnOrder]
    public string NIFAProjectNumber { get; set; } = "";
}

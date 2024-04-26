using CsvHelper.Configuration.Attributes;
using AD419.Jobs.PullNifaData.Attributes;

namespace AD419.Jobs.Models;

public class NifaPgmAwardModel
{
    [Name("Run Date")]
    [Order]
    public DateTime RunDate { get; set; }

    [Name("Award ID")]
    [Order]
    public string AwardID { get; set; } = "";

    [Name("Award Number")]
    [Order]
    public string AwardNumber { get; set; } = "";

    [Name("Award Type")]
    [Order]
    public string AwardType { get; set; } = "";

    [Name("Award Purpose")]
    [Order]
    public string AwardPurpose { get; set; } = "";

    [Name("Award Start Date")]
    [Order]
    public DateTime? AwardStartDate { get; set; }

    [Name("Award End Date")]
    [Order]
    public DateTime? AwardEndDate { get; set; }

    [Name("Award Funding Amount")]
    [Order]
    public decimal AwardFundingAmount { get; set; }

    [Name("Sponsor / Customer Name")]
    [Order]
    public string Sponsor_CustomerName { get; set; } = "";

    [Name("Sponsor / Customer Number")]
    [Order]
    public string Sponsor_CustomerNumber { get; set; } = "";

    [Name("UCOP Sponsor Code")]
    [Order]
    public int UCOPSponsorCode { get; set; }

    [Name("Assistance Listing Numbers")]
    [Order]
    public string AssistanceListingNumbers { get; set; } = "";

    [Name("Customer Class")]
    [Order]
    public string CustomerClass { get; set; } = "";

    [Name("Terms & Conditions-Cost Share")]
    [Order]
    public string Terms_Conditions_CostShare { get; set; } = "";

    [Name("Terms & Conditions-NSF Field of Study")]
    [Order]
    public string Terms_Conditions_NSFFieldOfStudy { get; set; } = "";

    [Name("Invoice/LOC (Award Document Number)")]
    [Order]
    public string Invoice_LOC_AwardDocumentNumber { get; set; } = "";

    [Name("LOC Number(Sponsor LOC Number)")]
    [Order]
    public string LOCNumber_SponsorLOCNumber { get; set; } = "";

    [Name("Federal Prime Name")]
    [Order]
    public string FederalPrimeName { get; set; } = "";

    [Name("Federal Prime Number")]
    [Order]
    public string FederalPrimeNumber { get; set; } = "";

    [Name("NIFA Accession Number")]
    [Order]
    public string NIFAAccessionNumber { get; set; } = "";

    [Name("NIFA Project Number")]
    [Order]
    public string NIFAProjectNumber { get; set; } = "";
}

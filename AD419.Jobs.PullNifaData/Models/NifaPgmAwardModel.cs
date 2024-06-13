using CsvHelper.Configuration.Attributes;
using AD419.Jobs.PullNifaData.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AD419.Jobs.PullNifaData.Models;

[Table("UCD_NIFA_PGM_AWARD")]
public class NifaPgmAwardModel
{
    [Name("Run Date")]
    [DbColumnOrder]
    public DateTime RunDate { get; set; }

    [Name("Award ID")]
    [DbColumnOrder]
    [DbKeyOrder(0)]
    public string AwardID { get; set; } = "";

    [Name("Award Number")]
    [DbColumnOrder]
    public string AwardNumber { get; set; } = "";

    [Name("Award Type")]
    [DbColumnOrder]
    public string AwardType { get; set; } = "";

    [Name("Award Purpose")]
    [DbColumnOrder]
    public string AwardPurpose { get; set; } = "";

    [Name("Award Start Date")]
    [DbColumnOrder]
    [DbKeyOrder(1)]
    public DateTime? AwardStartDate { get; set; }

    [Name("Award End Date")]
    [DbColumnOrder]
    public DateTime? AwardEndDate { get; set; }

    [Name("Award Funding Amount")]
    [DbColumnOrder]
    public decimal? AwardFundingAmount { get; set; }

    [Name("Sponsor / Customer Name")]
    [DbColumnOrder]
    public string Sponsor_CustomerName { get; set; } = "";

    [Name("Sponsor / Customer Number")]
    [DbColumnOrder]
    public string Sponsor_CustomerNumber { get; set; } = "";

    [Name("UCOP Sponsor Code")]
    [DbColumnOrder]
    public string UCOPSponsorCode { get; set; } = "";

    [Name("Assistance Listing Numbers")]
    [DbColumnOrder]
    public string AssistanceListingNumbers { get; set; } = "";

    [Name("Customer Class")]
    [DbColumnOrder]
    public string CustomerClass { get; set; } = "";

    [Name("Terms & Conditions-Cost Share")]
    [DbColumnOrder]
    public string Terms_Conditions_CostShare { get; set; } = "";

    [Name("Terms & Conditions-NSF Field of Study")]
    [DbColumnOrder]
    public string Terms_Conditions_NSFFieldOfStudy { get; set; } = "";

    [Name("Invoice/LOC (Award Document Number)")]
    [DbColumnOrder]
    public string Invoice_LOC_AwardDocumentNumber { get; set; } = "";

    [Name("LOC Number(Sponsor LOC Number)")]
    [DbColumnOrder]
    public string LOCNumber_SponsorLOCNumber { get; set; } = "";

    [Name("Federal Prime Name")]
    [DbColumnOrder]
    public string FederalPrimeName { get; set; } = "";

    [Name("Federal Prime Number")]
    [DbColumnOrder]
    public string FederalPrimeNumber { get; set; } = "";

    [Name("NIFA Accession Number")]
    [DbColumnOrder]
    public string NIFAAccessionNumber { get; set; } = "";

    [Name("NIFA Project Number")]
    [DbColumnOrder]
    public string NIFAProjectNumber { get; set; } = "";

    [Name("Award Name")]
    [DbColumnOrder]
    [MaxLength(300)]
    public string AwardName { get; set; } = "";

    [Name("Award Description")]
    [DbColumnOrder]
    public string AwardDescription { get; set; } = "";

    [Name("Award Organization")]
    [DbColumnOrder]
    [MaxLength(300)]
    public string AwardOrganization { get; set; } = "";

    [Name("Award Status")]
    [DbColumnOrder]
    public string AwardStatus { get; set; } = "";

    [Name("FlowThrough  Funds  Reference  Award  Name")]
    [DbColumnOrder]
    public string FlowThroughFundsReferenceAwardName { get; set; } = "";

    [Name("FlowThrough  Funds  Amount")]
    [DbColumnOrder]
    public decimal? FlowThroughFundsAmount { get; set; }

    [Name("FlowThrough  Funded  by  Federal  Agency")]
    [DbColumnOrder]
    public string FlowThroughFundedByFederalAgency { get; set; } = "";
}

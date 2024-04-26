MERGE INTO UCD_NIFA_PGM_AWARD_NEW AS target
USING #UCD_NIFA_PGM_AWARD AS source ON (
  target.AwardID = source.AwardID
  AND target.AwardStartDate = source.AwardStartDate
)
WHEN MATCHED THEN UPDATE SET
  target.RunDate = source.RunDate,
  target.AwardNumber = source.AwardNumber,
  target.AwardType = source.AwardType,
  target.AwardPurpose = source.AwardPurpose,
  target.AwardEndDate = source.AwardEndDate,
  target.AwardFundingAmount = source.AwardFundingAmount,
  target.Sponsor_CustomerName = source.Sponsor_CustomerName,
  target.Sponsor_CustomerNumber = source.Sponsor_CustomerNumber,
  target.UCOPSponsorCode = source.UCOPSponsorCode,
  target.AssistanceListingNumbers = source.AssistanceListingNumbers,
  target.CustomerClass = source.CustomerClass,
  target.Terms_Conditions_CostShare = source.Terms_Conditions_CostShare,
  target.Terms_Conditions_NSFFieldOfStudy = source.Terms_Conditions_NSFFieldOfStudy,
  target.Invoice_LOC_AwardDocumentNumber = source.Invoice_LOC_AwardDocumentNumber,
  target.LOCNumber_SponsorLOCNumber = source.LOCNumber_SponsorLOCNumber,
  target.FederalPrimeName = source.FederalPrimeName,
  target.FederalPrimeNumber = source.FederalPrimeNumber,
  target.NIFAAccessionNumber = source.NIFAAccessionNumber,
  target.NIFAProjectNumber = source.NIFAProjectNumber
WHEN NOT MATCHED THEN INSERT (
  RunDate,
  AwardID,
  AwardNumber,
  AwardType,
  AwardPurpose,
  AwardStartDate,
  AwardEndDate,
  AwardFundingAmount,
  Sponsor_CustomerName,
  Sponsor_CustomerNumber,
  UCOPSponsorCode,
  AssistanceListingNumbers,
  CustomerClass,
  Terms_Conditions_CostShare,
  Terms_Conditions_NSFFieldOfStudy,
  Invoice_LOC_AwardDocumentNumber,
  LOCNumber_SponsorLOCNumber,
  FederalPrimeName,
  FederalPrimeNumber,
  NIFAAccessionNumber,
  NIFAProjectNumber
)
VALUES
(
  source.RunDate,
  source.AwardID,
  source.AwardNumber,
  source.AwardType,
  source.AwardPurpose,
  source.AwardStartDate,
  source.AwardEndDate,
  source.AwardFundingAmount,
  source.Sponsor_CustomerName,
  source.Sponsor_CustomerNumber,
  source.UCOPSponsorCode,
  source.AssistanceListingNumbers,
  source.CustomerClass,
  source.Terms_Conditions_CostShare,
  source.Terms_Conditions_NSFFieldOfStudy,
  source.Invoice_LOC_AwardDocumentNumber,
  source.LOCNumber_SponsorLOCNumber,
  source.FederalPrimeName,
  source.FederalPrimeNumber,
  source.NIFAAccessionNumber,
  source.NIFAProjectNumber
);

go

truncate table #UCD_NIFA_PGM_AWARD;

go;
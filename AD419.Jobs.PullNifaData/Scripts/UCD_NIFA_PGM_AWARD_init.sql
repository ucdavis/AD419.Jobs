IF OBJECT_ID (N'dbo.#UCD_NIFA_PGM_AWARD', N'U') IS NULL BEGIN

CREATE TABLE #UCD_NIFA_PGM_AWARD (
  [RunDate] date,
  [AwardID] varchar(50),
  [AwardNumber] varchar(50),
  [AwardType] varchar(50),
  [AwardPurpose] varchar(50),
  [AwardStartDate] date,
  [AwardEndDate] date,
  [AwardFundingAmount] decimal(18, 2),
  [Sponsor_CustomerName] varchar(50),
  [Sponsor_CustomerNumber] varchar(50),
  [UCOPSponsorCode] varchar(50),
  [AssistanceListingNumbers] varchar(50),
  [CustomerClass] varchar(50),
  [Terms_Conditions_CostShare] varchar(50),
  [Terms_Conditions_NSFFieldOfStudy] varchar(50),
  [Invoice_LOC_AwardDocumentNumber] varchar(50),
  [LOCNumber_SponsorLOCNumber] varchar(50),
  [FederalPrimeName] varchar(50),
  [FederalPrimeNumber] varchar(50),
  [NIFAAccessionNumber] varchar(50),
  [NIFAProjectNumber] varchar(50)
);

CREATE UNIQUE CLUSTERED INDEX PK_TempUcdNifaPgmAward on #UCD_NIFA_PGM_AWARD (
  [AwardID],
  [AwardStartDate]
);

END

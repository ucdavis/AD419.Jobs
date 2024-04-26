IF OBJECT_ID (N'dbo.#UCD_NIFA_PGM_PROJECT', N'U') IS NULL BEGIN

CREATE TABLE dbo.#UCD_NIFA_PGM_PROJECT (
  [RunDate] DATE,
  [ProjectID] VARCHAR(50),
  [AwardID] VARCHAR(50),
  [ProjectLegalEntity] VARCHAR(100),
  [ProjectOwningOrganization] VARCHAR(100),
  [ProjectNumber] VARCHAR(50),
  [ProjectName] VARCHAR(100),
  [ProjectType] VARCHAR(50),
  [ProjectStartDate] DATE,
  [ProjectEndDate] DATE,
  [ProjectClosedDate] DATE,
  [ProjectCreationDate] DATE,
  [ProjectLastUpdateDate] DATE
);

CREATE UNIQUE CLUSTERED INDEX PK_TempUcdNifaPgmProject ON dbo.#UCD_NIFA_PGM_PROJECT (
  [ProjectID]
);

END
IF OBJECT_ID (N'dbo.#UCD_NIFA_PGM_EMPLOYEE', N'U') IS NULL BEGIN

CREATE TABLE #UCD_NIFA_PGM_EMPLOYEE (
  [RunDate] date,
  [AwardID] varchar(50),
  [ProjectID] varchar(50),
  [PersonID] varchar(50),
  [PersonName] varchar(50),
  [PersonNumber] varchar(50),
  [Role] varchar(50),
  [RoleStartDate] date,
  [RoleEndDate] date,
  [PersonStartDate] date,
  [PersonEndDate] date
);

CREATE UNIQUE CLUSTERED INDEX PK_TempUcdNifaPgmEmployee on #UCD_NIFA_PGM_EMPLOYEE (
  [AwardID],
  [ProjectID],
  [PersonID]
);

END
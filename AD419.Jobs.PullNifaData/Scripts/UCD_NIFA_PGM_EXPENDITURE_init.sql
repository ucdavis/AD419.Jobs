IF OBJECT_ID (N'dbo.#UCD_NIFA_PGM_EXPENDITURE', N'U') IS NULL BEGIN

CREATE TABLE #UCD_NIFA_PGM_EXPENDITURE (
  [RunDate] DATE,
  [ExpenditureProjectID] VARCHAR(50),
  [ExpenditureAwardID] VARCHAR(50),
  [ExpenditureItemDate] DATE,
  [ExpenditureType] VARCHAR(100),
  [ExpenditureCategory] VARCHAR(100),
  [BurdenedCostAmount] DECIMAL(18, 2),
  [TaskNumber] VARCHAR(50),
  [TaskName] VARCHAR(100),
  [BudgetPeriodStartDate] DATE,
  [BudgetPeriodEndDate] DATE,
  [ProjectBurdenStructurebase] VARCHAR(100),
  [ProjectBurdenCostRate] DECIMAL(18, 2),
  [ProjectTaskFundDFF] VARCHAR(50),
  [ProjectTaskPurposeDFF] VARCHAR(50),
  [ProjectTaskProgramDFF] VARCHAR(50),
  [ProjectTaskActivityDFF] VARCHAR(50)
);

CREATE UNIQUE CLUSTERED INDEX PK_TempUcdNifaPgmExpenditure ON #UCD_NIFA_PGM_EXPENDITURE (
  [ExpenditureProjectID], 
  [ExpenditureAwardID], 
  [ExpenditureItemDate]
);

END
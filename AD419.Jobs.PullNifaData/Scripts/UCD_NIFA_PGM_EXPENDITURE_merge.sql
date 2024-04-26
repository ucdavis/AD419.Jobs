MERGE INTO UCD_NIFA_PGM_EXPENDITURE_NEW AS target
USING #UCD_NIFA_PGM_EXPENDITURE AS source ON (
  target.ExpenditureProjectID = source.ExpenditureProjectID
  AND target.ExpenditureAwardID = source.ExpenditureAwardID
  AND target.ExpenditureItemDate = source.ExpenditureItemDate
)
WHEN MATCHED THEN UPDATE SET
  target.RunDate = source.RunDate,
  target.ExpenditureType = source.ExpenditureType,
  target.ExpenditureCategory = source.ExpenditureCategory,
  target.BurdenedCostAmount = source.BurdenedCostAmount,
  target.TaskNumber = source.TaskNumber,
  target.TaskName = source.TaskName,
  target.BudgetPeriodStartDate = source.BudgetPeriodStartDate,
  target.BudgetPeriodEndDate = source.BudgetPeriodEndDate,
  target.ProjectBurdenStructurebase = source.ProjectBurdenStructurebase,
  target.ProjectBurdenCostRate = source.ProjectBurdenCostRate,
  target.ProjectTaskFundDFF = source.ProjectTaskFundDFF,
  target.ProjectTaskPurposeDFF = source.ProjectTaskPurposeDFF,
  target.ProjectTaskProgramDFF = source.ProjectTaskProgramDFF,
  target.ProjectTaskActivityDFF = source.ProjectTaskActivityDFF
WHEN NOT MATCHED THEN INSERT (
  RunDate,
  ExpenditureProjectID,
  ExpenditureAwardID,
  ExpenditureItemDate,
  ExpenditureType,
  ExpenditureCategory,
  BurdenedCostAmount,
  TaskNumber,
  TaskName,
  BudgetPeriodStartDate,
  BudgetPeriodEndDate,
  ProjectBurdenStructurebase,
  ProjectBurdenCostRate,
  ProjectTaskFundDFF,
  ProjectTaskPurposeDFF,
  ProjectTaskProgramDFF,
  ProjectTaskActivityDFF
)
VALUES
(
  source.RunDate,
  source.ExpenditureProjectID,
  source.ExpenditureAwardID,
  source.ExpenditureItemDate,
  source.ExpenditureType,
  source.ExpenditureCategory,
  source.BurdenedCostAmount,
  source.TaskNumber,
  source.TaskName,
  source.BudgetPeriodStartDate,
  source.BudgetPeriodEndDate,
  source.ProjectBurdenStructurebase,
  source.ProjectBurdenCostRate,
  source.ProjectTaskFundDFF,
  source.ProjectTaskPurposeDFF,
  source.ProjectTaskProgramDFF,
  source.ProjectTaskActivityDFF
);

go

truncate table #UCD_NIFA_PGM_EXPENDITURE;

go

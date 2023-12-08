MERGE INTO UCD_NIFA_GL_New AS target
USING #UCD_NIFA_GL AS source ON (
  target.AccountingDate = source.AccountingDate
  AND target.PostedDate = source.PostedDate
  AND target.JournalSource = source.JournalSource
  AND target.JournalCategory = source.JournalCategory
  AND target.AccountCombination = source.AccountCombination
  -- DebitAmount and CreditAmount can contain null, making joining on them a bit more complex...
  AND (target.DebitAmount = source.DebitAmount OR (target.DebitAmount IS NULL AND source.DebitAmount IS NULL))
  AND (target.CreditAmount = source.CreditAmount OR (target.CreditAmount IS NULL AND source.CreditAmount IS NULL))
) 
WHEN MATCHED THEN UPDATE SET
  target.AccountingYear = source.AccountingYear,
  target.PostedYear = source.PostedYear,
  target.[Period] = source.[Period],
  target.Entity = source.Entity,
  target.EntityDesctiption = source.EntityDesctiption,
  target.Fund = source.Fund,
  target.FundDescription = source.FundDescription,
  target.ParentFund = source.ParentFund,
  target.ParentFundDescription = source.ParentFundDescription,
  target.FinancialDepartmentParentC = source.FinancialDepartmentParentC,
  target.FinancialDepartmentParentC_Description = source.FinancialDepartmentParentC_Description,
  target.FinancialDepartment = source.FinancialDepartment,
  target.FinancialDepartmentDescription = source.FinancialDepartmentDescription,
  target.NaturalAccount = source.NaturalAccount,
  target.NaturalAccountDescription = source.NaturalAccountDescription,
  target.NatruralAccountType = source.NatruralAccountType,
  target.Purpose = source.Purpose,
  target.PurposeDescription = source.PurposeDescription,
  target.Program = source.Program,
  target.ProgramDescription = source.ProgramDescription,
  target.Project = source.Project,
  target.ProjectDescription = source.ProjectDescription,
  target.Activity = source.Activity,
  target.ActivityDescription = source.ActivityDescription,
  target.InterEntity = source.InterEntity,
  target.GL_Future1 = source.GL_Future1,
  target.GL_Future2 = source.GL_Future2
WHEN NOT MATCHED THEN INSERT (
  AccountingYear,
  PostedYear,
  [Period],
  AccountingDate,
  PostedDate,
  JournalCategory,
  JournalSource,
  AccountCombination,
  Entity,
  EntityDesctiption,
  Fund,
  FundDescription,
  ParentFund,
  ParentFundDescription,
  FinancialDepartmentParentC,
  FinancialDepartmentParentC_Description,
  FinancialDepartment,
  FinancialDepartmentDescription,
  NaturalAccount,
  NaturalAccountDescription,
  NatruralAccountType,
  Purpose,
  PurposeDescription,
  Program,
  ProgramDescription,
  Project,
  ProjectDescription,
  Activity,
  ActivityDescription,
  InterEntity,
  GL_Future1,
  GL_Future2,
  DebitAmount,
  CreditAmount
)
VALUES
(
  source.AccountingYear,
  source.PostedYear,
  source.[Period],
  source.AccountingDate,
  source.PostedDate,
  source.JournalCategory,
  source.JournalSource,
  source.AccountCombination,
  source.Entity,
  source.EntityDesctiption,
  source.Fund,
  source.FundDescription,
  source.ParentFund,
  source.ParentFundDescription,
  source.FinancialDepartmentParentC,
  source.FinancialDepartmentParentC_Description,
  source.FinancialDepartment,
  source.FinancialDepartmentDescription,
  source.NaturalAccount,
  source.NaturalAccountDescription,
  source.NatruralAccountType,
  source.Purpose,
  source.PurposeDescription,
  source.Program,
  source.ProgramDescription,
  source.Project,
  source.ProjectDescription,
  source.Activity,
  source.ActivityDescription,
  source.InterEntity,
  source.GL_Future1,
  source.GL_Future2,
  source.DebitAmount,
  source.CreditAmount
);

go

truncate table #UCD_NIFA_GL;

go
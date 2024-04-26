MERGE INTO UCD_NIFA_PGM_EMPLOYEE_NEW AS target
USING #UCD_NIFA_PGM_EMPLOYEE AS source ON (
  target.AwardID = source.AwardID
  AND target.ProjectID = source.ProjectID
  AND target.PersonID = source.PersonID
)
WHEN MATCHED THEN UPDATE SET
  target.RunDate = source.RunDate,
  target.PersonName = source.PersonName,
  target.PersonNumber = source.PersonNumber,
  target.Role = source.Role,
  target.RoleStartDate = source.RoleStartDate,
  target.RoleEndDate = source.RoleEndDate,
  target.PersonStartDate = source.PersonStartDate,
  target.PersonEndDate = source.PersonEndDate
WHEN NOT MATCHED THEN INSERT (
  RunDate,
  AwardID,
  ProjectID,
  PersonID,
  PersonName,
  PersonNumber,
  Role,
  RoleStartDate,
  RoleEndDate,
  PersonStartDate,
  PersonEndDate
)
VALUES
(
  source.RunDate,
  source.AwardID,
  source.ProjectID,
  source.PersonID,
  source.PersonName,
  source.PersonNumber,
  source.Role,
  source.RoleStartDate,
  source.RoleEndDate,
  source.PersonStartDate,
  source.PersonEndDate
);

go

truncate table #UCD_NIFA_PGM_EMPLOYEE;

go

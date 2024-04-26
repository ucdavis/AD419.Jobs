MERGE INTO UCD_NIFA_PGM_PROJECT_NEW AS target
USING dbo.#UCD_NIFA_PGM_PROJECT AS source
ON (target.[ProjectID] = source.[ProjectID])
WHEN MATCHED THEN
  UPDATE SET
    target.[RunDate] = source.[RunDate],
    target.[AwardID] = source.[AwardID],
    target.[ProjectLegalEntity] = source.[ProjectLegalEntity],
    target.[ProjectOwningOrganization] = source.[ProjectOwningOrganization],
    target.[ProjectNumber] = source.[ProjectNumber],
    target.[ProjectName] = source.[ProjectName],
    target.[ProjectType] = source.[ProjectType],
    target.[ProjectStartDate] = source.[ProjectStartDate],
    target.[ProjectEndDate] = source.[ProjectEndDate],
    target.[ProjectClosedDate] = source.[ProjectClosedDate],
    target.[ProjectCreationDate] = source.[ProjectCreationDate],
    target.[ProjectLastUpdateDate] = source.[ProjectLastUpdateDate]
WHEN NOT MATCHED THEN
  INSERT (
    [RunDate],
    [ProjectID],
    [AwardID],
    [ProjectLegalEntity],
    [ProjectOwningOrganization],
    [ProjectNumber],
    [ProjectName],
    [ProjectType],
    [ProjectStartDate],
    [ProjectEndDate],
    [ProjectClosedDate],
    [ProjectCreationDate],
    [ProjectLastUpdateDate]
  )
  VALUES (
    source.[RunDate],
    source.[ProjectID],
    source.[AwardID],
    source.[ProjectLegalEntity],
    source.[ProjectOwningOrganization],
    source.[ProjectNumber],
    source.[ProjectName],
    source.[ProjectType],
    source.[ProjectStartDate],
    source.[ProjectEndDate],
    source.[ProjectClosedDate],
    source.[ProjectCreationDate],
    source.[ProjectLastUpdateDate]
  );

go

truncate table dbo.#UCD_NIFA_PGM_PROJECT;

go

truncate table ErpProjectValues;

go

insert into ErpProjectValues([Id], [Code], [Name], [ParentCode])
select [Id], [Code], [Name], [ParentCode] from #ErpProjectValues;

go

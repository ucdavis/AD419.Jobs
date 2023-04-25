truncate table ErpAccountValues;

go

insert into ErpAccountValues([Id], [Code], [Name], [ParentCode])
select [Id], [Code], [Name], [ParentCode] from #ErpAccountValues;

go

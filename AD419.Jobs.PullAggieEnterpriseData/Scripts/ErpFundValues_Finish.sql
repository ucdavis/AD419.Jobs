truncate table ErpFundValues;

go

insert into ErpFundValues([Id], [Code], [Name], [ParentCode])
select [Id], [Code], [Name], [ParentCode] from #ErpFundValues;

go

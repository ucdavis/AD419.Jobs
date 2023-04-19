truncate table ErpFinancialDepartmentValues;

go

insert into ErpFinancialDepartmentValues([Id], [Code], [Name], [ParentCode])
select [Id], [Code], [Name], [ParentCode] from #ErpFinancialDepartmentValues;

go

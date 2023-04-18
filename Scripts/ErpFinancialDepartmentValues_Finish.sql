drop table if exists ErpFinancialDepartmentValues;

go

select * into ErpFinancialDepartmentValues from #ErpFinancialDepartmentValues;

go

create index ix_ErpFinancialDepartmentValues_Code on ErpFinancialDepartmentValues ([Code]);
create index ix_ErpFinancialDepartmentValues_ParentCode on ErpFinancialDepartmentValues ([ParentCode]);

go
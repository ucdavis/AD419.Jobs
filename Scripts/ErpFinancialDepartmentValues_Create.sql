drop table if exists #ErpFinancialDepartmentValues;

go
create table
  #ErpFinancialDepartmentValues (
    Id bigint,
    Code nvarchar (255),
    Name nvarchar (255),
    ParentCode nvarchar (255)
  );

go
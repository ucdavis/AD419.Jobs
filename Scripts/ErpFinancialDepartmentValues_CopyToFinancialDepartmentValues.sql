create index ix_Code on #ErpFinancialDepartmentValues ([Code]);

go

create index ix_ParentCode on #ErpFinancialDepartmentValues ([ParentCode]);

go

drop table if exists FinancialDepartmentValues;

go

select
    g.Code as [LEVEL_G],
    g.Name as [LEVEL_G_NAME],
    f.Code as [LEVEL_F],
    f.Name as [LEVEL_F_NAME],
    e.Code as [LEVEL_E],
    e.Name as [LEVEL_E_NAME],
    d.Code as [LEVEL_D],
    d.Name as [LEVEL_D_NAME],
    c.Code as [LEVEL_C],
    c.Name as [LEVEL_C_NAME],
    b.Code as [LEVEL_B],
    b.Name as [LEVEL_B_NAME],
    a.Code as [LEVEL_A],
    a.Name as [LEVEL_A_NAME]
into FinancialDepartmentValues
from
    #ErpFinancialDepartmentValues as [a]
    left join #ErpFinancialDepartmentValues as [b] on b.ParentCode = a.Code
    left join #ErpFinancialDepartmentValues as [c] on c.ParentCode = b.Code
    left join #ErpFinancialDepartmentValues as [d] on d.ParentCode = c.Code
    left join #ErpFinancialDepartmentValues as [e] on e.ParentCode = d.Code
    left join #ErpFinancialDepartmentValues as [f] on f.ParentCode = e.Code
    left join #ErpFinancialDepartmentValues as [g] on g.ParentCode = f.Code
where
    a.Code like '%A';

go
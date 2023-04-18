drop table if exists ErpProjectValues;

go

select * into ErpProjectValues from #ErpProjectValues;

go

create index ix_ErpProjectValues_Code on ErpProjectValues ([Code]);
create index ix_ErpProjectValues_ParentCode on ErpProjectValues ([ParentCode]);

go
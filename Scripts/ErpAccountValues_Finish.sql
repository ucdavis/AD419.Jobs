drop table if exists ErpAccountValues;

go

select * into ErpAccountValues from #ErpAccountValues;

go

create index ix_ErpAccountValues_Code on ErpAccountValues ([Code]);
create index ix_ErpAccountValues_ParentCode on ErpAccountValues ([ParentCode]);

go
drop table if exists ErpFundValues;

go

select * into ErpFundValues from #ErpFundValues;

go

create index ix_ErpFundValues_Code on ErpFundValues ([Code]);
create index ix_ErpFundValues_ParentCode on ErpFundValues ([ParentCode]);

go
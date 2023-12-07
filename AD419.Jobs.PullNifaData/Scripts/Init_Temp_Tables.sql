IF OBJECT_ID (N'dbo.#UCD_NIFA_GL', N'U') IS NULL BEGIN
CREATE TABLE
  #UCD_NIFA_GL (
    AccountingYear int not null,
    PostedYear int,
    Period varchar(10) not null,
    AccountingDate date,
    PostedDate date,
    JournalCategory varchar(50),
    JournalSource varchar(50),
    AccountCombination varchar(150) not null,
    Entity varchar(5),
    EntityDesctiption varchar(50),
    Fund varchar(10),
    FundDescription varchar(150),
    ParentFund varchar(10),
    ParentFundDescription varchar(150),
    FinancialDepartmentParentC varchar(10),
    FinancialDepartmentParentC_Description varchar(150),
    FinancialDepartment varchar(10),
    FinancialDepartmentDescription varchar(150),
    NaturalAccount varchar(10),
    NaturalAccountDescription varchar(150),
    NatruralAccountType varchar(50),
    Purpose varchar(5),
    PurposeDescription varchar(150),
    Program varchar(5),
    ProgramDescription varchar(150),
    Project varchar(20),
    ProjectDescription varchar(250),
    Activity varchar(10),
    ActivityDescription varchar(150),
    InterEntity varchar(5),
    GL_Future1 varchar(10),
    GL_Future2 varchar(10),
    DebitAmount decimal(18, 2),
    CreditAmount decimal(18, 2)
  );

CREATE UNIQUE CLUSTERED INDEX PK_UcdNifaGlNew ON #UCD_NIFA_GL (
  AccountingDate,
  PostedDate,
  JournalSource,
  JournalCategory,
  AccountCombination,
  DebitAmount,
  CreditAmount
);

END
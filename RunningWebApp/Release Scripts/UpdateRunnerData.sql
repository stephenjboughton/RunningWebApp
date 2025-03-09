if not exists (select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA ='dbo' and TABLE_NAME = 'runner' and COLUMN_NAME = 'emailAddress')
begin
	Alter Table [dbo].[runner]
	Add [emailAddress] nvarchar(254) not null default('')

end
go

if not exists (select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA ='dbo' and TABLE_NAME = 'runner' and COLUMN_NAME = 'password')
begin
	Alter Table [dbo].[runner]
	Add [password] nvarchar(50) not null default('')

end
go

if not exists (select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA ='dbo' and TABLE_NAME = 'runner' and COLUMN_NAME = 'salt')
begin
	Alter Table [dbo].[runner]
	Add [salt] nvarchar(50) not null default('')

end
go
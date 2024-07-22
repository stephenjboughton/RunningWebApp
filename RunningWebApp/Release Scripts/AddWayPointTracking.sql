if exists (select * from dbo.sysobjects where ID = object_id(N'[dbo].[WayPoint]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[WayPoint]
GO
create table [dbo].[WayPoint]
(
	--primary key
	[WayPointId] int not null identity(1,1)

	--foreign keys
	,[RunId] int not null default(0)

	--data
	,[Lattitude] float not null default(0)
	,[Longitude] float not null default(0)
	,[Time] DateTime	 not null default('')

	--audits


	,Constraint WayPoint_PK Primary Key Clustered (WayPointId)
)
GO
create nonclustered index WayPoint_RunId on [dbo].[WayPoint](RunId)
GO




CREATE TYPE [dbo].[WayPoint] AS TABLE(  
    [RunId] [int] NOT NULL,  
    [Lattitude] [float] NOT NULL,  
    [Longitude] [float] NOT NULL,  
    [Time] [datetime] NOT NULL    
)  


if exists ( select * from sys.objects where object_id = OBJECT_ID(N'dbo.WayPoints__Insert') and type in (N'P', N'PC'))
drop procedure [dbo].[WayPoints__Insert]
go

create procedure [dbo].[WayPoints__Insert](@tableWayPoints WayPoint readonly)  
as  
begin  
   insert into WayPoint select [RunId],[Lattitude],[Longitude],[time] from @tableWayPoints  
end 
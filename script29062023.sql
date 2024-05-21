insert into dbo.catalogo(nombreCatalogo,estadoCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion) 
values('Efecto en Calificacion',1,'lgalarza',getdate(),'PC1')
go
insert into dbo.catalogo(nombreCatalogo,estadoCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion) 
values('Elementos Parametros Calificacion',1,'lgalarza',getdate(),'PC1')
go
insert into dbo.catalogo(nombreCatalogo,estadoCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion) 
values('Humedad Relativa',1,'lgalarza',getdate(),'PC1')
go

declare @codigoCatalogo smallint
--Efecto en Calificacion
select @codigoCatalogo = codigoCatalogo from dbo.catalogo where nombreCatalogo = 'Efecto en Calificacion'

insert into dbo.detalleCatalogo(codigoCatalogo,nombreDetalleCatalogo,valoracion,valoracion2,valoracion3,valoracion4,estadoDetalleCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion)
values(@codigoCatalogo,'RECHAZO',NULL,NULL,NULL,NULL,1,'lgalarza',getdate(),'PC1')

insert into dbo.detalleCatalogo(codigoCatalogo,nombreDetalleCatalogo,valoracion,valoracion2,valoracion3,valoracion4,estadoDetalleCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion)
values(@codigoCatalogo,'PENALIZA',NULL,NULL,NULL,NULL,1,'lgalarza',getdate(),'PC1')
go

declare @codigoCatalogo smallint
--Elementos Parametros Calificacion
select @codigoCatalogo = codigoCatalogo from dbo.catalogo where nombreCatalogo = 'Elementos Parametros Calificacion'

insert into dbo.detalleCatalogo(codigoCatalogo,nombreDetalleCatalogo,valoracion,valoracion2,valoracion3,valoracion4,estadoDetalleCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion)
values(@codigoCatalogo,'PIZARRA',NULL,NULL,NULL,NULL,1,'lgalarza',getdate(),'PC1')

insert into dbo.detalleCatalogo(codigoCatalogo,nombreDetalleCatalogo,valoracion,valoracion2,valoracion3,valoracion4,estadoDetalleCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion)
values(@codigoCatalogo,'VIOLETA',NULL,NULL,NULL,NULL,1,'lgalarza',getdate(),'PC1')

insert into dbo.detalleCatalogo(codigoCatalogo,nombreDetalleCatalogo,valoracion,valoracion2,valoracion3,valoracion4,estadoDetalleCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion)
values(@codigoCatalogo,'HONGO',NULL,NULL,NULL,NULL,1,'lgalarza',getdate(),'PC1')

insert into dbo.detalleCatalogo(codigoCatalogo,nombreDetalleCatalogo,valoracion,valoracion2,valoracion3,valoracion4,estadoDetalleCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion)
values(@codigoCatalogo,'IMPUREZA',NULL,NULL,NULL,NULL,1,'lgalarza',getdate(),'PC1')
go

declare @codigoCatalogo smallint
--Humedad Relativa
select @codigoCatalogo = codigoCatalogo from dbo.catalogo where nombreCatalogo = 'Humedad Relativa'

insert into dbo.detalleCatalogo(codigoCatalogo,nombreDetalleCatalogo,valoracion,valoracion2,valoracion3,valoracion4,estadoDetalleCatalogo,usuarioCreacion,fechaCreacion,equipoCreacion)
values(@codigoCatalogo,'HUMEDAD RELATIVA',0.07,NULL,NULL,NULL,1,'lgalarza',getdate(),'PC1')
go

------------------------------------------------------------------------------
CREATE TABLE [dbo].[parametrosCalificacion](
	[efectoParametro] [smallint] NOT NULL,
	[elementoParametro] [smallint] NOT NULL,
	[convencional] [decimal](4, 2) NOT NULL,
	[utz] [decimal](4, 2) NOT NULL,
 CONSTRAINT [PK_parametrosCalificacion] PRIMARY KEY CLUSTERED 
(
	[efectoParametro] ASC,
	[elementoParametro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[parametrosCalificacion] ([efectoParametro], [elementoParametro], [convencional], [utz]) VALUES (15, 17, CAST(0.21 AS Decimal(4, 2)), CAST(0.18 AS Decimal(4, 2)))
GO
INSERT [dbo].[parametrosCalificacion] ([efectoParametro], [elementoParametro], [convencional], [utz]) VALUES (15, 18, CAST(0.28 AS Decimal(4, 2)), CAST(0.26 AS Decimal(4, 2)))
GO
INSERT [dbo].[parametrosCalificacion] ([efectoParametro], [elementoParametro], [convencional], [utz]) VALUES (15, 19, CAST(0.07 AS Decimal(4, 2)), CAST(0.07 AS Decimal(4, 2)))
GO
INSERT [dbo].[parametrosCalificacion] ([efectoParametro], [elementoParametro], [convencional], [utz]) VALUES (16, 17, CAST(0.19 AS Decimal(4, 2)), CAST(0.10 AS Decimal(4, 2)))
GO
INSERT [dbo].[parametrosCalificacion] ([efectoParametro], [elementoParametro], [convencional], [utz]) VALUES (16, 18, CAST(0.26 AS Decimal(4, 2)), CAST(0.21 AS Decimal(4, 2)))
GO
INSERT [dbo].[parametrosCalificacion] ([efectoParametro], [elementoParametro], [convencional], [utz]) VALUES (16, 19, CAST(0.05 AS Decimal(4, 2)), CAST(0.03 AS Decimal(4, 2)))
GO
INSERT [dbo].[parametrosCalificacion] ([efectoParametro], [elementoParametro], [convencional], [utz]) VALUES (16, 20, CAST(0.04 AS Decimal(4, 2)), CAST(0.04 AS Decimal(4, 2)))
GO

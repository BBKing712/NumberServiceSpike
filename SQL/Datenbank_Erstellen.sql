USE [Nummernservice]
GO
/****** Object:  Table [dbo].[Datentypen]    Script Date: 18.03.2021 19:21:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Datentypen](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Bezeichnung] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Datentypen_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nummerdefinitionen]    Script Date: 18.03.2021 19:21:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nummerdefinitionen](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Bezeichnung] [nvarchar](50) NOT NULL,
	[QuelleBezeichnung] [nvarchar](50) NOT NULL,
	[ZielDatentypenID] [bigint] NOT NULL,
	[ZielBezeichnung] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Nummerdefinitionen_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nummerdefinitionquellen]    Script Date: 18.03.2021 19:21:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nummerdefinitionquellen](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[NummerDefinitionenID] [bigint] NOT NULL,
	[Position] [bigint] NOT NULL,
	[DatentypenID] [bigint] NOT NULL,
	[Bezeichnung] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Nummerdefinitionquellen_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nummerinformationen]    Script Date: 18.03.2021 19:21:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nummerinformationen](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[NummerdefinitionID] [bigint] NOT NULL,
	[Quelle] [nvarchar](max) NOT NULL,
	[Ziel] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Nummerinformationen_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Datentypen] ON 

INSERT [dbo].[Datentypen] ([ID], [Bezeichnung], [CreatedAt], [UpdatedAt]) VALUES (1, N'String', CAST(N'2021-03-18T18:17:39.023' AS DateTime), CAST(N'2021-03-18T18:17:39.023' AS DateTime))
INSERT [dbo].[Datentypen] ([ID], [Bezeichnung], [CreatedAt], [UpdatedAt]) VALUES (2, N'Integer', CAST(N'2021-03-18T18:17:52.573' AS DateTime), CAST(N'2021-03-18T18:17:52.573' AS DateTime))
INSERT [dbo].[Datentypen] ([ID], [Bezeichnung], [CreatedAt], [UpdatedAt]) VALUES (3, N'Guid', CAST(N'2021-03-18T18:18:13.387' AS DateTime), CAST(N'2021-03-18T18:18:13.387' AS DateTime))
SET IDENTITY_INSERT [dbo].[Datentypen] OFF
ALTER TABLE [dbo].[Datentypen] ADD  CONSTRAINT [DF_datentyp_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Datentypen] ADD  CONSTRAINT [DF_datentyp_UpdatedAt]  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Nummerdefinitionen] ADD  CONSTRAINT [DF_nummer_definition_nummer_definition_guid]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[Nummerdefinitionen] ADD  CONSTRAINT [DF_nummer_definition_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Nummerdefinitionen] ADD  CONSTRAINT [DF_nummer_definition_UpdatedAt]  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Nummerdefinitionquellen] ADD  CONSTRAINT [DF_nummer_definition_quelle_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Nummerdefinitionquellen] ADD  CONSTRAINT [DF_nummer_definition_quelle_UpdatedAt]  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Nummerinformationen] ADD  CONSTRAINT [DF_nummer_information_nummer_information_guid]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[Nummerinformationen] ADD  CONSTRAINT [DF_nummer_information_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Nummerinformationen] ADD  CONSTRAINT [DF_nummer_information_UpdatedAt]  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Nummerdefinitionen]  WITH CHECK ADD  CONSTRAINT [FK_Nummerdefinitionen__Datentypen_ID] FOREIGN KEY([ZielDatentypenID])
REFERENCES [dbo].[Datentypen] ([ID])
GO
ALTER TABLE [dbo].[Nummerdefinitionen] CHECK CONSTRAINT [FK_Nummerdefinitionen__Datentypen_ID]
GO
ALTER TABLE [dbo].[Nummerdefinitionquellen]  WITH CHECK ADD  CONSTRAINT [FK_Nummerdefinitionquellen_Datentypen_ID] FOREIGN KEY([DatentypenID])
REFERENCES [dbo].[Datentypen] ([ID])
GO
ALTER TABLE [dbo].[Nummerdefinitionquellen] CHECK CONSTRAINT [FK_Nummerdefinitionquellen_Datentypen_ID]
GO
ALTER TABLE [dbo].[Nummerdefinitionquellen]  WITH CHECK ADD  CONSTRAINT [FK_Nummerdefinitionquellen_Nummerdefinitionen_ID] FOREIGN KEY([NummerDefinitionenID])
REFERENCES [dbo].[Nummerdefinitionen] ([ID])
GO
ALTER TABLE [dbo].[Nummerdefinitionquellen] CHECK CONSTRAINT [FK_Nummerdefinitionquellen_Nummerdefinitionen_ID]
GO
ALTER TABLE [dbo].[Nummerinformationen]  WITH CHECK ADD  CONSTRAINT [FK_Nummerinformationen_NummerDefinitionen_ID] FOREIGN KEY([NummerdefinitionID])
REFERENCES [dbo].[Nummerdefinitionen] ([ID])
GO
ALTER TABLE [dbo].[Nummerinformationen] CHECK CONSTRAINT [FK_Nummerinformationen_NummerDefinitionen_ID]
GO

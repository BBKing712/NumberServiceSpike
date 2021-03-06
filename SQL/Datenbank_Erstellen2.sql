USE Nummernservice
GO
/****** Object:  Table [dbo].[datentyp]    Script Date: 18.03.2021 18:35:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[datentyp](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[datentyp_bezeichnung] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_datentyp] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[nummer_definition]    Script Date: 18.03.2021 18:35:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nummer_definition](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[nummer_definition_guid] [uniqueidentifier] NOT NULL,
	[nummer_definition_bezeichnung] [nvarchar](50) NOT NULL,
	[nummer_definition_quelle_bezeichnung] [nvarchar](50) NOT NULL,
	[nummer_definition_ziel_datentyp_id] [bigint] NOT NULL,
	[nummer_definition_ziel_bezeichnung] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_nummer_definition] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[nummer_definition_quelle]    Script Date: 18.03.2021 18:35:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nummer_definition_quelle](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[nummer_definition_id] [bigint] NOT NULL,
	[nummer_definition_quelle_pos] [bigint] NOT NULL,
	[nummer_definition_quelle_datentyp_id] [bigint] NOT NULL,
	[nummer_definition_quelle_bezeichnung] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_nummer_definition_quelle] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[nummer_information]    Script Date: 18.03.2021 18:35:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nummer_information](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[nummer_information_guid] [uniqueidentifier] NOT NULL,
	[nummer_definition_id] [bigint] NOT NULL,
	[Nnmmer_information_quelle] [nvarchar](max) NOT NULL,
	[nummer_information_ziel] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_nummer_information] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[datentyp] ADD  CONSTRAINT [DF_datentyp_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[datentyp] ADD  CONSTRAINT [DF_datentyp_UpdatedAt]  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[nummer_definition] ADD  CONSTRAINT [DF_nummer_definition_nummer_definition_guid]  DEFAULT (newid()) FOR [nummer_definition_guid]
GO
ALTER TABLE [dbo].[nummer_definition] ADD  CONSTRAINT [DF_nummer_definition_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[nummer_definition] ADD  CONSTRAINT [DF_nummer_definition_UpdatedAt]  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[nummer_definition_quelle] ADD  CONSTRAINT [DF_nummer_definition_quelle_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[nummer_definition_quelle] ADD  CONSTRAINT [DF_nummer_definition_quelle_UpdatedAt]  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[nummer_information] ADD  CONSTRAINT [DF_nummer_information_nummer_information_guid]  DEFAULT (newid()) FOR [nummer_information_guid]
GO
ALTER TABLE [dbo].[nummer_information] ADD  CONSTRAINT [DF_nummer_information_CreatedAt]  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[nummer_information] ADD  CONSTRAINT [DF_nummer_information_UpdatedAt]  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[nummer_definition]  WITH CHECK ADD  CONSTRAINT [FK_nummer_definition_datentyp] FOREIGN KEY([nummer_definition_ziel_datentyp_id])
REFERENCES [dbo].[datentyp] ([ID])
GO
ALTER TABLE [dbo].[nummer_definition] CHECK CONSTRAINT [FK_nummer_definition_datentyp]
GO
ALTER TABLE [dbo].[nummer_definition_quelle]  WITH CHECK ADD  CONSTRAINT [FK_nummer_definition_quelle_datentyp] FOREIGN KEY([nummer_definition_quelle_datentyp_id])
REFERENCES [dbo].[datentyp] ([ID])
GO
ALTER TABLE [dbo].[nummer_definition_quelle] CHECK CONSTRAINT [FK_nummer_definition_quelle_datentyp]
GO
ALTER TABLE [dbo].[nummer_information]  WITH CHECK ADD  CONSTRAINT [FK_nummer_information_nummer_definition] FOREIGN KEY([nummer_definition_id])
REFERENCES [dbo].[nummer_definition] ([ID])
GO
ALTER TABLE [dbo].[nummer_information] CHECK CONSTRAINT [FK_nummer_information_nummer_definition]
GO

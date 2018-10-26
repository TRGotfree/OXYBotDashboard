USE [telegramBotOXY]
GO

/****** Object:  Table [dbo].[District]    Script Date: 26.10.2018 15:03:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[District](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](100) NULL,
	[COMMAND] [nvarchar](100) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[District] ADD  CONSTRAINT [DF_District_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO



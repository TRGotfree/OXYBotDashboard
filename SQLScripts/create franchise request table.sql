USE [telegramBotOXY]
GO

/****** Object:  Table [dbo].[FranchiseRequest]    Script Date: 23.04.2019 12:17:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FranchiseRequest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TelegramUserId] [int] NOT NULL,
	[PhoneAndName] [nvarchar](100) NOT NULL,
	[DateTimeEntered] [datetime] NOT NULL,
 CONSTRAINT [PK_FranchiseRequest] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FranchiseRequest] ADD  CONSTRAINT [DF_FranchiseRequest_DateTimeEntered]  DEFAULT (getdate()) FOR [DateTimeEntered]
GO

ALTER TABLE [dbo].[FranchiseRequest]  WITH CHECK ADD  CONSTRAINT [FK_FranchiseRequest_TelegramUsers] FOREIGN KEY([TelegramUserId])
REFERENCES [dbo].[TelegramUsers] ([Id])
GO

ALTER TABLE [dbo].[FranchiseRequest] CHECK CONSTRAINT [FK_FranchiseRequest_TelegramUsers]
GO



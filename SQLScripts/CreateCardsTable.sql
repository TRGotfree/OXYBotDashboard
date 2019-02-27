USE [telegramBotOXY]
GO

/****** Object:  Table [dbo].[Cards]    Script Date: 27.02.2019 16:21:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cards](
	[ID] [int] NOT NULL,
	[CARD_ID] [nvarchar](50) NOT NULL,
	[USER_FIO] [nvarchar](200) NOT NULL,
	[BIRTH_DATE] [date] NOT NULL,
	[PHONE] [nvarchar](50) NOT NULL,
	[EMAIL] [nvarchar](50) NULL,
	[SEX] [nvarchar](6) NOT NULL,
	[IS_USER_WANT_GET_NEWS] [bit] NOT NULL,
	[IS_REGISTERED] [bit] NOT NULL,
	[TELEGRAM_USER_ID] [int] NOT NULL,
 CONSTRAINT [PK_CARDS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Cards] ADD  CONSTRAINT [DF_CARDS_IS_USER_WANT_GET_NEWS]  DEFAULT ((1)) FOR [IS_USER_WANT_GET_NEWS]
GO

ALTER TABLE [dbo].[Cards] ADD  CONSTRAINT [DF_CARDS_IS_REGISTERED]  DEFAULT ((0)) FOR [IS_REGISTERED]
GO

ALTER TABLE [dbo].[Cards]  WITH CHECK ADD  CONSTRAINT [FK_CARDS_TG_USERS] FOREIGN KEY([TELEGRAM_USER_ID])
REFERENCES [dbo].[TelegramUsers] ([Id])
GO

ALTER TABLE [dbo].[Cards] CHECK CONSTRAINT [FK_CARDS_TG_USERS]
GO



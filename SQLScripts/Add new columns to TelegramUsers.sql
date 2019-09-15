ALTER TABLE dbo.TelegramUsers ADD IsActive BIT;
ALTER TABLE dbo.TelegramUsers ADD LastMessageDate DATETIME;

ALTER TABLE [dbo].[TelegramUsers] ADD  CONSTRAINT [DF_TelegramUsers_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[TelegramUsers] ADD  CONSTRAINT [DF_TelegramUsers_LastMessageDate]  DEFAULT (getdate()) FOR [LastMessageDate]
GO

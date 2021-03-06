INSERT INTO dbo.TelegramUsers (ChatId, NickName, UserFirstName, UserLastName)
SELECT mi.ChatId, mi.UserName, mi.UserFirstName, mi.UserLastName
FROM dbo.MessagesInfo mi
INNER JOIN (
 SELECT ChatId, MAX(MessageId) As MaxMessageId FROM dbo.MessagesInfo
 GROUP BY ChatId
) miWithMax ON mi.ChatId = miWithMax.ChatId AND mi.MessageId = miWithMax.MaxMessageId
WHERE mi.ChatId > 0
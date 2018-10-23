using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.DataBaseDomen
{
    public class SqlScripts
    {
        public readonly static string GetBotAdmin = "dbo.getBotAdmin";
        public readonly static string CheckBotAdmin = "dbo.checkBotAdmin";
        public readonly static string GetAllTelegramUsers = "dbo.getAllTelegramUsers";
        public readonly static string GetTelegramUsersPageByPage = "dbo.getTelegramUsers";
        public readonly static string GetAdvertisingActions = "dbo.getActionsInfo";
        public readonly static string UpdateAdvertisingAction = "dbo.updateAction";
        public readonly static string InsertAdvertisingAction = "dbo.InsertNewDiscount_Actions";
        public readonly static string GetDrugStores = "dbo.getDrugStores";
        public readonly static string InsertDrugStore = "dbo.insertDrugStore";
        public readonly static string UpdateDrugStore = "dbo.updateDrugStore";
    }
}

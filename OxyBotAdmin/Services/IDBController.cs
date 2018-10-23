using OxyBotAdmin.DataBaseDomen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public interface IDBController
    {
        LoginDbController GetLoginDbController();

        TelegramBotUsersDBController GetTGUsersConroller();

        AdvertActionsDBController GetAdvertActionsDBController();

        DrugStoreDBController GetDrugStoreDBController();
    }
}

namespace OxyBotAdmin.Repository
{
    public class SqlScripts
    {
        public static string GetBotAdmin => "dbo.getBotAdmin";

        public static string CheckBotAdmin => "dbo.checkBotAdmin";

        public static string GetAllTelegramUsers => "dbo.getAllTelegramUsers";

        public static string GetTelegramUsersPageByPage => "dbo.getTelegramUsers";

        public static string GetAdvertisingActions => "dbo.getActionsInfo";

        public static string UpdateAdvertisingAction => "dbo.updateAction";

        public static string InsertAdvertisingAction => "dbo.InsertNewDiscount_Actions";

        public static string GetDrugStores => "dbo.getDrugStores";

        public static string InsertOrUpdateDrugStore => "dbo.insertOrUpdateDrugStore";

        public static string UpdateDrugStore => "dbo.updateDrugStore";

        public static string GetDistricts => "dbo.getDistricts";

        public static string GetRequests => "dbo.getUserRequest";

        public static string GetAnnotations => "dbo.getGoodAnnotation";

        public static string GetAnnotationById => "dbo.getGoodAnnotationById";

        public static string UpdateAnnotation => "dbo.updateAnnotation";

        public static string InsertOrUpdateAnnotation => "dbo.insertOrUpdateAnnotation";

        public static string UpdateOrInsertAnnotationImage => "dbo.insertOrUpdateAnnotationImage";

        public static string GetDiscountsCardsData => "dbo.getCardByPage";

        public static string InsertOrUpdateDiscountCardData => "dbo.insertOrUpdateCardData";
    }
}

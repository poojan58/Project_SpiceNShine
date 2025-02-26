namespace SpiceAndShine.Models
{
    public static class Common
    {
        public static string SiteURL { get; set; }
        public static string SiteCDNBaseURL { get; set; }
        public static string DBConnectionString { get; set; }
        public static class SessionKeys
        {
            public const string ManagerSession = "ManagerSession";
        }
        public static class StoredProcedureNames
        {
            public const string manager_ManagerLogin = "manager_ManagerLogin";
            public const string manager_GetProductCategoryDetailsById = "manager_GetProductCategoryDetailsById";
            public const string manager_AddEditProductCategory = "manager_AddEditProductCategory";
            public const string manager_GetProductCategoryGrid = "manager_GetProductCategoryGrid";
            public const string manager_DeleteProductCategory = "manager_DeleteProductCategory";

            public const string manager_GetProductDetailsById = "manager_GetProductDetailsById";
            public const string manager_AddEditProduct = "manager_AddEditProduct";
            public const string manager_GetProductGrid = "manager_GetProductGrid";
            public const string manager_DeleteProduct = "manager_DeleteProduct";
        }
        public static class Messages
        {
            public const string LoginSuccess = "Login Successfully";
            public const string UserNotAvailable = "User Name or Email does not exists";
            public const string IncorrectPassword = "Incorrect password";
            public const string RestaurantIsNotActive = "You are currently not active, Kindly contact admin or email on support email address.";
            public const string LoginFailed = "Failed to login";
        }
    }
}

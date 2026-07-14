using WareHouseApp.People;

namespace WareHouseApp
{
    public static class SessionManager
    {
        public static string CurrentUser { get; set; } = null;
        public static string Role { get; set; } = null;

        public static void ClearSession()
        {
            CurrentUser = null;
            Role = null;
        }
    }
}

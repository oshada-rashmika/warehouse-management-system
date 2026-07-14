using WareHouseApp.People;

namespace WareHouseApp
{
    public static class SessionManager
    {
        public static Person CurrentUser { get; set; } = null;

        public static bool IsAdmin => CurrentUser is Admin;

        public static void ClearSession()
        {
            CurrentUser = null;
        }
    }
}

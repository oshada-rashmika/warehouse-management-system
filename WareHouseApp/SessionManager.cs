using WareHouseApp.People;

namespace WareHouseApp
{
    public static class SessionManager
    {
        public static string Username { get; set; } = null;
        public static string Role { get; set; } = null;

        public static void ClearSession()
        {
            Username = null;
            Role = null;
        }
    }
}

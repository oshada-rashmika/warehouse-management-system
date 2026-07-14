using WareHouseApp.People;

namespace WareHouseApp
{
    /// <summary>
    /// Tracks the currently authenticated user across the application lifetime.
    /// Set CurrentUser immediately after a successful login; clear it on logout.
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// The Person subclass instance that completed authentication.
        /// Null when no user is logged in.
        /// </summary>
        public static Person CurrentUser { get; set; } = null;

        /// <summary>Returns true if an Admin is currently logged in.</summary>
        public static bool IsAdmin => CurrentUser is Admin;

        /// <summary>Clears the session (use on logout).</summary>
        public static void ClearSession()
        {
            CurrentUser = null;
        }
    }
}

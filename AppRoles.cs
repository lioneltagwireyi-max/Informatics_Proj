using System;

namespace PhoneFit
{
    /// <summary>
    /// Canonical role names for post-login routing and page access checks.
    /// The database Role table may store Admin (preferred) or the legacy Manager name.
    /// </summary>
    public static class AppRoles
    {
        public const string Customer = "Customer";
        public const string Admin = "Admin";
        public const string Manager = "Manager";

        public static bool IsAdminRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }

            return string.Equals(roleName, Admin, StringComparison.OrdinalIgnoreCase)
                || string.Equals(roleName, Manager, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsCustomerRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }

            return string.Equals(roleName, Customer, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetPostLoginRedirect(string roleName)
        {
            if (IsAdminRole(roleName))
            {
                return "Manager.aspx";
            }

            return "Home.aspx";
        }
    }
}

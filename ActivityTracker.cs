using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace PhoneFit
{
    /// <summary>
    /// Lightweight customer web-activity logger.
    /// Writes to App_Data (always) and optionally to SQL CustomerWebActivity
    /// when the PhoneFitDB connection string is available.
    /// </summary>
    public static class ActivityTracker
    {
        public const string ActionLogin = "Login";
        public const string ActionPageView = "PageView";
        public const string ActionAddToCart = "AddToCart";
        public const string ActionCheckout = "Checkout";

        private static readonly object FileLock = new object();
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public static void LogCustomerAction(string actionType, string pagePath, string detail = null)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                if (context == null || context.Session == null)
                {
                    return;
                }

                string roleName = context.Session["RoleName"] as string;
                if (!AppRoles.IsCustomerRole(roleName))
                {
                    return;
                }

                int? userId = null;
                if (context.Session["UserID"] != null)
                {
                    userId = Convert.ToInt32(context.Session["UserID"]);
                }

                Log(userId, roleName, actionType, pagePath, detail);
            }
            catch
            {
                // Activity logging must never break the shopper experience.
            }
        }

        public static void LogLogin(int userId, string roleName)
        {
            try
            {
                if (!AppRoles.IsCustomerRole(roleName))
                {
                    return;
                }

                Log(userId, roleName, ActionLogin, "Login.aspx", "Customer signed in");
            }
            catch
            {
            }
        }

        public static void Log(int? userId, string roleName, string actionType, string pagePath, string detail)
        {
            ActivityRecord record = new ActivityRecord
            {
                UserID = userId,
                RoleName = roleName ?? string.Empty,
                ActionType = actionType ?? string.Empty,
                PagePath = pagePath ?? string.Empty,
                Detail = detail ?? string.Empty,
                OccurredAt = DateTime.Now
            };

            AppendToFile(record);
            TryInsertSql(record);
        }

        public static ActivityStats GetStats(int recentDays = 7, int recentEventLimit = 25)
        {
            List<ActivityRecord> events = ReadAllEvents();
            DateTime since = DateTime.Now.AddDays(-recentDays);

            List<ActivityRecord> recentWindow = events
                .Where(e => e.OccurredAt >= since)
                .ToList();

            ActivityStats stats = new ActivityStats
            {
                TotalEvents = events.Count,
                EventsLastDays = recentWindow.Count,
                RecentDayWindow = recentDays,
                LoginCountLastDays = recentWindow.Count(e =>
                    string.Equals(e.ActionType, ActionLogin, StringComparison.OrdinalIgnoreCase)),
                PageViewCountLastDays = recentWindow.Count(e =>
                    string.Equals(e.ActionType, ActionPageView, StringComparison.OrdinalIgnoreCase)),
                AddToCartCountLastDays = recentWindow.Count(e =>
                    string.Equals(e.ActionType, ActionAddToCart, StringComparison.OrdinalIgnoreCase)),
                CheckoutCountLastDays = recentWindow.Count(e =>
                    string.Equals(e.ActionType, ActionCheckout, StringComparison.OrdinalIgnoreCase)),
                ActiveCustomersLastDays = recentWindow
                    .Where(e => e.UserID.HasValue)
                    .Select(e => e.UserID.Value)
                    .Distinct()
                    .Count(),
                TopPages = recentWindow
                    .Where(e => string.Equals(e.ActionType, ActionPageView, StringComparison.OrdinalIgnoreCase))
                    .GroupBy(e => string.IsNullOrWhiteSpace(e.PagePath) ? "(unknown)" : e.PagePath)
                    .Select(g => new NamedCount { Name = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(8)
                    .ToList(),
                TopActions = recentWindow
                    .GroupBy(e => string.IsNullOrWhiteSpace(e.ActionType) ? "(unknown)" : e.ActionType)
                    .Select(g => new NamedCount { Name = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToList(),
                RecentCustomers = recentWindow
                    .Where(e => e.UserID.HasValue)
                    .GroupBy(e => e.UserID.Value)
                    .Select(g => new RecentCustomerActivity
                    {
                        UserID = g.Key,
                        EventCount = g.Count(),
                        LastActivityAt = g.Max(x => x.OccurredAt),
                        LastAction = g.OrderByDescending(x => x.OccurredAt).First().ActionType,
                        LastPage = g.OrderByDescending(x => x.OccurredAt).First().PagePath
                    })
                    .OrderByDescending(x => x.LastActivityAt)
                    .Take(15)
                    .ToList(),
                RecentEvents = events
                    .OrderByDescending(e => e.OccurredAt)
                    .Take(recentEventLimit)
                    .ToList()
            };

            return stats;
        }

        private static void AppendToFile(ActivityRecord record)
        {
            string path = GetLogFilePath();
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string line = Serializer.Serialize(record);

            lock (FileLock)
            {
                File.AppendAllText(path, line + Environment.NewLine);
            }
        }

        private static void TryInsertSql(ActivityRecord record)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PhoneFitDB"] != null
                ? ConfigurationManager.ConnectionStrings["PhoneFitDB"].ConnectionString
                : null;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"INSERT INTO dbo.CustomerWebActivity
                            (UserID, RoleName, ActionType, PagePath, Detail, OccurredAt)
                          VALUES
                            (@UserID, @RoleName, @ActionType, @PagePath, @Detail, @OccurredAt)";

                    command.Parameters.AddWithValue("@UserID",
                        (object)record.UserID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RoleName",
                        (object)record.RoleName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ActionType", record.ActionType);
                    command.Parameters.AddWithValue("@PagePath", record.PagePath);
                    command.Parameters.AddWithValue("@Detail",
                        string.IsNullOrEmpty(record.Detail) ? (object)DBNull.Value : record.Detail);
                    command.Parameters.AddWithValue("@OccurredAt", record.OccurredAt);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                // SQL is optional; App_Data remains the reliable local store.
            }
        }

        private static List<ActivityRecord> ReadAllEvents()
        {
            List<ActivityRecord> fromSql = TryReadSqlEvents();
            if (fromSql != null && fromSql.Count > 0)
            {
                return fromSql;
            }

            return ReadFileEvents();
        }

        private static List<ActivityRecord> TryReadSqlEvents()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PhoneFitDB"] != null
                ? ConfigurationManager.ConnectionStrings["PhoneFitDB"].ConnectionString
                : null;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            try
            {
                List<ActivityRecord> list = new List<ActivityRecord>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"SELECT UserID, RoleName, ActionType, PagePath, Detail, OccurredAt
                          FROM dbo.CustomerWebActivity
                          ORDER BY OccurredAt DESC";

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ActivityRecord
                            {
                                UserID = reader["UserID"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(reader["UserID"]),
                                RoleName = reader["RoleName"] as string ?? string.Empty,
                                ActionType = reader["ActionType"] as string ?? string.Empty,
                                PagePath = reader["PagePath"] as string ?? string.Empty,
                                Detail = reader["Detail"] as string ?? string.Empty,
                                OccurredAt = Convert.ToDateTime(reader["OccurredAt"])
                            });
                        }
                    }
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        private static List<ActivityRecord> ReadFileEvents()
        {
            string path = GetLogFilePath();
            if (!File.Exists(path))
            {
                return new List<ActivityRecord>();
            }

            List<ActivityRecord> list = new List<ActivityRecord>();

            lock (FileLock)
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    try
                    {
                        ActivityRecord record = Serializer.Deserialize<ActivityRecord>(line);
                        if (record != null)
                        {
                            list.Add(record);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return list;
        }

        private static string GetLogFilePath()
        {
            string appData = HostingEnvironment.MapPath("~/App_Data");
            if (string.IsNullOrEmpty(appData))
            {
                appData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            }

            return Path.Combine(appData, "customer-activity.jsonl");
        }
    }

    public class ActivityRecord
    {
        public int? UserID { get; set; }
        public string RoleName { get; set; }
        public string ActionType { get; set; }
        public string PagePath { get; set; }
        public string Detail { get; set; }
        public DateTime OccurredAt { get; set; }
    }

    public class NamedCount
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class RecentCustomerActivity
    {
        public int UserID { get; set; }
        public int EventCount { get; set; }
        public DateTime LastActivityAt { get; set; }
        public string LastAction { get; set; }
        public string LastPage { get; set; }
    }

    public class ActivityStats
    {
        public int TotalEvents { get; set; }
        public int EventsLastDays { get; set; }
        public int RecentDayWindow { get; set; }
        public int LoginCountLastDays { get; set; }
        public int PageViewCountLastDays { get; set; }
        public int AddToCartCountLastDays { get; set; }
        public int CheckoutCountLastDays { get; set; }
        public int ActiveCustomersLastDays { get; set; }
        public List<NamedCount> TopPages { get; set; }
        public List<NamedCount> TopActions { get; set; }
        public List<RecentCustomerActivity> RecentCustomers { get; set; }
        public List<ActivityRecord> RecentEvents { get; set; }
    }
}

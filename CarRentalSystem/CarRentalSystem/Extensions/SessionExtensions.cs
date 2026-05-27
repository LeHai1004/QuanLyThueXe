using Microsoft.AspNetCore.Http;

namespace CarRentalSystem.Extensions
{
    public static class SessionExtensions
    {
        public static string GetFullName(this ISession session)
        {
            return session.GetString("FullName") ?? "Người dùng";
        }

        public static string GetRoleName(this ISession session)
        {
            return session.GetString("RoleName") ?? "Khách";
        }

        public static int? GetAccountId(this ISession session)
        {
            var idString = session.GetString("AccountId");
            if (int.TryParse(idString, out int id))
            {
                return id;
            }
            return null;
        }
    }
}
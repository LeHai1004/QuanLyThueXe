namespace CarRentalSystem.Extensions
{
    public static class StringExtensions
    {
        public static string MaskNationalId(this string id)
        {
            if (string.IsNullOrEmpty(id) || id.Length != 12)
            {
                return id;
            }
            return id.Substring(0, 3) + "******" + id.Substring(9, 3);
        }
    }
}
namespace SellingBook.Utils
{
    public class UUIDConverter
    {
        public static Guid ConvertToGuid(string uuid)
        {
            return new Guid(uuid);
        }

        public static string ConvertToString(Guid guid)
        {
            return guid.ToString();
        }
    }
}

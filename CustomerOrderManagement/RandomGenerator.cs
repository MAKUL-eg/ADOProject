using System;
using System.Linq;

namespace CustomerOrderManagement
{
    public class RandomGenerator
    {
        private static Random random = new Random();
        public static string GenerateUniqueOrderNumber()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string randomString = GenerateRandomString(5);
            return timestamp + randomString;
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

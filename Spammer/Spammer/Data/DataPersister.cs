using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Spammer
{
    public static class DataPersister
    {
        private static string sessionKey { get; set; }

        private static string hostUrl = "http://localhost:37131/api/";

        public static void RegisterUser(string username, string password, string email)
        {
            RegisterUserData register = new RegisterUserData()
            {
                AuthCode = Cryptography.CalculateSHA1(username, password),
                Email = email,
                Username = username
            };

            sessionKey = HttpRequester.Post<string>(hostUrl + "user/register", register);
        }

        public static void LoginUser(string username, string password)
        {
           LoginUserData login = new LoginUserData()
           {
               Username = username,
               AuthCode = Cryptography.CalculateSHA1(username, password)
           };

           sessionKey = HttpRequester.Post<string>(hostUrl + "user/login", login);
        }

        public static void Logout()
        {
            HttpRequester.Get<string>(hostUrl + "user/logout/" + sessionKey);
            sessionKey = string.Empty;
        }

        public static List<FullHistoryData> GetFullHistory()
        {
            var result = HttpRequester.Get<List<FullHistoryData>>(hostUrl + "user/fullhistory/" + sessionKey);
            return result;
        }

        public static Dictionary<DateTime, int> GetChartHistory(int days)
        {
            var result = HttpRequester.Get<Dictionary<DateTime, int>>(hostUrl + "user/historydiagram/" + sessionKey + "?days=" + days);
            return result;
        }

        public static void SendEmails(SendEmailData emailData)
        {
            HttpRequester.Post<Dictionary<DateTime, int>>(hostUrl + "user/send/" + sessionKey,emailData);
        }
    }
}

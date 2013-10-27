using Spammer.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Spammer.Classes;
using Spammer.DataLayer;
using System.Text;
using Typesafe.Mailgun;
using System.Net.Mail;

namespace Spammer.Services.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage Register([FromBody]RegisterUserData data)
        {
            if (data.Username.Length < Varibles.UsernameMinLenght)
            {
                var respons = this.Request.CreateResponse(HttpStatusCode.BadRequest, "Username is too short");
                return respons;
            }
            if (data.AuthCode.Length < Varibles.AuthoCodeLenght)
            {
                var respons = this.Request.CreateResponse(HttpStatusCode.BadRequest, "AuthoCode is invalid");
                return respons;
            }

            try
            {
                new MailAddress(data.Email);
            }
            catch(Exception ex)
            {
                var respons = this.Request.CreateResponse(HttpStatusCode.BadRequest, "Email is invalid");
                return respons;
            }

            SpamBaseEntities sqlCon = new SpamBaseEntities();

            using (sqlCon)
            {
                var findUser =
                    (from user in sqlCon.Users
                     where user.Username == data.Username
                     select user).FirstOrDefault();

                if (findUser != null)
                {
                    var respons = this.Request.CreateResponse(HttpStatusCode.BadRequest, "Username already exist");
                    return respons;
                }

                User newUser = new User()
                {
                    AuthCode = data.AuthCode,
                    Email = data.Email,
                    Username = data.Username
                };

                sqlCon.Users.Add(newUser);
                sqlCon.SaveChanges();

                string userSessionKey = this.GenerateSessionKey(newUser.Id);
                newUser.SessionKey = userSessionKey;

                sqlCon.SaveChanges();

                return this.Request.CreateResponse(HttpStatusCode.Created, userSessionKey);
            }
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage Login([FromBody]LoginUserData data)
        {
            SpamBaseEntities sqlCon = new SpamBaseEntities();

            using (sqlCon)
            {
                var findUser =
                    (from user in sqlCon.Users
                     where user.Username == data.Username
                     && user.AuthCode == data.AuthCode
                     select user).FirstOrDefault();

                if (findUser == null)
                {
                    var respons = this.Request.CreateResponse(HttpStatusCode.NotFound, "Username not found");
                    return respons;
                }

                string userSessionKey = this.GenerateSessionKey(findUser.Id);
                findUser.SessionKey = userSessionKey;

                sqlCon.SaveChanges();

                return this.Request.CreateResponse(HttpStatusCode.Created, userSessionKey);
            }
        }

        [HttpGet]
        [ActionName("logout")]
        public HttpResponseMessage Login(string sessionKey)
        {
            SpamBaseEntities sqlCon = new SpamBaseEntities();

            using (sqlCon)
            {
                var user = this.GetUserBySessionKey(sessionKey, sqlCon);

                if (user == null)
                {
                    var respons = this.Request.CreateResponse(HttpStatusCode.NotFound, "Username not found");
                    return respons;
                }

                user.SessionKey = null;
                sqlCon.SaveChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        [HttpPost]
        [ActionName("send")]
        public HttpResponseMessage SendEmails([FromBody]SendEmailData data, string sessionKey)
        {
            SpamBaseEntities sqlCon = new SpamBaseEntities();
            using (sqlCon)
            {
                var user = this.GetUserBySessionKey(sessionKey, sqlCon);
                if (user == null)
                {
                    var respons = this.Request.CreateResponse(HttpStatusCode.NotFound, "Username not found");
                    return respons;
                }

                foreach (var receiverEmail in data.Emails)
                {
                    this.EmailSender(user.Email, receiverEmail, data.Subject, data.Content);
                }

                if (data.Emails != null)
                {
                    SendEmail newSendEmails = new SendEmail()
                    {
                        Content = data.Content,
                        Recipients = string.Join(",", data.Emails.ToArray()),
                        SendDate = DateTime.Now,
                        UserId = user.Id,
                        Subject = data.Subject

                    };

                    sqlCon.SendEmails.Add(newSendEmails);
                    sqlCon.SaveChanges();
                }

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        [HttpGet]
        [ActionName("fullhistory")]
        public HttpResponseMessage GetFullHistory(string sessionKey)
        {
            SpamBaseEntities sqlCon = new SpamBaseEntities();

            using (sqlCon)
            {
                var user = this.GetUserBySessionKey(sessionKey, sqlCon);

                if (user == null)
                {
                    var respons = this.Request.CreateResponse(HttpStatusCode.NotFound, "Username not found");
                    return respons;
                }

                var request =
                    from sendEmails in sqlCon.SendEmails
                    where sendEmails.UserId == user.Id
                    select sendEmails;

                List<FullHistoryData> history = new List<FullHistoryData>();

                foreach (var item in request)
                {
                    FullHistoryData newHistory = new FullHistoryData()
                    {
                        Content = item.Content,
                        Recipients = item.Recipients,
                        SendDate = item.SendDate,
                        Subject = item.Subject
                    };

                    history.Add(newHistory);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, history);
            }
        }

        [HttpGet]
        [ActionName("historydiagram")]
        public HttpResponseMessage GetFullHistory(string sessionKey,int days)
        {
            SpamBaseEntities sqlCon = new SpamBaseEntities();

            using (sqlCon)
            {
                var user = this.GetUserBySessionKey(sessionKey, sqlCon);

                if (user == null)
                {
                    var respons = this.Request.CreateResponse(HttpStatusCode.NotFound, "Username not found");
                    return respons;
                }
                DateTime date = DateTime.Now.AddDays(days * -1);

                var request =
                    from sendEmails in sqlCon.SendEmails
                    where sendEmails.UserId == user.Id
                    && sendEmails.SendDate >= date
                    select sendEmails;

                Dictionary<DateTime, int> history = new Dictionary<DateTime, int>();

                foreach (var item in request)
                {
                    if(!history.ContainsKey(item.SendDate.Date))
                    {
                        history.Add(item.SendDate.Date, 0);
                    }

                    var emailsCount = item.Recipients.Split(',').Count();

                    history[item.SendDate.Date] += emailsCount;
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, history);
            }
        }

        private void EmailSender(string userEmail, string receiverEmail ,string subject, string content)
        {
            try
            {
                MailgunClient newMail = new MailgunClient(Varibles.ServerDomain, Varibles.MailApiKey);
                MailMessage newMessage = new MailMessage(userEmail, receiverEmail, subject, content);
                newMail.SendMail(newMessage);
            }
            catch (Exception ex)
            {
            }
        }

        private User GetUserBySessionKey(string sessionKey, SpamBaseEntities sqlcon)
        {
            var result =
                (from user in sqlcon.Users
                 where user.SessionKey == sessionKey
                 select user).FirstOrDefault();

            return result;
        }

        private string GenerateSessionKey(int id)
        {
            string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder result = new StringBuilder();
            result.Append(id);
            Random rand = new Random();

            for (int index = result.Length; index < Varibles.AuthoCodeLenght; index++)
            {
                int charIndex = rand.Next(0, symbols.Length);
                result.Append(symbols[charIndex]);
            }

            return result.ToString();
        }
    }
}
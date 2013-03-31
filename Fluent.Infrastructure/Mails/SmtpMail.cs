using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Mails
{

    public class SmtpMail
    {
        /// <summary>
        /// 格式：SmtpAccount:Password@SmtpServerAddress<br>
        /// 或者：SmtpServerAddress<br>
        /// <code>
        /// SmtpMail.SmtpServer="user:12345678@smtp.126.com";
        /// //或者:
        /// SmtpMail.SmtpServer="smtp.126.com"; 
        /// 或者：
        /// SmtpMail.SmtpServer=SmtpServerHelper.GetSmtpServer("user","12345678","smtp.126.com");
        /// </code>
        /// </summary>
        public static string SmtpServer { get; set; }

        public static bool Send(MailMessage mailMessage, string username, string password)
        {
            var helper = new SmtpServerHelper();
            return helper.SendEmail(SmtpServer, 25, username, password, mailMessage);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace KetabAbee.Application.Senders
{
    public class SendEmail
    {
        public static async Task Send(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.mail.yahoo.com");
            mail.From = new MailAddress("dahakdolatabad@yahoo.com", "فروشگاه آنلاین کتاب آبی");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("dahakdolatabad@yahoo.com", "dbfjlckbrfrbfsyw");
            SmtpServer.EnableSsl = true;

            await Task.Run(() =>
            {
                SmtpServer.SendAsync(mail, null);
            });
        }
    }
}
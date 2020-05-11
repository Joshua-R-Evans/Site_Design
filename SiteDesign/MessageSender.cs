using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SiteDesign
{
    public class MessageSender
    {
        private const string FromAddress = "itsjustsitedesign@gmail.com";
        private const string Password = "rrlnswwwzwbansmh";
        public static void SendEmail(string[] toEmailAddresses, string subject, string body)
        {
            using (var client = new SmtpClient())
            using (var msg = new MailMessage())
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(FromAddress, Password);
                client.EnableSsl = true;
                client.Port = 587;
                client.Host = "smtp.gmail.com";

                msg.From = new MailAddress(FromAddress);
                foreach (var address in toEmailAddresses)
                {
                    msg.To.Add(address);

                }                                           

                msg.Body = body;
                msg.Subject = subject;
                //msg.IsBodyHtml = true; //for stylized emails.
                //msg.Attachments.Add(attachments here);
            }
        }
    }
}
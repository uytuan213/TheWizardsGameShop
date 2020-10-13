using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TheWizardsGameShop
{
    /// <summary>
    /// A helper class to send email
    /// Reference: <see cref="http://www.binaryintellect.net/articles/e30d07c6-6f57-43e7-a2ce-6d2d67ebf403.aspx"/>
    /// </summary>
    public class EmailHelper
    {
        private static MailboxAddress adminEmail = new MailboxAddress("The Wizards Admin", "thewizardsgameshop@gmail.com");
        private const string EMAIL_ADDRESS = "thewizardsgameshop@gmail.com";
        private const string EMAIL_PASSWORD = "TheWizards3050!";

        public static void SendEmail(MailboxAddress recipient, string subject, BodyBuilder bodyBuilder)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(adminEmail);
            message.To.Add(recipient);

            message.Subject = subject;

            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(EMAIL_ADDRESS, EMAIL_PASSWORD);

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}

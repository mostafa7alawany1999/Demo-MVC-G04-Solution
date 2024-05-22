using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("mostafa7alawany@gmail.com", "wsuw pgmy tybe gqiw");
            client.Send("mostafa7alawany@gmail.com", email.Recipients, email.Subject, email.Body);
        }
    }
}

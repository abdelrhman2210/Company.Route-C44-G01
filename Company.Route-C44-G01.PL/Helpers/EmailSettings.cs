using System.Net;
using System.Net.Mail;

namespace Company.Route_C44_G01.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                //sfrdsardabdwwjiv
                smtpClient.Credentials = new NetworkCredential("abdelrhmanessam2210@gmail.com", "sfrdsardabdwwjiv");
                smtpClient.Send("abdelrhmanessam2210@gmail.com", email.To, email.Subject, email.Body);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}

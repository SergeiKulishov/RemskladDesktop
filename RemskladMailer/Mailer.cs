using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace RemskladMailer
{
    public class Mailer
    {
        public static async Task SendEmailAsync(string message= "")
        {
            MailAddress from = new MailAddress("sigurtholegsson@gmail.com", "Уведомление от Remsklad");
            MailAddress to = new MailAddress("rewq95kso@yandex.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Остатки на складе";
            m.Body = message;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("sigurtholegsson@gmail.com", "R911e007");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            Console.WriteLine("Письмо отправлено");
        }

        
    }
}

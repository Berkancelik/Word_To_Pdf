using System;
using System.IO;
using System.Net.Mail;

namespace Word_to_Pdf.Consumer
{
    internal class Program
    {
        public static bool EmailSend(string email, MemoryStream memoryStream, string fileName)
        {
            try
            {

          
            memoryStream.Position = 0;

            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);

            Attachment attach = new Attachment(memoryStream, ct);
            attach.ContentDisposition.FileName = $"{fileName}.pdf";

            MailMessage mailMessage = new MailMessage();

            SmtpClient smtpClient = new SmtpClient();

            mailMessage.From = new MailAddress("berkancelikist@gmail.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = "Pdf Dosyaso olulturma | nokta.com";
            mailMessage.Body = "pdf dosyanız ektedir";
            mailMessage.IsBodyHtml = true;
            mailMessage.Attachments.Add(attach);

            smtpClient.Host = "mail.orneksite.com";
            smtpClient.Port = 587;

            smtpClient.Credentials = new System.Net.NetworkCredential("admin@orneksite.com", "Parola");

            Console.WriteLine($"Sonuç: {email} adresine gönderilmiştir");
                memoryStream.Close();
                memoryStream.Dispose();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Mail gönderim sırasında bir hata meydana geldi:{ex.InnerException}");
                return false;
            }

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}

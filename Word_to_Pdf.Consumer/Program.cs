using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text;

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
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://dhqyhvke:5ukMea46fWrkSxEq53cPQxBqr9N92sEQ@moose.rmq.cloudamqp.com/dhqyhvke");

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.ExchangeDeclare("convert-exchange", ExchangeType.Direct, true, false, null);

                    channel.QueueBind(queue: "File", exchange: "convert-exchange", null);

                    channel.BasicQos(0, 1, false);

                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume("File", false, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        try
                        {
                            Console.WriteLine("Kuyruktan bir mesaj alındı ve işleniyor");

                            Document document = new Document();
                            string message = Encoding.UTF8.GetString(ea.Body);
                            MessageWordToPdf messageWordToPdf = JsonConvert.DeserializeObject<MessageWordToPdf>(message);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    };

                }
            }

        }
    }
}

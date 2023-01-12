using System.Net.Mail;
using System.Net;
using RequestTracker.Interfaces;
using RequestTracker.Models.BaseModels.RequestModels;
using RequestTracker.Models.BaseModels.ResponseModels;
using RequestTracker.Models.DBModels;

namespace RequestTracker.Services
{
    public interface IEmailService
    {
        void sendMail(string msg, string recipientEmail);
    }
    public class EmailService : IEmailService
    {
        public void sendMail(string msg, string recipientEmail)
        {
            try
            {
                var fromAddress = new MailAddress("donotreplyme1234@gmail.com");
                var toAddress = new MailAddress(recipientEmail);
                string fromPassword = "okcnmpqkrwkjuexg";
                string subject = "Registered for Heaven";
                string body = msg;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 50000

                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true

                })

                {
                    try
                    {
                        // Open the image file from directory and attach it to the email
                        var imagePath = "transparent.png";
                        var image = new Attachment(imagePath);
                        message.Attachments.Add(image);

                        // Set the Content-ID of the image
                        image.ContentId = "image1";

                        smtp.Send(message);
                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        //_logger.LogError(ex.FailedRecipient, this);
                        for (int i = 0; i < ex.InnerExceptions.Length; i++)
                        {
                            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                            if (status == SmtpStatusCode.MailboxBusy ||
                                status == SmtpStatusCode.MailboxUnavailable ||
                                status == SmtpStatusCode.TransactionFailed ||
                                status == SmtpStatusCode.ServiceNotAvailable ||
                                status == SmtpStatusCode.ServiceClosingTransmissionChannel ||
                                status == SmtpStatusCode.GeneralFailure)
                            {
                                Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                                System.Threading.Thread.Sleep(5000);
                                smtp.Send(message);
                            }
                            else
                            {
                                //_logger.LogError("Failed to deliver message to {0}",
                                //ex.InnerExceptions[i].FailedRecipient);
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                //_logger.LogError(ex.Message, this);
            }

        }
    }
}

using MailKit.Net.Smtp;
using MailKit.Security;
using server.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using server.DTOs;

namespace server.Utilites
{
    /// <summary>
    /// This class has been copied from here https://blog.christian-schou.dk/send-emails-with-asp-net-core-with-mailkit/
    /// </summary>
    public class MailUtility : IMailUtility
    {
        private readonly MailSettings _settings;

        public MailUtility(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SendEmailAsync(MailData mailData, CancellationToken ct = default)
        {
            try
            {
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
                mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

                foreach (string mailAddress in mailData.To)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                if (!string.IsNullOrEmpty(mailData.ReplyTo))
                    mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

                if (mailData.Bcc != null)
                {
                    foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }

                if (mailData.Cc != null)
                {
                    foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }

                var body = new BodyBuilder();
                mail.Subject = mailData.Subject;
                body.HtmlBody = mailData.Body;
                mail.Body = body.ToMessageBody();


                using var smtp = new SmtpClient();

                if (_settings.UseSSL)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_settings.UseStartTls)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                }
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);


                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
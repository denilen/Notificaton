using System;
using System.Threading.Tasks;
using Commons.Logging;
using Infrastructure.EmailGate.DataTypes;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailGate.Application.Email
{
    public interface IEmailMessageSender
    {
        public Task Send(SendEmailMessageCommand sendEmailMessageCommand);
    }

    public class EmailMessageSender : IEmailMessageSender
    {
        private static readonly ILogger Logger = LoggerFactory.Create<EmailMessageConsumer>();

        private readonly EmailMessageSenderConfiguration _configuration;
        private readonly RepeatableSendMessageGuard _repeatableSendMessageGuard;

        public EmailMessageSender(EmailMessageSenderConfiguration configuration,
                                  RepeatableSendMessageGuard repeatableSendMessageGuard)
        {
            _configuration              = configuration;
            _repeatableSendMessageGuard = repeatableSendMessageGuard;
        }

        public async Task Send(SendEmailMessageCommand sendEmailMessageCommand)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(_configuration.Sender, _configuration.Sender));
                email.To.Add(new MailboxAddress(sendEmailMessageCommand.Address, sendEmailMessageCommand.Address));
                email.Body = new TextPart("plain")
                {
                    Text = $@"{sendEmailMessageCommand.Message}"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(_configuration.BaseUrl, _configuration.BasePort, _configuration.UseSsl);
                await client.AuthenticateAsync(_configuration.Login, _configuration.Password);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
            catch (Exception exception)
            {
                Logger.Warn("Cannot send Email to '{EmailAddress}', error: {SendEmailException}",
                            sendEmailMessageCommand.Address, exception.Message);
                return;
            }
        }
    }
}

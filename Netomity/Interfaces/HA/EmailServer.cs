using Netomity.Core;
using Netomity.Core.Enum;
using Netomity.Interfaces.Basic;
using Netomity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces.HA
{
    public class EmailServer: HAInterface
    {

        string Address { get; set; }
        int Port { get; set; }
        bool IsSSL { get; set; }
        string FromAddress { get; set; }
        string FromAddressName { get; set; }
        string Password { get; set; }

        public EmailServer(string address=null, string fromAddress=null, string password=null, int port=0, bool isSSL=true, string fromAddressName=null)
            : base(iface: null)
        {
            Address = address;
            FromAddress = fromAddress;
            FromAddressName = fromAddressName != null ? fromAddressName : fromAddress;             
            Password = password;
            Port = port;
            IsSSL = isSSL;
        }


        public override async Task<bool> Command(Command command)
        {
            return await Task.Run(() => {
                if (command.Primary == CommandType.Notify)
                   SendEmail(command);
                return true;

            });

            
        }
        protected override List<Command> _DataToCommands(string data)
        {
            return new List<Command>();

        }

        private void SendEmail(Command command)
        {
            try
            {

                var fromAddress = new MailAddress(FromAddress, FromAddressName);
                var toAddressName = command.StringParams[NotificationParamType.ToAddressName];
                if (toAddressName == null)
                    toAddressName = command.StringParams[NotificationParamType.ToAddress];
                var toAddress = new MailAddress(command.StringParams[NotificationParamType.ToAddress], toAddressName);

                var smtp = new SmtpClient
                {
                    Host = Address,
                    Port = Port,
                    EnableSsl = IsSSL,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(FromAddress, Password),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = command.StringParams[NotificationParamType.Subject],
                    Body = command.Secondary,
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }


    }
}

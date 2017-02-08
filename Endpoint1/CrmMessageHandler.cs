namespace Endpoint1
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    public class CrmMessageHandler : IHandleMessages<CrmMessage>
    {
        static ILog log = LogManager.GetLogger<CrmMessageHandler>();

        public Task Handle(CrmMessage message, IMessageHandlerContext context)
        {
            log.Info($"Received CRM message id: {context.MessageId} (body id: {message.messageId})");

            return context.Send(new Message1
            {
                Property = $"CRM says hello. MessageName='{message.body.MessageName}'"
            });
        }
    }
}
namespace CRMAdapterEndpoint
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using CRMMapping.Messages;

    public class ContactUpdateHandler : IHandleMessages<ContactUpdate>
    {
        static ILog log = LogManager.GetLogger<ContactUpdateHandler>();

        public Task Handle(ContactUpdate message, IMessageHandlerContext context)
        {
            log.Info($"Received CRM ContactUpdate message id: {context.MessageId} (contact: {message.FullName})");
            System.Console.WriteLine($"Received CRM ContactUpdate message id: {context.MessageId} (contact: {message.FullName})");

            return Task.CompletedTask;
        }
    }
}
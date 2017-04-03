namespace CRMAdapterEndpoint
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using CustomerManagementMessages;
    using CRMMapping.Messages;

    public class ContactCreateHandler : IHandleMessages<ContactCreate>
    {
        static ILog log = LogManager.GetLogger<ContactCreateHandler>();

        public Task Handle(ContactCreate message, IMessageHandlerContext context)
        {
            log.Info($"Received CRM message id: {context.MessageId} (contact: {message.FullName})");

            var newCustomer = new NewCustomerReceived();
            newCustomer.ContactId = message.ContactId;
            newCustomer.CreatedById = message.CreatedById;
            newCustomer.CreateDate = message.CreateDate;
            newCustomer.FullName = message.FullName;
            newCustomer.FirstName = message.FirstName;
            newCustomer.LastName = message.LastName;
            newCustomer.Address = message.Address;
            newCustomer.Email = message.Email;

            log.Info($"Created new customer for Customer {newCustomer.ContactId}, Lastname {newCustomer.LastName}, createdby {newCustomer.CreatedById}");

            return context.Publish(newCustomer);
        }
    }
}
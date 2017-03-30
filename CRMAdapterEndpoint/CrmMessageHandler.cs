namespace CRMAdapterEndpoint
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    //using CRMAdapterEndpoint.Messages;
    using CustomerManagementMessages;
    using CRMMapping.Messages;

    public class CrmMessageHandler : IHandleMessages<ContactCreate>
    {
        static ILog log = LogManager.GetLogger<CrmMessageHandler>();

        public Task Handle(ContactCreate message, IMessageHandlerContext context)
        {
            log.Info($"Received CRM message id: {context.MessageId} (contact: {message.FullName})");

            //Messagename should be 'create'
            var NewCustomer = new NewCustomerReceived();

            NewCustomer.ContactId = message.ContactId;
            NewCustomer.CreatedById = message.CreatedById;
            DateTime createddate = message.CreateDate;
            NewCustomer.FullName = message.FullName;
            NewCustomer.FirstName = message.FirstName;
            NewCustomer.LastName = message.LastName;
            NewCustomer.Address = message.Address;
            NewCustomer.Email = message.Email;

            log.Info($"Created new customer for Customer {NewCustomer.ContactId}, Lastname {NewCustomer.LastName}, createdby {NewCustomer.CreatedById}");


            return context.Publish(NewCustomer);


        }

           }
}
namespace CRMAdapterEndpoint
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using CRMAdapterEndpoint.Messages;
    using CustomerManagementMessages;

    public class CrmMessageHandler : IHandleMessages<CrmMessage>
    {
        static ILog log = LogManager.GetLogger<CrmMessageHandler>();

        public Task Handle(CrmMessage message, IMessageHandlerContext context)
        {
            log.Info($"Received CRM message id: {context.MessageId} (body id: {message.CorrelationId})");

            //Messagename should be 'create'
            string messagename = message.MessageName;

            //The primary entity name should be 'Contact'
            // NOTE: We could do some sort of adapter/command pattern using the entity name here...
            string entityname = message.PrimaryEntityName;

            //Extract the contact information from the JSON based raw message and assign it into our message.
            var NewCustomer = new NewCustomerReceived();
            NewCustomer.ContactId = new Guid(message.PrimaryEntityId);
            NewCustomer.CreatedById = new Guid(message.InitiatingUserId);
            DateTime createddate = message.OperationCreatedOn;
            NewCustomer.FullName = GetCrmValue(message, "fullname");
            NewCustomer.FirstName = GetCrmValue(message, "firstname");
            NewCustomer.LastName = GetCrmValue(message, "lastname");
            NewCustomer.Address = GetCrmValue(message, "address1_composite");
            NewCustomer.Email = GetCrmValue(message, "emailaddress1");

            log.Info($"Created new customer for Customer {NewCustomer.ContactId}, Lastname {NewCustomer.LastName}, createdby {NewCustomer.CreatedById}");


            return context.Publish(NewCustomer);


        }

        private string GetCrmValue(CrmMessage message, string key)
        {
            var returnvalue = string.Empty;
            var attribute = message.InputParameters[0].value.Attributes.Find(x => x.key == key);

            if (attribute != null)
            {
                returnvalue = message.InputParameters[0].value.Attributes.Find(x => x.key == key).value.ToString();
            }

            return returnvalue;
        }
    }
}
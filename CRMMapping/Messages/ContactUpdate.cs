using System;
using NServiceBus;
using Microsoft.Xrm.Sdk;
using CRMMapping.CustomExtensions;
namespace CRMMapping.Messages
{
    public class ContactUpdate : IMessage
    {
        public Guid ContactId { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime CreateDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }


        public ContactUpdate(RemoteExecutionContext context)
        {

            ContactId = context.PrimaryEntityId;
            CreatedById = context.InitiatingUserId;
            CreateDate = context.OperationCreatedOn;

            //We can cast the 'Target' to a late bound CRM Entity type to parse it a bit easier.
            Entity entity = (Entity) context.InputParameters["Target"];

            FullName = this.GetCrmValue(entity,"fullname");
            FirstName = this.GetCrmValue(entity, "firstname");
            LastName = this.GetCrmValue(entity, "lastname");
            Address = this.GetCrmValue(entity, "address1_composite");
            Email = this.GetCrmValue(entity, "emailaddress1");
        }

    }
}

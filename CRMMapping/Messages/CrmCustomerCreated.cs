using System;
using NServiceBus;

namespace CRMMapping.Messages
{
    public class CrmCustomerCreated : IMessage
    {
        public Guid ContactId { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime CreateDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}

using System;
using NServiceBus;

namespace CustomerManagementMessages
{

    //The native Customer Service related messages that are to be shared with subscribers should go in this assembly. 
    
    public class NewCustomerReceived:IEvent
    {
        //Communicates the new customer from CRM inside our NServiceBus 
        public Guid ContactId { get; set; }
        public Guid CreatedById { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
      }


    public class CreateCustomerTaskRequest : ICommand
    {

        public Guid ContactId { get; set; }
        public Guid CreatedById { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Deadline { get; set; }
        
    }

    public class CreateCustomerTaskResponse : IMessage
    {
        public Guid ContactId { get; set; }
        public Guid TaskId { get; set; }

    }

    public class UpdateTaskRequest : ICommand
    {
        public Guid TaskId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool MarkComplete { get; set; }
        public Guid AssignedToUserId { get; set; }
       public Guid RelatedContactId { get; set; }
        
    }
    public class UpdateTaskResponse : IMessage
    {
        public Guid TaskId { get; set; }
        public Guid ContactId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
       
      }






}



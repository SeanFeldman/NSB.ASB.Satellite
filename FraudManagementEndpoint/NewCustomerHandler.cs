using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using CustomerManagementMessages;
using FraudManagementMessages;

namespace FraudManagementEndpoint
{
    class NewCustomerHandler : IHandleMessages<NewCustomerReceived>
    {
        public Task Handle(NewCustomerReceived message, IMessageHandlerContext context)
        {
            //In a full solution this endpoint Would create a command to send to a Fraud API Gateway.  
            //In this simulation we'll simply decide pass or failure based on the state the contact lives in. 
            FraudReviewResult fraudResult;

            if (message.FirstName.Contains("y"))
            {
                fraudResult = new FraudReviewResult { Success=false, ContactId=message.ContactId,ResponseDescription = "New contact has failed fraud review", ReferenceId = new Guid().ToString() };

                System.Console.WriteLine($"Failed Fraud Review of {message.FirstName} {message.LastName}.");
            }
            else
            {
                fraudResult = new FraudReviewResult { Success=true, ContactId = message.ContactId, ResponseDescription = "New customer has passed fraud review", ReferenceId = new Guid().ToString() };
                System.Console.WriteLine($"Successful Fraud Review of {message.FirstName} {message.LastName}.");

            }

            //Communicate to the other services the outcome of our work... 

            return context.Publish(fraudResult);


        }
    }
}

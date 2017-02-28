using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using CustomerManagementMessages;
using FraudManagementMessages;


namespace CustomerManagermentEndpoint
{
    public class sagaData : ContainSagaData
    {
       //The customer ID is the correlation between the new customer event and the fraud event.
        public Guid contactId { get; set; }
        public NewCustomerReceived newCustomerEvent {get; set;}
        public FraudReviewResult fraudResult { get; set; }
        public CreateCustomerTaskResponse taskcreatedResponse { get; set; }
        public UpdateTaskResponse taskupdatedResponse { get; set; }

    }

    public class CustomerManagementSaga : Saga<sagaData>, IAmStartedByMessages<NewCustomerReceived>, IAmStartedByMessages<FraudReviewResult>,  IHandleMessages<CreateCustomerTaskResponse>, IHandleMessages<UpdateTaskResponse>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<sagaData> mapper)
        {
            mapper.ConfigureMapping<NewCustomerReceived>(m => m.ContactId).ToSaga(s => s.contactId);
            mapper.ConfigureMapping<FraudReviewResult>(m => m.ContactId).ToSaga(s => s.contactId);
            mapper.ConfigureMapping<CreateCustomerTaskResponse>(m => m.ContactId).ToSaga(s => s.contactId);
            mapper.ConfigureMapping<UpdateTaskResponse>(m => m.TaskId).ToSaga(s => s.contactId);
        }


        public Task Handle(FraudReviewResult message, IMessageHandlerContext context)
        {
            Data.fraudResult = message;
            
            return CheckForTaskUpdate(context);
        }


        public Task Handle(NewCustomerReceived message, IMessageHandlerContext context)
        {
            //Save the entire message.  We'll need it. 
            Data.newCustomerEvent = message;
            System.Console.WriteLine($"New Customer {message.ContactId} Received. Lastname is {message.LastName}.  Creating a Task due in an hour");
            var newTaskRequest = new CreateCustomerTaskRequest { ContactId = Data.contactId, Description = "Customer Fraud Review", Subject = "Fraud Review Task", Deadline = DateTimeOffset.Now.AddHours(8) };

            return context.Send(newTaskRequest);

        }


        public Task Handle(CreateCustomerTaskResponse message, IMessageHandlerContext context)
        {
            Data.taskcreatedResponse = message;
            System.Console.WriteLine($"Task response received for {message.ContactId}");
            return CheckForTaskUpdate(context);
        }

        public Task Handle(UpdateTaskResponse message, IMessageHandlerContext context)
        {
            Data.taskupdatedResponse = message;
            MarkAsComplete();
            return Task.CompletedTask;
          }

       


        private Task CheckForTaskUpdate(IMessageHandlerContext context)
        {
            //If we have a response from the Task, we can update it. Can't be certain that the response is back before the fraud event arrives. 
            if ((Data.taskcreatedResponse != null) && (Data.fraudResult !=null))
            {
                UpdateTaskRequest updateTask = new UpdateTaskRequest();
                updateTask.TaskId = Data.taskcreatedResponse.TaskId;
                updateTask.Subject = "Fraud Review";
                updateTask.RelatedContactId = Data.contactId;

                System.Console.WriteLine($"Updating the task {updateTask.TaskId}. Mark Complete will be {Data.fraudResult.Success}");

                var fraudDetail = Data.fraudResult.ResponseDescription;

                if (Data.fraudResult.Success==true)
                {
                    updateTask.Description = $"Automated Fraud Review Successful. Detail:{fraudDetail}";
                    updateTask.MarkComplete = true;

                }
                else
                {
                    updateTask.Description = $"Automated Fraud Review Failure.  It's up to you now!. Detail:{fraudDetail}";
                    updateTask.MarkComplete = false;
                   // updateTask.AssignedToUserId = Data.newCustomerEvent.CreatedById;

                }

                return context.Send(updateTask);
            }
            else
            {
                return Task.CompletedTask;

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using CustomerManagementMessages;

namespace CRMApiGatewayEndpoint
{
    class UpdateTaskHandler : IHandleMessages<UpdateTaskRequest>
    {
        private CRMApiManager apiManager;

        public UpdateTaskHandler(CRMApiManager apiManager)
        {

            this.apiManager = apiManager;
        }
        public async Task Handle(UpdateTaskRequest message, IMessageHandlerContext context)
        {

            System.Console.WriteLine($"Api manager updated task {message.TaskId}.");
            var contactId = await apiManager.UpdateTask(message.TaskId,message.MarkComplete,message.AssignedToUserId,message.Description);

            contactId = message.RelatedContactId;
            await context.Reply(new UpdateTaskResponse { ContactId = contactId, TaskId = message.TaskId, Message = "", Success = true });


           
        }
    }
}

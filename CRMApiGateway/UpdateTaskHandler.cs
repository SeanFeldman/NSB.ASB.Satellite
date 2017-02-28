namespace CRMApiGatewayEndpoint
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using CustomerManagementMessages;

    class UpdateTaskHandler : IHandleMessages<UpdateTaskRequest>
    {
        private CRMApiManager apiManager;

        public UpdateTaskHandler(CRMApiManager apiManager)
        {
            this.apiManager = apiManager;
        }

        public async Task Handle(UpdateTaskRequest message, IMessageHandlerContext context)
        {

            Console.WriteLine($"Api manager updated task {message.TaskId}.");
            var contactId = await apiManager.UpdateTask(message.TaskId,message.MarkComplete,message.AssignedToUserId,message.Description).ConfigureAwait(false);

            contactId = message.RelatedContactId;
            await context.Reply(new UpdateTaskResponse { ContactId = contactId, TaskId = message.TaskId, Message = "", Success = true }).ConfigureAwait(false);
        }
    }
}

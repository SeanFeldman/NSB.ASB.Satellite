namespace Endpoint1
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System;
    

    public class CrmMessageHandler : IHandleMessages<CrmMessage>
    {
        static ILog log = LogManager.GetLogger<CrmMessageHandler>();

        public Task Handle(CrmMessage message, IMessageHandlerContext context)
        {
            log.Info($"Received CRM message id: {context.MessageId} (body id: {message.messageId})");
            
            //Extrace the contact information from the JSON based raw message. 

            string contactid = message.body.PrimaryEntityId;
            string messagename = message.body.MessageName;
            string createdbyid = message.body.InitiatingUserId;
            string fullname = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "fullname").value.ToString();
            string firstname = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "firstname").value.ToString();
            string lastname = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "lastname").value.ToString();
            string fulladdress = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "address1_composite").value.ToString();
            string emailaddress = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "emailaddress1").value.ToString();
            DateTime createddate = DateTime.Parse(message.body.OperationCreatedOn);



            return context.Send(new Message1
            {
                Property = $"CRM says hello. New Contact is '{fullname}'"

            });
        }
    }
}
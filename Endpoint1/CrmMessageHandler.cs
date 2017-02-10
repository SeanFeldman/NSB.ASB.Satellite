namespace Endpoint1
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;

    public class CrmMessageHandler : IHandleMessages<CrmMessage2>
    {
        static ILog log = LogManager.GetLogger<CrmMessageHandler>();

        public Task Handle(CrmMessage2 message, IMessageHandlerContext context)
        {
            log.Info($"Received CRM message id: {context.MessageId} (body id: {message.messageId})");
            //dynamic json = JObject.Parse(message.body.InputParameters[0].ToString());
           
            //string[] storeNames = json.SelectToken("Stores").Select(s => (string)s).ToArray();

            string contactid = message.body.PrimaryEntityId;
            string messagename = message.body.MessageName;
            string createdbyid = message.body.InitiatingUserId;
            string fullname = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "fullname").value.ToString();
            string firstname = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "firstname").value.ToString();
            string lastname = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "lastname").value.ToString();
            string fulladdress = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "address1_composite").value.ToString();
            string emailaddress = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "emailaddress1").value.ToString();
                        
            return context.Send(new Message1
            {
                Property = $"CRM says hello. MessageName='{message.body.MessageName}'"

            });
        }
    }
}
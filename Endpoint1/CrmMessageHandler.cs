﻿namespace Endpoint1
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
            DateTime createddate = DateTime.Parse(message.body.OperationCreatedOn);
            //string fullname = message.body.InputParameters[0].value.Attributes.Find(x => x.key == "fullname").value.ToString();
            string fullname = GetCrmValue(message,"fullname");
            string firstname = GetCrmValue(message, "firstname");
            string lastname = GetCrmValue(message, "lastname");
            string fulladdress = GetCrmValue(message, "address1_composite");
            string emailaddress = GetCrmValue(message, "emailaddress1");
          
            return context.Send(new Message1
            {
                Property = $"CRM says hello. New Contact is '{fullname}'"

            });

            
        }

        private string GetCrmValue(CrmMessage message, string key)
        {
            var returnvalue = string.Empty;
            var attribute = message.body.InputParameters[0].value.Attributes.Find(x => x.key == key);

            if (attribute != null)
            {
                returnvalue = message.body.InputParameters[0].value.Attributes.Find(x => x.key == key).value.ToString();

            }

            return returnvalue;
        }
    }
}
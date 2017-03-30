using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using CRMMapping.Messages;
using Microsoft.Xrm.Sdk;
using NServiceBus;
using System.Globalization;
using System.Threading;
using System.Reflection;
using Newtonsoft.Json;

namespace CRMMapping
{
    public static class Mapper
    {
        public static MappingResult Map(Dictionary<string, string> messageHeaders, byte[] crmRawMessage)
        {
            // Deserialize CRM message into RemoteExecutionContext
            var stream = new MemoryStream(crmRawMessage);
            var remoteExecutionContext = (RemoteExecutionContext)new DataContractJsonSerializer(typeof(RemoteExecutionContext)).ReadObject(stream);

            //Get the Entity and Action from the header in the raw CRM message from Azure. 
            var entityName = messageHeaders["http://schemas.microsoft.com/xrm/2011/Claims/EntityLogicalName"];
            var entityAction = messageHeaders["http://schemas.microsoft.com/xrm/2011/Claims/RequestName"];

            var mapperTypeName =  entityName.ToLower() + entityAction.ToLower();  //"CRMMapping.Messages."+

            IMessage targetMessage = null;

            switch (mapperTypeName)
            {
                case "contactcreate":
                    //The mapping will be done in the constructor
                    targetMessage = new ContactCreate(remoteExecutionContext);

                    break;

                case "contactupdate":

                    break;

                default:
                    //I'll throw some map not found exception and configure it to not be retried. 
                    break;
            }

            // serialize the message

            var serializedObject = JsonConvert.SerializeObject(targetMessage);
            var bytes = System.Text.Encoding.UTF8.GetBytes(serializedObject);

            
            return new MappingResult(bytes, targetMessage.GetType().FullName);
        }
    }
}
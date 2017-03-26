using System.IO;
using System.Runtime.Serialization.Json;
using CRMMapping.Messages;
using Microsoft.Xrm.Sdk;
using NServiceBus;

namespace CRMMapping
{
    public static class Mapper
    {
        public static MappingResult Map(byte[] crmRawMessage)
        {
            // Deserialize CRM message into RemoteExecutionContext
            var stream = new MemoryStream(crmRawMessage);
            var remoteExecutionContext = (RemoteExecutionContext)new DataContractJsonSerializer(typeof(RemoteExecutionContext)).ReadObject(stream);

            // map it to a known NSB type
            //                var createContact = new CrmCreateContact();
            //                var parameter = remoteExecutionContext.InputParameters.First();
            //                var parameterValue = (Value)parameter.Value;
            //                var fullName = parameterValue.Attributes.FirstOrDefault(attribute => attribute.key == "fullname");
            //                createContact.FullName = (string)fullName.value;

            // serialize the message
            //                var serializedObject = JsonConvert.SerializeObject(createContact);
            //                var bytes = Encoding.UTF8.GetBytes(serializedObject);

            // update the body of the incoming message
            //                context.UpdateMessage(bytes);

            // set the incoming message type header for NSB
            //                 context.Message.Headers[Headers.EnclosedMessageTypes] = typeof(CrmCreateContact).AssemblyQualifiedName

            return new MappingResult(new byte[0], typeof(CrmCustomerCreated).AssemblyQualifiedName);
        }
    }
}
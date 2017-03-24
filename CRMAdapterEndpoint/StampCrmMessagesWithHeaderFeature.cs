using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using NServiceBus.Unicast.Messages;

namespace CRMAdapterEndpoint
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;
    using CRMAdapterEndpoint.Messages;

    public class StampCrmMessagesWithHeaderFeature : Feature
    {
        internal StampCrmMessagesWithHeaderFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var pipeline = context.Pipeline;
            pipeline.Register<StampCrmMessagesWithHeaderRegistration>();
        }

        static string SerializeEnclosedMessageTypes(MessageMetadata metadata)
        {
            var assemblyQualifiedNames = new HashSet<string>();
            foreach (var type in metadata.MessageHierarchy)
            {
                assemblyQualifiedNames.Add(type.AssemblyQualifiedName);
            }

            return string.Join(";", assemblyQualifiedNames);
        }
    }

    public class StampCrmMessagesWithHeaderRegistration : RegisterStep
    {
        public StampCrmMessagesWithHeaderRegistration() : base("StampCrmMessagesWithHeader", typeof(StampCrmMessagesWithHeaderBehavior), "Stamp incoming CRM messages with NSB header to allow processing in a handler")
        {
        }
    }

    public class StampCrmMessagesWithHeaderBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            //Quick check to see if this is a native message.  We don't want to alter the type otherwise.
            if (!context.Message.Headers.ContainsKey("NServiceBus.EnclosedMessageTypes"))
            {
                context.Message.Headers[Headers.EnclosedMessageTypes] = typeof(CrmMessage).AssemblyQualifiedName;

                // Deserialize CRM message into RemoteExecutionContext
                var stream = new MemoryStream(context.Message.Body);
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
            }
            return next();
        }
    }

    public class CrmCreateContact : IMessage
    {
        public string FullName { get; set; }
    }
}
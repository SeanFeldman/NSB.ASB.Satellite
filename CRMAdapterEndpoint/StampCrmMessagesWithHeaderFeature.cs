using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Xrm.Sdk;

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
                var remoteExecutionContext = new DataContractJsonSerializer(typeof(RemoteExecutionContext)).ReadObject(stream);
            }
            return next();
        }
    }
}
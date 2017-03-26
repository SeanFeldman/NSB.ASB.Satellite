using System.Collections.Generic;
using CRMAdapterEndpoint.Messages;
using CRMMapping;
using NServiceBus.Unicast.Messages;

namespace CRMAdapterEndpoint
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;

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
                var mappingResult = Mapper.Map(context.Message.Body);
               
                context.Message.Headers[Headers.EnclosedMessageTypes] = typeof(CrmMessage).AssemblyQualifiedName;

                // this will replace the header update above
//                context.Message.Headers[Headers.EnclosedMessageTypes] = mappingResult.TypeHeaderValue;
//                context.UpdateMessage(mappingResult.SerializedMessageBody);
            }
            return next();
        }
    }

    public class CrmCreateContact : IMessage
    {
        public string FullName { get; set; }
    }
}
namespace Endpoint1
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Features;
    using NServiceBus.MessageInterfaces.MessageMapper.Reflection;
    using NServiceBus.ObjectBuilder;
    using NServiceBus.Routing;
    using NServiceBus.Serialization;
    using NServiceBus.Transport;
    using NServiceBus.Unicast.Messages;

    public class CrmSatelliteFeature : Feature
    {
        private IMessageSerializer messageSerializer;
        private string messageTypesHeaderValue;

        public CrmSatelliteFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var jsonSerializerDefinition = new JsonSerializer();
            var factory = jsonSerializerDefinition.Configure(context.Settings);
            var messageMapper = new MessageMapper();
            var messageMetadataRegistry = context.Settings.Get<MessageMetadataRegistry>();
            var message2Metadata = messageMetadataRegistry.GetMessageMetadata(typeof(Message2));
            messageTypesHeaderValue = SerializeEnclosedMessageTypes(message2Metadata);
            messageMapper.Initialize(new [] { message2Metadata.MessageType });
            messageSerializer = factory(messageMapper);

            context.AddSatelliteReceiver(
                name: "CrmSatellite",
                transportAddress: "myqueue", 
                runtimeSettings: PushRuntimeSettings.Default, 
                recoverabilityPolicy: (config, errorContext) =>
                {
                    return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue); // could be a custom queue as well
                },
                onMessage: OnMessage);
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

        private Task OnMessage(IBuilder builder, MessageContext context)
        {
            var dispatcher = builder.Build<IDispatchMessages>();

            var message = new Message2 { Property = $"message from Satellite. Handled native message ID:{context.MessageId}." };

            using (var stream = new MemoryStream())
            {
                messageSerializer.Serialize(message, stream);

                var headers = new Dictionary<string, string> { {Headers.EnclosedMessageTypes, messageTypesHeaderValue} };
                var outgoingMessage = new OutgoingMessage(Guid.NewGuid().ToString(), headers, stream.ToArray());
                var transportOperation = new TransportOperation(outgoingMessage, new UnicastAddressTag("samples.azure.servicebus.endpoint1"));
                return dispatcher.Dispatch(new TransportOperations(transportOperation), new TransportTransaction(), new ContextBag());
            }
        }
    }
}
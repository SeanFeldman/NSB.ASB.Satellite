namespace Endpoint1
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.ObjectBuilder;
    using NServiceBus.Transport;

    public class CrmSatelliteFeature : Feature
    {
        public CrmSatelliteFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.AddSatelliteReceiver(
                name: "CrmSatellite",
                transportAddress: "myqueue", 
                runtimeSettings: PushRuntimeSettings.Default, 
                recoverabilityPolicy: (config, errorContext) =>
                {
                    return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue); // could be a custom queue as well
                },
                onMessage: OnMessage);
            var hasComponent = context.Container.HasComponent<IMessageSession>();
        }

        private Task OnMessage(IBuilder builder, MessageContext context)
        {
            var messageId = context.MessageId;
            Console.WriteLine(messageId);
            return Program.MessageSession.Send("samples.azure.servicebus.endpoint1", new Message2 {Property = $"message from Satellite. Handled native message ID:{messageId}."});
        }
    }
}
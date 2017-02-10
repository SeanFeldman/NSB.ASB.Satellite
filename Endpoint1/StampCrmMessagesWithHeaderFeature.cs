namespace Endpoint1
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
            context.Message.Headers[Headers.EnclosedMessageTypes] = typeof(CrmMessage2).AssemblyQualifiedName;
            return next();
        }
    }
}